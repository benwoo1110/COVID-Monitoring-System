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

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class GenerateSHNReportScreen : CovidScreen
    {
        public override string Name => "shnReport";

        private Header header = new Header("header")
        {
            Text = "Generate SHN Report CSV",
            BoundingBox = {Top = 0}
        };
        private Input reportDate = new Input("reportDate")
        {
            Prompt = "Report Date",
            BoundingBox = {Top = 4}
        };
        private Button generate = new Button("generate")
        {
            Text = "[Generate]",
            BoundingBox = {Top = 6}
        };
        private Label result = new Label("result")
        {
            BoundingBox = {Top = 8},
            ClearOnExit = true
        };

        private Button openFile = new Button("openFile")
        {
            Text = "[Open File]",
            BoundingBox = {Top = 1},
            Hidden = true
        };
        
        public GenerateSHNReportScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            openFile.BoundingBox.SetRelativeBox(result);
        }

        [OnClick("generate")]
        private void OnGenerateReport([Parser("reportDate", "result")] DateTime targetDate)
        {
            //TODO: More detailed return for a CSV file generation.
            if (CovidManager.GenerateSHNStatusReportFile(targetDate))
            {
                result.Text = "File has been created.";
                openFile.Hidden = false;
                ClearAllInputs();
                return;
            }

            result.Text = "There was an error generating the report.";
            openFile.Hidden = true;
        }
    }
}