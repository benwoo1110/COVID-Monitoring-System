//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.IO;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.Utilities;

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
            BoundingBox = {Top = 0}
        };

        private string cachedFilePath;

        public GenerateSHNReportScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) :
            base(displayManager, covidManager)
        {
            openFile.BoundingBox.SetRelativeElement(result);
        }

        public override void PreLoad()
        {
            openFile.Hidden = true;
            cachedFilePath = string.Empty;
        }

        [OnClick("generate")]
        private void OnGenerateReport([InputParam("reportDate", "result")] DateTime targetDate)
        {
            var reportResult = CovidManager.GenerateSHNStatusReportFile(targetDate);
            if (reportResult.IsSuccessful())
            {
                result.Text =
                    $"SHN report on {targetDate} has been successfully generated. File has been saved to '{reportResult.FilePath}'.";
                openFile.Hidden = false;
                cachedFilePath = reportResult.FilePath;
                ClearAllInputs();
                return;
            }

            result.Text = $"There was an error generating the report. {reportResult.GetErrorMessage()}";
            openFile.Hidden = true;
            cachedFilePath = string.Empty;
        }

        [OnClick("openFile")]
        private void OnOpenReportFile()
        {
            if (string.IsNullOrEmpty(cachedFilePath))
            {
                return;
            }

            CoreHelper.OpenFile(cachedFilePath);
        }
    }
}