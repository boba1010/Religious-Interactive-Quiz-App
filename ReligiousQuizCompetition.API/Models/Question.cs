namespace ReligiousQuizCompetition.API.Models
{
    public enum QuestionType
    {
        MultipleChoices,
        EssayQuestion
    }

    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuestionName { get; set; } = null!;
        public string[]? Choices { get; set; }
        public QuestionType QuestionType { get; set; }

        public int Score { get; set; }

        public string Answer { get; set; } = null!;
        public string? SelectedChoice { get; set; }
    }
}
