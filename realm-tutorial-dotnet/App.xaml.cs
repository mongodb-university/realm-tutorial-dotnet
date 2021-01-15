using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Realms.Sync;

namespace RealmDotnetTutorial
{
    public partial class App : Application
    {
        private const string appId = "tasktracker-iafbl";
        public static Realms.Sync.App realmApp;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            realmApp = Realms.Sync.App.Create(appId);
            if (App.realmApp.CurrentUser == null)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new ProjectPage());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
