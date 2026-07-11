namespace ReligiousQuizCompetition.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte[] Salt { get; set; }

        public int Score { get; set; }
    }
}
