namespace ReligiousQuizCompetition.Web.Services
{
    public interface ILocalStorage
    {
        public Task<string?> GetAsync(string key);
        public Task SetAsync(string key, string value);
        public Task RemoveAsync(string key);
    }
}
