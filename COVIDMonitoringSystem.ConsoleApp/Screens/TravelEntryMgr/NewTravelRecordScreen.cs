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

        private Header header = new Header("header")
        {
            Text = "New Travel Record",
            BoundingBox = {Top = 0}
        };
        private Input name = new Input("name")
        {
            Prompt = "Name",
            BoundingBox = {Top = 4}
        };
        private Input country = new Input("country")
        {
            Prompt = "Last Country of Embark",
            BoundingBox = {Top = 5}
        };
        private Input entryMode = new Input("entryMode")
        {
            Prompt = "Entry Mode",
            BoundingBox = {Top = 6}
        };
        private Input entryDate = new Input("entryDate")
        {
            Prompt = "Entry Date",
            BoundingBox = {Top = 7}
        };
        private Label shnMessage = new Label("shnMessage")
        {
            Text = "Some message",
            BoundingBox = {Top = 8},
        };
        private Input shnFacility = new Input("shnFacility")
        {
            Prompt = "SHN Facility Name",
            BoundingBox = {Top = 0},
        };
        private Button create = new Button("create")
        {
            Text = "[Create Record]",
            BoundingBox = {Top = 1}
        };
        private Label result = new Label("result")
        {
            Text = "Result here.",
            BoundingBox = {Top = 1}
        };
        
        public NewTravelRecordScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            shnFacility.BoundingBox.SetRelativeBox(shnMessage);
            create.BoundingBox.SetRelativeBox(shnFacility);
            result.BoundingBox.SetRelativeBox(create);
        }

        public override void PreLoad()
        {
            shnMessage.Hidden = true;
            shnFacility.Hidden = true;
        }

        [OnEnterInput("country")]
        private void OnUpdateCountry([Parser("country")] SHNTier tier)
        {
            if (tier == SHNTier.Dedicated)
            {
                shnMessage.Hidden = false;
                shnFacility.Hidden = false;
                shnMessage.Text = $"\nYou are required to serve SHN in dedicated facility when coming from {country.Text}.";
                return;
            }

            shnMessage.Hidden = true;
            shnFacility.Hidden = true;
        }

        [OnClick("create")]
        private void OnCreateRecord(
            [Parser("name", "result")] Person person,
            [Parser("country", "result")] string countryName,
            [Parser("entryMode", "result")] TravelEntryMode mode,
            [Parser("entryDate", "result")] DateTime entryTime,
            [Parser("shnFacility", "result")] SHNFacility facility)
        {
            
        }
    }
}