namespace ReligiousQuizCompetition.Shared.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }
        public List<QuestionDTO> Questions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
