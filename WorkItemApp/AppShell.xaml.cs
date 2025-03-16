namespace WorkItemApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("addworkitem", typeof(AddWorkItemPage));
            Routing.RegisterRoute("editworkitem", typeof(EditWorkItemPage));
        }
    }
}
