using ReligiousQuizCompetition.Shared.DTOs;

namespace ReligiousQuizCompetitionWinUI.Templates
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MultipleQuestionTemplate { get; set; } = new();
        public DataTemplate WrittenTemplate { get; set; } = new();

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var question = (QuestionDTO)item;

            return question.QuestionType == QuestionType.MultipleChoices 
                ? MultipleQuestionTemplate : WrittenTemplate;
        }
    }
}
