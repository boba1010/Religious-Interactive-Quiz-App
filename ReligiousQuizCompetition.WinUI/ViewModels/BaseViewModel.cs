using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace ReligiousQuizCompetitionWinUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        [ObservableProperty]
        public partial bool IsNotBusy { get; set; } = true;

        [ObservableProperty]
        public partial Visibility BusyState { get; set; } = Visibility.Collapsed;

        [ObservableProperty]
        public partial Visibility NotBusyState { get; set; }

        partial void OnIsBusyChanged(bool value)
        {
            IsNotBusy = !value;
            BusyState = value ? Visibility.Visible : Visibility.Collapsed;
            NotBusyState = value ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
