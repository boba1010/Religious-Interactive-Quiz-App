using ReligiousQuizCompetitionWinUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ReligiousQuizCompetitionWinUI.Services;

namespace ReligiousQuizCompetitionWinUI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class QuizPage : Page
{
    private QuizViewModel ViewModel { get; set; }

    public QuizPage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetService<QuizViewModel>();
        Loading += OnLoading;
        DataContext = ViewModel;
    }
    public async void OnLoading(FrameworkElement sender, object args)
    {
        await ViewModel.GetQuizAsync();
    }
}
