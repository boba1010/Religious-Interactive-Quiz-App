using ReligiousQuizCompetition.Shared.DTOs;

namespace ReligiousQuizCompetition.Shared.Contracts.Responses
{
    public class UserSignupResponse
    {
        public UserDTO User { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
