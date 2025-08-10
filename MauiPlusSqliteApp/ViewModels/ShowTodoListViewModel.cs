using CommunityToolkit.Mvvm.ComponentModel;
using MauiPlusSqliteApp.Models;
using MauiPlusSqliteApp.Services;
using System.Collections.ObjectModel;

namespace MauiPlusSqliteApp.ViewModels
{
    public partial class ShowTodoListViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Todo> todoList;

        public ShowTodoListViewModel()
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
    }
}
