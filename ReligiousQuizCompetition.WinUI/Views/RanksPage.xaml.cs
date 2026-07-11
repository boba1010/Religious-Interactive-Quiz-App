using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ReligiousQuizCompetitionWinUI.Views;

public sealed partial class RanksPage : Page
{
    private RanksViewModel ViewModel { get; set; }
    public RanksPage()
    {
        InitializeComponent();

        ViewModel = App.Services.GetService<RanksViewModel>();
        DataContext = ViewModel;

        Loading += OnLoading;
    }

    private async void OnLoading(FrameworkElement sender, object args)
    {
        await ViewModel.GetRanksAsync();
    }
}
