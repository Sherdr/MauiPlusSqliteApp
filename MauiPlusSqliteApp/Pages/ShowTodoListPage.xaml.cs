using MauiPlusSqliteApp.ViewModels;

namespace MauiPlusSqliteApp.Pages;

public partial class ShowTodoListPage : ContentPage
{
	public ShowTodoListPage()
	{
		BindingContext = new ShowTodoListViewModel();
		InitializeComponent();
	}
}