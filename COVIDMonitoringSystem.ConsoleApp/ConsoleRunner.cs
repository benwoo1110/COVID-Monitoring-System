﻿//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Screens.General;
using COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr;
using COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class ConsoleRunner
    {
        private COVIDMonitoringManager Manager { get; }

        private ConsoleDisplayManager DisplayManager { get; }

        public ConsoleRunner()
        {
            DisplayManager = new ConsoleDisplayManager();
            Manager = new COVIDMonitoringManager();
            RegisterInputResolvers();
            RegisterInputValueTypes();
            SetUpMenus();
            SetUpGeneralScreens();
            SetUpSafeEntryScreens();
            SetUpTravelEntryScreens();
        }

        private void RegisterInputResolvers()
        {
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPerson,
                "No person with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPersonOfType<Resident>,
                "No resident with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPersonOfType<Visitor>,
                "No visitor with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindBusinessLocation,
                "No business with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterInputResolver<SHNFacility>(
                (screen, value) =>
                {
                    var facility = Manager.FindSHNFacility(value);
                    if (facility == null)
                    {
                        throw new InputParseFailedException($"No SHN facility with name '{value}' found.");
                    }
                    if (!facility.IsAvailable())
                    {
                        throw new InputParseFailedException($"{value} is full. Please choose another SHN facility.");
                    }

                    return facility;
                }
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                SHNTier.FindAppropriateTier,
                "This shouldn't happen. Input was: {0}"
            );
            DisplayManager.ResolveManager.RegisterQuickEnumResolver<TravelEntryMode>("entry mode");
        }

        private void RegisterInputValueTypes()
        {
            DisplayManager.ValuesManager.RegisterInputValueType(
                "countries", (
                screen) => Manager.ValidCountries, 
                "{0} is not a valid country."
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "collectLocation", 
                screen => Manager.ValidCollectionLocation,
                "{0} is not a valid collection location."
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "nationality",
                screen => Manager.ValidNationalities,
                "{0} is not a valid nationality."
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "person",
                screen => Manager.PersonList.ConvertAll(person => person.Name)
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "resident",
                screen => Manager.GetAllPersonOfType<Resident>().ConvertAll(resident => resident.Name)
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "visitor",
                screen => Manager.GetAllPersonOfType<Visitor>().ConvertAll(visitor => visitor.Name)
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "businessLocation",
                screen => Manager.BusinessLocationList.ConvertAll(business => business.BusinessName)
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "shnFacility",
                screen => Manager.GetAvailableSHNFacility().ConvertAll(facility => facility.FacilityName)
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "entryMode",
                screen => Enum.GetNames(typeof(TravelEntryMode)).ToList()
            );
            DisplayManager.ValuesManager.RegisterInputValueType(
                "checkOutLocation",
                screen =>
                {
                    if (!(screen is CheckOutScreen checkOutScreen) || checkOutScreen.CachePerson == null)
                    {
                        return new List<string>();
                    }
                    
                    var latestCheckinDate = new List<DateTime>();
                    var latestCheckoutDate = new List<DateTime>();
                    var result = new List<string>();
                    foreach (var i in checkOutScreen.CachePerson.SafeEntryList)
                    {
                        latestCheckinDate.Add(i.CheckIn);
                        latestCheckoutDate.Add(i.CheckOut);
                        if (latestCheckinDate.Max() > latestCheckoutDate.Max())
                        {
                            result.Add(i.Location.BusinessName);
                        }
                    }

                    return result;
                }
            );
        }

        private void SetUpMenus()
        {
            DisplayManager.RegisterScreen(new MainMenuBuilder(DisplayManager)
                .OfName("mainMenu")
                .WithHeader("COVID Management System")
                .AddOption("View Details of a Person", "viewPersonDetails")
                .AddOption("View All Visitors", "viewAllVisitors")
                .AddOption("Safe Entry Management", "safeEntryMenu")
                .AddOption("Travel Entry Management", "travelEntryMenu")
                .AddOption("Explore Global Stats", "globalStats")
                .Build()
            );
            DisplayManager.RegisterScreen(new MenuBuilder(DisplayManager)
                .OfName("safeEntryMenu")
                .WithHeader("Travel Entry Management")
                .AddOption("Assign or replace TraceTogether Token", "assignToken")
                .AddOption("View all business locations", "viewLocations")
                .AddOption("Change capacity of business location", "changeCapacity")
                .AddOption("SafeEntry Check-In", "checkIn")
                .AddOption("SafeEntry Check-Out", "checkOut")
                .AddOption("Generate Contact Tracing Report", "contactTrace")
                .Build()
            );
            DisplayManager.RegisterScreen(new MenuBuilder(DisplayManager)
                .OfName("travelEntryMenu")
                .WithHeader("Travel Entry Management")
                .AddOption("View All SHN Facilities", "viewFacilities")
                .AddOption("New Visitor", "newVisitor")
                .AddOption("New Travel Record", "newTravelRecord")
                .AddOption("Pay SHN Charges", "viewSHNCharges")
                .AddOption("Generate SHN Status Report", "shnReport")
                .Build()
            );
        }

        private void SetUpGeneralScreens()
        {
            DisplayManager.RegisterScreen(new PersonDetailsScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ListScreenBuilder<Visitor>(DisplayManager)
                .OfName("viewAllVisitors")
                .WithHeader("View All Visitors")
                .WithProperties(new[] {"Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList"})
                .WithGetter(() => Manager.GetAllPersonOfType<Visitor>())
                .Build()
            );
            DisplayManager.RegisterScreen(new GlobalStatsScreen(DisplayManager, Manager));
        }

        private void SetUpSafeEntryScreens()
        {
            DisplayManager.RegisterScreen(new TokenScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ListScreenBuilder<BusinessLocation>(DisplayManager)
                .OfName("viewLocations")
                .WithHeader("View All Business Location")
                .WithProperties(new[] {"BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow"})
                .WithGetter(() => Manager.BusinessLocationList)
                .Build()
            );
            DisplayManager.RegisterScreen(new EditCapacityScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new CheckInScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new CheckOutScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ContactTraceScreen(DisplayManager, Manager));
        }

        private void SetUpTravelEntryScreens()
        {
            DisplayManager.RegisterScreen(new ListScreenBuilder<SHNFacility>(DisplayManager)
                .OfName("viewFacilities")
                .WithHeader("View All SHN Facilities")
                .WithProperties(new[] {"FacilityName", "FacilityCapacity", "FacilityVacancy", "FromLand", "FromSea", "FromAir"})
                .WithGetter(() => Manager.SHNFacilitiesList)
                .Build()
            );
            DisplayManager.RegisterScreen(new NewVisitorScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new NewTravelRecordScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ViewSHNChargesScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new PaySHNChargesScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new GenerateSHNReportScreen(DisplayManager, Manager));
        }

        public void Run()
        {
            Console.SetWindowSize(128, 40);
            DisplayManager.Run("mainMenu");
        }
    }
}