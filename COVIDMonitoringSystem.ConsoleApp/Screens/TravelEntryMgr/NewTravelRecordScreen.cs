//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class NewTravelRecordScreen : CovidScreen
    {
        public override string Name => "newTravelRecord";

        private Header header = new Header
        {
            Text = "New Travel Record",
            BoundingBox = {Top = 0}
        };

        private Input name = new Input
        {
            Prompt = "Name",
            BoundingBox = {Top = 4}
        };

        private Input country = new Input
        {
            Prompt = "Last Country of Embark",
            BoundingBox = {Top = 5},
            SuggestionType = "countries"
        };

        private Input entryMode = new Input
        {
            Prompt = "Entry Mode",
            BoundingBox = {Top = 6}
        };

        private Input entryDate = new Input
        {
            Prompt = "Entry Date",
            BoundingBox = {Top = 7}
        };

        private Label shnMessage = new Label
        {
            Text = "Some message",
            BoundingBox = {Top = 8},
        };

        private Input shnFacility = new Input
        {
            Prompt = "SHN Facility Name",
            BoundingBox = {Top = 0},
        };

        private Button create = new Button
        {
            Text = "[Create Record]",
            BoundingBox = {Top = 1}
        };

        private Label result = new Label
        {
            BoundingBox = {Top = 1},
            ClearOnExit = true
        };

        public NewTravelRecordScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            shnFacility.BoundingBox.SetRelativeElement(shnMessage);
            create.BoundingBox.SetRelativeElement(shnFacility);
            result.BoundingBox.SetRelativeElement(create);
        }

        public override void PreLoad()
        {
            shnMessage.Hidden = true;
            shnFacility.Hidden = true;
        }

        [OnEnterInput("country")]
        private void OnUpdateCountry([InputParam("country")] SHNTier tier)
        {
            if (tier == SHNTier.Dedicated)
            {
                shnMessage.Hidden = false;
                shnFacility.Hidden = false;
                shnMessage.Text =
                    $"\nYou are required to serve SHN in dedicated facility when coming from {country.Text}.";
                return;
            }

            shnMessage.Hidden = true;
            shnFacility.Hidden = true;
        }

        [OnClick("create")]
        private void OnCreateRecord(
            [InputParam("name", "result")] Person person,
            [InputParam("country", "result")] [Values("countries")] string lastEmbarkCountry,
            [InputParam("entryMode", "result")] TravelEntryMode mode,
            [InputParam("entryDate", "result")] DateTime entryTime,
            [InputParam("shnFacility", "result")] SHNFacility facility)
        {
            var travelEntry = new TravelEntry(person, lastEmbarkCountry, mode, entryTime);

            if (travelEntry.Conditions.RequireDedicatedFacility)
            {
                if (facility == null)
                {
                    result.Text = "You are required to specify a dedicated SHN facility to stay in.";
                    return;
                }

                travelEntry.AssignSHNFacility(facility);
            }
            
            person.AddTravelEntry(travelEntry);
            
            result.Text = $"A new travel entry record has been successfully added for {person.Name}.";
            ClearAllInputs();
        }
    }
}