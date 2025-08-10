using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiPlusSqliteApp.Models;
using MauiPlusSqliteApp.Services;
using System.Collections.ObjectModel;

namespace MauiPlusSqliteApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Todo> todoList;

        [ObservableProperty]
        string newTitle;

        public MainViewModel()
        {
            TodoList = new ObservableCollection<Todo>();
            LoadTodoList();
        }

        async void LoadTodoList()
        {
            try
            {
                var todos = OperationsTodo.Read();
                TodoList.Clear();
                foreach (var todo in todos)
                {
                    TodoList.Add(todo);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void AddTodo()
        {
            if (string.IsNullOrWhiteSpace(NewTitle))
            {
                await Shell.Current.DisplayAlert("Error!", "Title cannot be empty!", "OK");
                return;
            }
            try
            {
                var todo = new Todo { Title = NewTitle };
                OperationsTodo.Create(todo);
                LoadTodoList();
                NewTitle = string.Empty;
                await Shell.Current.DisplayAlert("Success!", "Todo added.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void EditTodo(Todo todoEdit)
        {
            string newTitle = await Shell.Current.DisplayPromptAsync("Edit title...",
                    "Enter new title", initialValue: todoEdit.Title);
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                await Shell.Current.DisplayAlert("Error!", "Title cannot be empty!", "OK");
                return;
            }
            try
            {
                todoEdit.Title = newTitle;
                OperationsTodo.Update(todoEdit);
                LoadTodoList();
                await Shell.Current.DisplayAlert("Success!", "Todo updated.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void DeleteTodo(Todo todoDelete)
        {
            bool confirmed = await Shell.Current.DisplayAlert("Confirm Delete...",
                    $"Are you sure want to delete {todoDelete.Title}",
                    "Yes", "No");
            if (!confirmed)
            {
                return;
            }
            try
            {
                OperationsTodo.Delete(todoDelete);
                LoadTodoList();
                await Shell.Current.DisplayAlert("Success!", "Todo deleted.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}
