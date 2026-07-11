using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetitionWinUI.ViewModels;

namespace ReligiousQuizCompetitionWinUI.Views
{
    public sealed partial class SignupPage : Page
    {
        private SignupViewModel? ViewModel { get; set; }
        public SignupPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<SignupViewModel>();
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
