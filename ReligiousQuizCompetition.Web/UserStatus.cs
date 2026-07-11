namespace ReligiousQuizCompetition.Web
{
    public class UserStatus
    {
        private static bool _isLoggedIn;
        public static bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    // 2. Trigger the event when the value changes
                    OnStatusChanged?.Invoke();
                }
            }
        }
        public static string AccessToken { get; set; } = null!;

        public static event Action? OnStatusChanged;

        public static void Signout()
        {
            IsLoggedIn = false;
            AccessToken = null;
        }
    }
}
