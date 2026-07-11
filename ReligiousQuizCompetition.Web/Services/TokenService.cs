using ReligiousQuizCompetition.Shared.Services;

namespace ReligiousQuizCompetition.Web.Services
{
    public class TokenService(ISecureStorage secureStorage) : ITokenService
    {
        private const string TokenKey = "Token";
        private const string RefreshTokenKey = "RefreshToken";

        public async Task Clear()
        {
            secureStorage.Remove(TokenKey);
            secureStorage.Remove(RefreshTokenKey);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await secureStorage.GetAsync(RefreshTokenKey);
        }

        public async Task<string?> GetTokenAsync()
        {
            return UserStatus.AccessToken;
        }

        public async Task SetRefreshTokenAsync(string token)
        {
            await secureStorage.SetAsync(RefreshTokenKey, token);
        }

        public async Task SetTokenAsync(string token)
        {
            UserStatus.AccessToken = token;
        }
    }
}
