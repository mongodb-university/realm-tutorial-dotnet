﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Realms;
using Realms.Sync;
using Xamarin.Forms;

namespace realm_tutorial_dotnet
{
    public partial class TaskPage : ContentPage
    {
        private Realm taskRealm;
        private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();

        public ObservableCollection<Task> MyTasks
        {
            get
            {
                return _tasks;
            }
        }

        public TaskPage()
        {
            InitializeComponent();
            OnStart();
        }

        private async void OnStart()
        {
            try
            {
                activityIndicator.IsRunning = true;

                var syncConfig = new SyncConfiguration($"project={App.realmApp.CurrentUser.Id }", App.realmApp.CurrentUser);
                taskRealm = await Realm.GetInstanceAsync(syncConfig);
                SetUpTaskList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error Fetching Tasks", ex.Message, "OK");
                activityIndicator.IsRunning = false;
            }
        }

        private void SetUpTaskList()
        {
            _tasks = new ObservableCollection<Task>(taskRealm.All<Task>().ToList());
            listTasks.ItemsSource = MyTasks;
            activityIndicator.IsRunning = false;
        }

        async void TextCell_Tapped(object sender, EventArgs e)
        {
            var taskId = ((ViewCell)sender).ClassId;
            var task = taskRealm.All<Task>().Where(t => t.Id == taskId).FirstOrDefault();

            var editTaskPage = new EditTaskPage(taskRealm, task);
            editTaskPage.OperationCompeleted += EditTaskPage_OperationCompeleted;
            await Navigation.PushAsync(editTaskPage);
        }

        private void EditTaskPage_OperationCompeleted(object sender, EventArgs e)
        {
            (sender as EditTaskPage).OperationCompeleted -= EditTaskPage_OperationCompeleted;
            OnStart();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("New Task", "Enter the Task Name");

            if (result == null)
            {
                return;
            }

            if (taskRealm == null) { 
                var syncConfig = new SyncConfiguration($"project={App.realmApp.CurrentUser.Id }", App.realmApp.CurrentUser);
                taskRealm = await Realm.GetInstanceAsync(syncConfig);
            }

            var newTask = new Task()
            {
                Name = result,
                Status = Task.TaskStatus.Open.ToString()
            };

            taskRealm.Write(() =>
            {
                taskRealm.Add(newTask);
            });

            MyTasks.Add(newTask);
        }
    }
}