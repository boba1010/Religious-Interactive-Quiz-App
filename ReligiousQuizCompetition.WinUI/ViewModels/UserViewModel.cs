using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.Views;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class UserViewModel(NavigationService navService, UserService userService) : BaseViewModel
    {
        [ObservableProperty]
        public partial int HighestScore { get; set; }

        [ObservableProperty]
        public partial UserDTO User { get; set; }

        [ObservableProperty]
        public partial bool IsLoggedIn { get; set; }

        public async Task LoadUserData()
        {
            HighestScore = Preferences.Get("HighestScore", 0);
        }

        [RelayCommand]
        public void Logout()
        {
            IsLoggedIn = false;
            SecureStorage.Clear();
            Preferences.Clear();
            User = null;
            HighestScore = 0;
        }

        public RequestUserSignup RequestUserSignup { get; set; } = new();
        [RelayCommand]
        public async Task SignupAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var response = await userService.SignupAsync(RequestUserSignup);
            if (response == null)
            {
                IsBusy = false;
                await DialogService.ShowAlertAsync("Something wrong has happened please try again", "");
                return;
            }

            await SecureStorage.SetAsync("AccessToken", response.Token);
            await SecureStorage.SetAsync("RefreshToken", response.RefreshToken);
            User = response.User;
            IsLoggedIn = true;

            navService.NavigateTo<HomePage>();
            IsBusy = false;
        }

        public RequestUserLogin RequestUserLogin { get; set; } = new();
        [RelayCommand]
        public async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var response = await userService.LoginAsync(RequestUserLogin);
            if (response == null)
            {
                IsBusy = false;
                await DialogService.ShowAlertAsync("Something wrong has happened please try again", "");
                return;
            }

            await SecureStorage.SetAsync("AccessToken", response.Token);
            await SecureStorage.SetAsync("RefreshToken", response.RefreshToken);
            User = response.User;
            IsLoggedIn = true;

            navService.NavigateTo<HomePage>();
            IsBusy = false;
        }

        public async Task VerifyUser()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsLoggedIn = await userService.VerifyUser();
            IsBusy = false;
        }
    }
}
