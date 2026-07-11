using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ReligiousQuizCompetitionWinUI.Views;

public sealed partial class RanksPage : Page
{
    public RanksViewModel ViewModel { get; set; }
    public RanksPage()
    {
        InitializeComponent();

        ViewModel = App.Services.GetService<RanksViewModel>();
        
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.GetRanksAsync();
    }
}
