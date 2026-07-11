using CommunityToolkit.Mvvm.ComponentModel;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        [ObservableProperty]
        public partial bool IsNotBusy { get; set; } = true;

        partial void OnIsBusyChanged(bool value)
        {
            IsNotBusy = !value;
        }
    }
}
