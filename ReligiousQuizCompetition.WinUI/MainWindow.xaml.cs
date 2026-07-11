using Microsoft.UI.Windowing;
using ReligiousQuizCompetitionWinUI.ViewModels;
using ReligiousQuizCompetitionWinUI.Views;

namespace ReligiousQuizCompetitionWinUI
{
    public sealed partial class MainWindow : Window
    {
        public UserViewModel ViewModel { get; set; }
        public MainWindow(UserViewModel vm)
        {
            InitializeComponent();

            ViewModel = vm;

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(quizAppTitlebar);
            var tb = AppWindow.TitleBar;
            tb.PreferredHeightOption = TitleBarHeightOption.Tall;

            App.RootFrame = rootFrame;

            rootFrame.Navigated += OnNavigated;

            rootFrame.Navigate(typeof(HomePage));
        }

        private async void OnNavigated(object? sender, NavigationEventArgs e)
        {
            quizAppTitlebar.IsBackButtonVisible = rootFrame.CanGoBack;
            quizAppTitlebar.IsBackButtonEnabled = rootFrame.CanGoBack;
        }

        private async void OnBackRequested(TitleBar sender, object args)
        {
            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private async void OnLoginButtonClicked(object? sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(LoginPage));
        }

        public Visibility HideIfLoggedIn(bool isLoggedIn)
        {
            return isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
        }
        public Visibility ShowIfLoggedIn(bool isLoggedIn)
        {
            return isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
