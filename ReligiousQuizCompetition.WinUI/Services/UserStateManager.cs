namespace ReligiousQuizCompetitionWinUI.Services
{
    public class UserStateManager
    {
        public static bool IsLoggedIn { get; set; }

        public static void Logout()
        {
            IsLoggedIn = false;
            Preferences<object>.Clear();
        }
    }
}
