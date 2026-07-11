using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.ViewModels;

namespace ReligiousQuizCompetitionWinUI.Views;

public sealed partial class HomePage : Page
{
    private HomeViewModel ViewModel { get; set; }

    public HomePage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetService<HomeViewModel>();
        Loading += OnLoading;
        DataContext = ViewModel;
    }

    private async void OnLoading(FrameworkElement sender, object args)
    {
        await ViewModel.VerifyUser();
        ViewModel.LoadHighestScore();
    }

    private async void OnQuizButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (UserStateManager.IsLoggedIn)
        {
            Frame.Navigate(typeof(QuizPage));
            return;
        }

        await DialogService.ShowAlertAsync("Please login first", "");
    }

    private async void OnRanksButtonClicked(object? sender, RoutedEventArgs e)
    {
        if (UserStateManager.IsLoggedIn)
        {
            Frame.Navigate(typeof(RanksPage));
            return;
        }

        await DialogService.ShowAlertAsync("Please login first", "");
    }
}
