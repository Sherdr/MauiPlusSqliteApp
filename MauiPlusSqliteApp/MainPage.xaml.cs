using MauiPlusSqliteApp.Data;
using MauiPlusSqliteApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiPlusSqliteApp
{
    public partial class MainPage : ContentPage
    {
        readonly TodoRepository dbContext;

        public ObservableCollection<Todo> TodoList { get; set; }

        public MainPage(TodoRepository dbContext)
        {
            this.dbContext = dbContext;
            InitializeComponent();
            TodoList = new ObservableCollection<Todo>();
            BindingContext = this;
            LoadTodoList();
        }

        async void LoadTodoList()
        {
            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                var todos = await dbContext.Todolist.ToListAsync();
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
                TodoList.Add(todo);
                TitleEntry.Text = string.Empty;
                dbContext.Todolist.Add(todo);
                await dbContext.SaveChangesAsync();
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
                    dbContext.Todolist.Remove(todoDelete);
                    TodoList.Remove(todoDelete);
                    await dbContext.SaveChangesAsync();
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
                    dbContext.Todolist.Update(todoEdit);
                    await dbContext.SaveChangesAsync();
                    var index = TodoList.IndexOf(todoEdit);
                    if(index != -1)
                    {
                        TodoList[index] = todoEdit;
                    }
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
