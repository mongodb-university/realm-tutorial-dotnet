using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RealmDotnetTutorial.Models;
using Xamarin.Forms;

namespace RealmDotnetTutorial
{
    public partial class AddMemberPage : ContentPage
    {
        private List<User> teamMembers;
        private ObservableCollection<User> _members = new ObservableCollection<User>();

        public ObservableCollection<User> Members
        {
            get
            {
                return _members;
            }
        }

        public event EventHandler<EventArgs> OperationCompeleted = delegate { };

        public AddMemberPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            try
            {
                // TODO: Call the "getMyTeamMembers" to get all team members
                // teamMembers = await ...
                foreach (var member in teamMembers)
                {
                    _members.Add(member);
                }
                listMembers.ItemsSource = Members;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            var email = ((Button)sender).CommandParameter;
            try
            {
                // TODO: Pass email.ToString() to the "removeTeamMember"
                // function.
                // var result = await ...
                await DisplayAlert("Remove User", result.ToString(), "OK");
                listMembers.ItemsSource = Members;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Add User to My Project", "User email:");
            if (result != null)
            {
                try
                {
                    // TODO: Pass the result object to the "addTeamMember" 
                    // function.
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                    return;
                }
            }
            Complete();
        }

        async void Complete()
        {
            OperationCompeleted(this, EventArgs.Empty);
            await Navigation.PopAsync();
        }
    }

    class FunctionResult
    {
        public string Error { get; set; }
        public int MatchedCount { get; set; }
        public int ModifiedCount { get; set; }

    }
}
