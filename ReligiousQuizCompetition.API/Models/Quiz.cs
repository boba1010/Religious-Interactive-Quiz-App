namespace ReligiousQuizCompetition.API.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
