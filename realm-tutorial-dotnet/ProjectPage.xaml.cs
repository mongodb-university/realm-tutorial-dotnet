using System;
using System.Collections.ObjectModel;
using System.Linq;
using Realms;
using Realms.Sync;
using Xamarin.Forms;

namespace realm_tutorial_dotnet
{
    public partial class ProjectPage : ContentPage
    {
        private User user;
        private Realm userRealm;
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();

        public ObservableCollection<Project> MyProjects
        {
            get
            {
                return _projects;
            }
        }

        public ProjectPage()
        {
            if (App.realmApp.CurrentUser != null) App.realmApp.CurrentUser.LogOutAsync();

            InitializeComponent();
            OnStart();
        }

        private async void OnStart()
        {
            if (App.realmApp.CurrentUser == null)
            {
                var loginPage = new LoginPage();
                loginPage.OperationCompeleted += LoginPage_OperationCompeleted;
                await Navigation.PushAsync(loginPage);
            }
            else
            {
                try
                {
                    var syncConfig = new SyncConfiguration($"user={ App.realmApp.CurrentUser.Id }", App.realmApp.CurrentUser);
                    userRealm = await Realm.GetInstanceAsync(syncConfig);
                    user = userRealm.All<User>().ToList().Where(u => u.Id == App.realmApp.CurrentUser.Id).FirstOrDefault();

                    if (user != null) SetUpProjectList();
                }
                catch (Exception ex)
                {
                    //TODO:
                }
            }
        }

        private void LoginPage_OperationCompeleted(object sender, EventArgs e)
        {
            (sender as LoginPage).OperationCompeleted -= LoginPage_OperationCompeleted;
            OnStart();
        }

        private void SetUpProjectList()
        {
            listProjects.ItemsSource = MyProjects;
            foreach (Project p in user.MemberOf)
            {
                MyProjects.Add(p);
            }
            if (MyProjects.Count <= 0)
            {
                MyProjects.Add(new Project("No projects found!"));
            }
        }

        void TextCell_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new TaskPage());
        }
    }
}