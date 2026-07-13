namespace ReligiousQuizCompetition.Admin.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public int Score { get; set; }
        public List<int> AnsweredQuizzes { get; set; } = [];
    }
}
