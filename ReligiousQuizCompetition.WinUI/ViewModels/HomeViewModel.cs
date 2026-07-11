using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetitionWinUI.Services;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class HomeViewModel(AuthService authService, ISecureStorage secureStorage) : BaseViewModel
    {
        [ObservableProperty]
        public partial uint HighestScore { get; set; }

        public void LoadHighestScore()
        {
            HighestScore = Preferences<uint>.Get("HighestScore");
        }

        [RelayCommand]
        public void Logout()
        {
            UserStateManager.Logout();
            secureStorage.Clear();
        }

        public async Task VerifyUser()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var response = await authService.VerifyUserAsync();
            UserStateManager.IsLoggedIn = response.Data;
            IsBusy = false;
        }
    }
}
