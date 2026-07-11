namespace ReligiousQuizCompetition.Shared.Contracts.Requests
{
    public class SubmitQuizRequest
    {
        public int Id { get; set; }
        public List<string> Answers { get; set; } = null;
    }
}
