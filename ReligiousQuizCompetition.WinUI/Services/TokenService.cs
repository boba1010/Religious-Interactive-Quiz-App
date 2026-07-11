using ReligiousQuizCompetition.Shared.Services;
using System.Threading.Tasks;

namespace ReligiousQuizCompetitionWinUI.Services
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
            return await secureStorage.GetAsync(TokenKey);
        }

        public async Task SetRefreshTokenAsync(string token)
        {
            await secureStorage.SetAsync(RefreshTokenKey, token);
        }

        public async Task SetTokenAsync(string token)
        {
            await secureStorage.SetAsync(TokenKey, token);
        }
    }
}
