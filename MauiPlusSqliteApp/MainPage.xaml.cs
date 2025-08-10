using MauiPlusSqliteApp.Data;
using MauiPlusSqliteApp.Models;
using MauiPlusSqliteApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MauiPlusSqliteApp
{
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<Todo> TodoList { get; set; }

        public MainPage(TodoRepository dbContext)
        {
            InitializeComponent();
            TodoList = new ObservableCollection<Todo>();
            BindingContext = this;
            LoadTodoList();
        }

        async void LoadTodoList()
        {
            try
            {
                var todos = OperationsTodo.Read();
                TodoList.Clear();
                foreach(var todo in todos)
                {
                    TodoList.Add(todo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        async void OnAddTodoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Error!", "Title cannot be empty!", "OK");
                return;
            }
            try
            {
                var todo = new Todo { Title = TitleEntry.Text };
                OperationsTodo.Create(todo);
                LoadTodoList();
                TitleEntry.Text = string.Empty;
                await DisplayAlert("Success!", "Todo added.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        async void OnDeleteTodoClicked(object sender, EventArgs e)
        {
            if(sender is Button btn && btn.CommandParameter is Todo todoDelete)
            {
                bool confirmed = await DisplayAlert("Confirm Delete...",
                    $"Are you sure want to delete {todoDelete.Title}",
                    "Yes", "No");
                if(!confirmed)
                {
                    return;
                }
                try
                {
                    OperationsTodo.Delete(todoDelete);
                    LoadTodoList();
                    await DisplayAlert("Success!", "Todo deleted.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", ex.Message, "OK");
                }
            }
        }

        async void OnEditTodoClicked(object sender, EventArgs e)
        {
            if(sender is Button btn && btn.CommandParameter is Todo todoEdit)
            {
                string newTitle = await DisplayPromptAsync("Edit title...",
                    "Enter new title", initialValue: todoEdit.Title);
                if (string.IsNullOrWhiteSpace(newTitle))
                {
                    await DisplayAlert("Error!", "Title cannot be empty!", "OK");
                    return;
                }
                try
                {
                    todoEdit.Title = newTitle;
                    OperationsTodo.Update(todoEdit);
                    LoadTodoList();
                    await DisplayAlert("Success!", "Todo updated.", "OK");
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Error!", ex.Message, "OK");
                }
            }
        }
    }
}
