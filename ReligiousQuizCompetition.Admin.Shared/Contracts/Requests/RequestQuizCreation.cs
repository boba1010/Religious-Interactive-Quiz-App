using ReligiousQuizCompetition.Admin.Shared.Models;

namespace ReligiousQuizCompetition.Admin.Shared.Contracts.Requests
{
    public class RequestQuizCreation
    {
        public List<Question> Questions { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
