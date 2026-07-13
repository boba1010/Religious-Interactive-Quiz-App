using ReligiousQuizCompetition.Admin.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Admin.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Services;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Admin.Web.Services
{
    public class AuthService(ITokenService tokenService, HttpClient httpClient, ApiClient apiClient)
    {
        private const string url = "https://religious-interactive-quiz-api.onrender.com/api/admin";
        //private const string url = "https://localhost:7242/api/admin";

        public async Task<ServiceResult<AdminLoginResponse>> LoginAsync(RequestAdminLogin request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}/auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return ServiceResult<AdminLoginResponse>.Failure(msg);
                }

                var result = await response.Content.ReadFromJsonAsync<AdminLoginResponse>();
                await tokenService.SetTokenAsync(result.Token);
                await tokenService.SetRefreshTokenAsync(result.RefreshToken);
                return ServiceResult<AdminLoginResponse>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<AdminLoginResponse>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<AdminLoginResponse>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<AdminLoginResponse>.Failure($"Error: An unexpected system error occurred.");
            }
        }

        public async Task<ServiceResult<bool>> VerifyAdminUserAsync()
        {
            try
            {
                var result = await apiClient.SendAsync(client => client.GetAsync($"{url}/auth/verify"));
                Console.WriteLine(result.StatusCode);
                return ServiceResult<bool>.Success(result.IsSuccessStatusCode);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<bool>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<bool>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<bool>.Failure($"Error: An unexpected system error occurred.");
            }
        }
    }
}
