using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class NewVisitorScreen : Screen
    {
        private Label info = new Label("info")
        {
            Text = "Please Enter Details of the Visitor",
            BoundingBox = {Top = 4}
        };

        private Input name = new Input("name")
        {
            Prompt = "Name",
            BoundingBox = {Top = 6}
        };

        private Input passportNo = new Input("passportNo")
        {
            Prompt = "Passport Number",
            BoundingBox = {Top = 7}
        };

        private Input nationality = new Input("Nationality")
        {
            Prompt = "Nationality",
            BoundingBox = {Top = 8}
        };

        private Button create = new Button("create")
        {
            Text = "[Create]",
            BoundingBox = {Top = 10},
        };

        private Label result = new Label("result")
        {
            BoundingBox = {Top = 12}
        };
        
        public NewVisitorScreen(ConsoleManager manager) : base(manager)
        {
            Name = "newVisitor";
            create.Runner = CreateNewVisitor;

            AddElement(new Header("header", "New Visitor"));
            AddElement(info);
            AddElement(name);
            AddElement(passportNo);
            AddElement(nationality);
            AddElement(create);
            AddElement(result);
        }

        private void CreateNewVisitor()
        {
            if (!name.HasText() || !passportNo.HasText() || !nationality.HasText())
            {
                result.Text = "Incomplete details. No visitor has been added to the system.";
                return;
            }

            // Manager.AddPerson(new Visitor(name.Text, passportNo.Text, nationality.Text));
            result.Text = $"New visitor {name.Text} has been added to the system.";

            name.ClearText();
            passportNo.ClearText();
            nationality.ClearText();
        }
    }
}