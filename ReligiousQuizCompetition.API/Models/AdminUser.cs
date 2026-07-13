namespace ReligiousQuizCompetition.API.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
    }
}
