namespace ReligiousQuizCompetition.Shared.Services
{
    public interface ISecureStorage
    {
        public Task<string?> GetAsync(string key);
        public Task SetAsync(string key, string value);
        public void Remove(string key);
        public void Clear();
    }
}
