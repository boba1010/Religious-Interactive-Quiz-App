using CommunityToolkit.Mvvm.Input;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.Views;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class LoginViewModel(AuthService authService, ISecureStorage secureStorage, NavigationService navService) : BaseViewModel
    {
        public RequestUserLogin RequestUserLogin { get; set; } = new();
        [RelayCommand]
        public async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var response = await authService.LoginAsync(RequestUserLogin);
            if (!response.IsSuccess)
            {
                IsBusy = false;
                await DialogService.ShowAlertAsync("Something wrong has happened please try again", "");
                return;
            }

            var result = response.Data;

            await secureStorage.SetAsync("AccessToken", result.Token);
            await secureStorage.SetAsync("RefreshToken", result.RefreshToken);
            UserStateManager.IsLoggedIn = true;

            navService.NavigateTo<HomePage>();
            IsBusy = false;
        }
    }
}
