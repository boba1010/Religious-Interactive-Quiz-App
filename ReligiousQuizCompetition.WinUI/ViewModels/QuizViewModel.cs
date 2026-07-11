using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetitionWinUI.Services;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels;

public partial class QuizViewModel(QuizService quizService, NavigationService navService) : BaseViewModel
{
    [ObservableProperty]
    public partial QuizDTO Quiz { get; set; }

    public async Task GetQuizAsync()
    {
        if (IsBusy == true)
            return;

        IsBusy = true;

        Quiz = null!;
        var response = await quizService.GetQuizAsync();

        if (!response.IsSuccess)
        {
            IsBusy = false;
            navService.GoBack();
            return;
        }

        Quiz = response.Data;
        IsBusy = false;
    }

    [RelayCommand]
    public async Task SubmitQuizAsync()
    {
        if (IsBusy)
            return;

        var request = new SubmitQuizRequest() { Id = Quiz.Id, Answers = [] };

        foreach (QuestionDTO question in Quiz.Questions)
        {
            if (request.Answers == null)
                continue;
            request.Answers.Add(question.Answer);
        }

        IsBusy = true;
        var response = await quizService.SubmitQuizAsync(request);

        if (!response.IsSuccess)
        {
            await DialogService.ShowAlertAsync("Something wrong has happened while sending your answers please try again.", "");
            return;
        }

        var result = response.Data;

        uint highestScore = Preferences<uint>.Get("HighestScore", 0);

        if (result.Score > highestScore)
            Preferences<uint>.Set("HighestScore", (uint)result.Score);
        IsBusy = false;
        navService.GoBack();
    }
}
