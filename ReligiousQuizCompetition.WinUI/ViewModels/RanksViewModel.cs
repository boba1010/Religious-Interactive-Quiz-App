using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetitionWinUI.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class RanksViewModel(RanksService ranksService) : BaseViewModel
    {
        public ObservableCollection<UserRankDTO> Ranks { get; set; } = [];

        public async Task GetRanksAsync()
        {
            if (IsBusy)
                return;

            Ranks.Clear();
            IsBusy = true;

            var response = await ranksService.GetRanks();
            if (!response.IsSuccess)
            {
                await DialogService.ShowAlertAsync("Error", response.ErrorMessage);
                return;
            }

            var ranksDTO = response.Data;

            foreach (var rank in ranksDTO.Ranks)
                Ranks.Add(new() { FullName = rank.FullName, Score = rank.Score, Rank = rank.Rank });

            IsBusy = false;
        }
    }
}
