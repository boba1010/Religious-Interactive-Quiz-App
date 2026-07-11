namespace ReligiousQuizCompetition.Shared.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
