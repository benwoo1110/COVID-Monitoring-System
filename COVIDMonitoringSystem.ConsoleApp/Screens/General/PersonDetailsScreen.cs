using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.General
{
    public class PersonDetailsScreen : CovidScreen

    {
        public override string Name => "viewPersonDetails";

        private Header header = new Header("header")
        {
            Text = "Details of a Person",
            BoundingBox = {Top = 0}
        };
        private Input name = new Input("name")
        {
            Prompt = "Person Name",
            BoundingBox = {Top = 4}
        };
        private Button view = new Button("view")
        {
            Text = "[View Details]",
            BoundingBox = {Top = 6}
        };
        private Label details = new Label("details")
        {
            BoundingBox = {Top = 8}
        };

        public PersonDetailsScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("view")]
        private void OnViewDetails([Parser("name", "details")] Person targetPerson)
        {
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

            details.Text = personInfo.Build();
            ClearAllInputs();
        }
    }
}