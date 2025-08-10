using MauiPlusSqliteApp.ViewModels;

namespace MauiPlusSqliteApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
