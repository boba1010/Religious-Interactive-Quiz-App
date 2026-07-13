using ReligiousQuizCompetition.Shared.Requests;
using ReligiousQuizCompetition.Shared.Responses;
using System.Net;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Shared.Services
{
    public class ApiClient(HttpClient httpClient, ITokenService tokenService)
    {
        //private readonly string url = "https://religious-interactive-quiz-api.onrender.com/api/user";
        private readonly string url = "https://localhost:7242/api/user";

        public async Task<RefreshTokenResponse?> RefreshSessionAsync(RequestRefreshToken request)
        {
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{url}/auth/refresh", request);

                if (!result.IsSuccessStatusCode)
                    return null;

                var response = await result.Content.ReadFromJsonAsync<RefreshTokenResponse>();
                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> SendAsync(Func<HttpClient, Task<HttpResponseMessage>> request)
        {
            var token = await tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

            var response = await request(httpClient);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            var refreshToken = await tokenService.GetRefreshTokenAsync();

            var refreshed = await RefreshSessionAsync(new() { Token = refreshToken });
            if (refreshed == null)
                return response;

            await tokenService.SetTokenAsync(refreshed.AccessToken);
            await tokenService.SetRefreshTokenAsync(refreshed.RefreshToken);

            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", refreshed.AccessToken);

            return await request(httpClient);
        }
    }
}
