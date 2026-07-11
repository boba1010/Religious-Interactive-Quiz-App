using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Models;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.Views;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels;

public partial class QuizViewModel(QuizService quizService, NavigationService navService) : BaseViewModel
{
    [ObservableProperty]
    public partial Quiz Quiz { get; set; }

    public async Task GetQuizAsync()
    {
        if (IsBusy == true)
            return;

        IsBusy = true;

        Quiz = null!;
        Quiz result = await quizService.GetQuizAsync();

        if (result == null)
        {
            BusyState = Visibility.Collapsed;
            IsBusy = false;
            navService.GoBack();
            return;
        }

        Quiz = result;
        BusyState = Visibility.Collapsed;
        IsBusy = false;
    }

    [RelayCommand]
    public async Task SubmitQuizAsync()
    {
        if (IsBusy)
            return;

        var request = new SubmitQuizRequest() { Id = Quiz.Id, Answers = [], SelectedChoices = [] };

        foreach (Question question in Quiz.Questions)
        {
            if (request.Answers == null)
                continue;
            request.Answers.Add(question.Answer);
            if (request.SelectedChoices == null || question.SelectedChoice == null)
                continue;
            request.SelectedChoices.Add(question.SelectedChoice);
        }

        IsBusy = true;
        var response = await quizService.SubmitQuizAsync(request);

        if (response == null)
        {
            await DialogService.ShowAlertAsync("Something wrong has happened while sending your answers please try again.", "");
            return;
        }

        int highestScore = Preferences.Get("HighestScore", 0);

        if (response.CorrectAnswersCount > highestScore)
            Preferences.Set("HighestScore", response.CorrectAnswersCount);
        IsBusy = false;
        navService.GoBack();
    }
}
