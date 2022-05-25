using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RealmDotnetTutorial.Models;
using Realms.Sync.Exceptions;
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
            var functionToCall = "getMyTeamMembers";
            try
            {
                teamMembers = await App.RealmApp.CurrentUser.Functions
                    .CallAsync<List<User>>(functionToCall);
                foreach (var member in teamMembers)
                {
                    _members.Add(member);
                }
                listMembers.ItemsSource = Members;
            }
            catch (AppException ex)
            {
                if (ex.Message.Contains("FunctionNotFound"))
                {
                    HandleFunctionError(functionToCall, ex);
                }
                else
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            var functionToCall = "removeTeamMember";
            var email = ((Button)sender).CommandParameter;
            try
            {
                var result = await App.RealmApp.CurrentUser.Functions.CallAsync(functionToCall, email.ToString());
                await DisplayAlert("Remove User", result.ToString(), "OK");
                listMembers.ItemsSource = Members;
            }
            catch (AppException ex)
            {
                if (ex.Message.Contains("FunctionNotFound"))
                {
                    HandleFunctionError(functionToCall, ex);
                }
                else
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            var functionToCall = "addTeamMember";
            string result = await DisplayPromptAsync("Add User to My Project", "User email:");
            if (result != null)
            {
                try
                {
                    var functionResult = await App.RealmApp.CurrentUser.Functions.CallAsync<FunctionResult>(functionToCall, result);
                }
                catch (AppException ex)
                {
                    if (ex.Message.Contains("FunctionNotFound"))
                    {
                        HandleFunctionError(functionToCall, ex);
                    }
                    else
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
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

        private async void HandleFunctionError(string functionToCall, AppException ex)
        {
            string message = "It looks like your backend is not set up correctly. " +
                                    $"Did the \"{functionToCall}\" function get " +
                                    $"created? See the 'Set up the Task Tracker " +
                                    $"Tutorial Backend' steps in the tutorial." +
                                    $"\r\n\r\n{ex.Message}";
            await DisplayAlert("Error", message, "OK");
            LogFunctionError();
        }

        void LogFunctionError()
        {
            Console.WriteLine("One or more functions is missing on the backend. " +
                "Check your set up. For more information , see" +
                "https://www.mongodb.com/docs/realm/tutorial/realm-app/#functions");
        }

    }

    class FunctionResult
    {
        public string Error { get; set; }
        public int MatchedCount { get; set; }
        public int ModifiedCount { get; set; }

    }
}