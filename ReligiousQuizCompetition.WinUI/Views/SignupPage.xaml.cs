using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetitionWinUI.ViewModels;

namespace ReligiousQuizCompetitionWinUI.Views
{
    public sealed partial class SignupPage : Page
    {
        private UserViewModel? ViewModel { get; set; }
        public SignupPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetService<UserViewModel>();
            DataContext = ViewModel;
        }
    }
}
