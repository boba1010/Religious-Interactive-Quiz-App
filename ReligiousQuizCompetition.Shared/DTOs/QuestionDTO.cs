using ReligiousQuizCompetition.Shared.Models;

namespace ReligiousQuizCompetition.Shared.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string QuestionName { get; set; } = null!;
        public string[]? Choices { get; set; } = null;
        public QuestionType QuestionType { get; set; }

        public string Answer { get; set; } = null!;
        public string SelectedChoice { get; set; } = null!;
    }
}
