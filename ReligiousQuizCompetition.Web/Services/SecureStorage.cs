using ReligiousQuizCompetition.Shared.Services;

namespace ReligiousQuizCompetition.Web.Services
{
    public class SecureStorage(ILocalStorage localStorage) : ISecureStorage
    {
        public void Clear()
        {
        }

        public async Task<string?> GetAsync(string key)
        {
            return await localStorage.GetAsync(key);
        }

        public void Remove(string key)
        {
            localStorage.RemoveAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await localStorage.SetAsync(key, value);
        }
    }
}
