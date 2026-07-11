using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetitionWinUI.Services;

namespace ReligiousQuizCompetitionWinUI.Views
{
    public sealed partial class LoginPage : Page
    {
        private UserViewModel? ViewModel { get; set; }
        private NavigationService? NavService { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<UserViewModel>();
            DataContext = ViewModel;
            NavService = App.Services.GetService<NavigationService>();
        }

        private void OnCreateNewAccountClick(object sender, RoutedEventArgs e)
        {
            NavService?.NavigateTo<SignupPage>();
        }
    }
}
