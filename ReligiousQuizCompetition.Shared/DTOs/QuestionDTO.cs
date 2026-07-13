namespace ReligiousQuizCompetition.Shared.DTOs
{
    public enum QuestionType
    {
        MultipleChoices,
        EssayQuestion
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string QuestionName { get; set; } = null!;
        public string[]? Choices { get; set; } = null;
        public QuestionType QuestionType { get; set; }

        public string Answer { get; set; } = null!;
        public int Score { get; set; }
    }
}
