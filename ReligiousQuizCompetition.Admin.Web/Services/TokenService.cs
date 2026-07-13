using ReligiousQuizCompetition.Shared.Services;

namespace ReligiousQuizCompetition.Admin.Web.Services
{
    public class TokenService(ISecureStorage secureStorage) : ITokenService
    {
        private const string RefreshTokenKey = "RefreshToken";

        public async Task Clear()
        {
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await secureStorage.GetAsync(RefreshTokenKey);
        }

        public async Task<string?> GetTokenAsync()
        {
            return AdminStatus.AccessToken;
        }

        public async Task SetRefreshTokenAsync(string token)
        {
            await secureStorage.SetAsync(RefreshTokenKey, token);
        }

        public async Task SetTokenAsync(string token)
        {
            AdminStatus.AccessToken = token;
        }
    }
}
