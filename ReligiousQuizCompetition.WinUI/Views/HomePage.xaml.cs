using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.ViewModels;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.Views;

public sealed partial class HomePage : Page
{
    private UserViewModel ViewModel { get; set; }

    public HomePage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetService<UserViewModel>();
        Loading += OnLoading;
        DataContext = ViewModel;
    }

    private async void OnLoading(FrameworkElement sender, object args)
    {
        await ViewModel.VerifyUser();
        await ViewModel.LoadUserData();
    }

    private async void OnQuizButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.IsLoggedIn)
        {
            Frame.Navigate(typeof(QuizPage));
            return;
        }

        await DialogService.ShowAlertAsync("Please login first", "");
    }

    private async void OnRanksButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.IsLoggedIn)
        {
            Frame.Navigate(typeof(RanksPage));
            return;
        }

        await DialogService.ShowAlertAsync("Please login first", "");
    }
}
