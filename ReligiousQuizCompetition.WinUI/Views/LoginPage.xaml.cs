using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ReligiousQuizCompetitionWinUI.Views
{
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel? ViewModel { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<LoginViewModel>();
            DataContext = ViewModel;
        }

        private void OnCreateNewAccountClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignupPage));
        }
    }
}
