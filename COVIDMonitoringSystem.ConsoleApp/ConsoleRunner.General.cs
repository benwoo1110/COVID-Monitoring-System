using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Screens;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void SetUpGeneralScreens()
        {
            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewPersonDetails")
                .WithHeader("View Details of a Person")
                .AddElement(new Input("name", "Name"))
                .AddElement(new Spacer())
                .AddElement(new Button("find")) // , "[Find]", ShowPersonDetails))
                .AddElement(new Spacer())
                .AddElement(new Label("details"))
                .Build()
            );
            
            DisplayManager.RegisterScreen(new ListScreenBuilder<Visitor>(DisplayManager)
                .OfName("viewAllVisitors")
                .WithHeader("View All Visitors")
                .WithProperties(new[] {"Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList"})
                .WithGetter(() => Manager.GetAllPersonOfType<Visitor>())
                .Build()
            );
            
            DisplayManager.RegisterScreen(new GlobalStatsScreen(DisplayManager, Manager));
        }

        private void ShowPersonDetails(AbstractScreen abstractScreen)
        {
            var nameInput = abstractScreen.FindElementOfType<Input>("name");
            var detailInfo = abstractScreen.FindElementOfType<Label>("details");

            var targetPerson = Manager.FindPerson(nameInput.Text);
            if (targetPerson == null)
            {
                detailInfo.Text = "Person not found.";
                return;
            }

            var personInfo = new DetailsBuilder();
            personInfo.AddInfo("Name", targetPerson.Name);

            if (targetPerson is Visitor visitor)
            {
                personInfo.AddInfo("Type", "Visitor")
                    .AddInfo("Nationality", visitor.Nationality)
                    .AddInfo("Passport Number", visitor.PassportNo);
            }

            if (targetPerson is Resident resident)
            {
                personInfo.AddInfo("Type", "Resident")
                    .AddInfo("Address", resident.Address)
                    .AddInfo("Last Left Country", resident.LastLeftCountry)
                    .Separator();

                if (resident.Token == null)
                {
                    personInfo.AddInfo("Trace Together Token", "None");
                }
                else
                {
                    personInfo.AddInfo("Trace Together Token", "")
                        .AddInfo("  Serial Number", resident.Token.SerialNo)
                        .AddInfo("  Collected Location", resident.Token.CollectionLocation)
                        .AddInfo("  Expiry Date", resident.Token.ExpiryDate);
                }
            }
            
            personInfo.Separator()
                .AddOrderedList("Safe Entry Records", targetPerson.SafeEntryList)
                .Separator()
                .AddOrderedList("Travel Entry Records", targetPerson.TravelEntryList);

            detailInfo.Text = personInfo.Build();
            nameInput.ClearText();
        }

        private void ExploreGlobalStats()
        {
            CHelper.WriteLine("ExploreGlobalStats");
        }
    }
}