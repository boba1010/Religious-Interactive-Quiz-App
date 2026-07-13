using Microsoft.JSInterop;

namespace ReligiousQuizCompetition.Admin.Web.Services
{
    public class LocalStorage(IJSRuntime js) : ILocalStorage
    {
        public async Task<string?> GetAsync(string key)
        {
            return await js.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task RemoveAsync(string key)
        {
            await js.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task SetAsync(string key, string value)
        {
            await js.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }
}
