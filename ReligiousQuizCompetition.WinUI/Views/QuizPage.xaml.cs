using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ReligiousQuizCompetitionWinUI.Views;

public sealed partial class QuizPage : Page
{
    public QuizViewModel ViewModel { get; set; }

    public QuizPage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetService<QuizViewModel>();
        Loading += OnLoading;
    }
    public async void OnLoading(FrameworkElement sender, object args)
    {
        await ViewModel.GetQuizAsync();
    }
}
