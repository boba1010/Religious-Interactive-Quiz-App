using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.Models;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Shared.Services
{
    public class AuthService(HttpClient httpClient, ITokenService tokenService, ApiClient apiClient)
    {
        private const string url = "https://religious-interactive-quiz.onrender.com/api/user";

        public async Task<ServiceResult<UserLoginResponse>> LoginAsync(RequestUserLogin request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}/auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return ServiceResult<UserLoginResponse>.Failure(msg);
                }

                var result = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                await tokenService.SetTokenAsync(result.Token);
                await tokenService.SetRefreshTokenAsync(result.RefreshToken);
                return ServiceResult<UserLoginResponse>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<UserLoginResponse>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<UserLoginResponse>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<UserLoginResponse>.Failure($"Error: An unexpected system error occurred.");
            }
        }

        public async Task<ServiceResult<UserSignupResponse>> SignupAsync(RequestUserSignup request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{url}/auth/signup", request);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return ServiceResult<UserSignupResponse>.Failure(msg);
                }

                var result = await response.Content.ReadFromJsonAsync<UserSignupResponse>();
                await tokenService.SetTokenAsync(result.Token);
                await tokenService.SetRefreshTokenAsync(result.RefreshToken);
                return ServiceResult<UserSignupResponse>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<UserSignupResponse>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceResult<UserSignupResponse>.Failure("Error: Network connection failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceResult<UserSignupResponse>.Failure($"Error: An unexpected system error occurred.");
            }
        }

        public async Task<ServiceResult<bool>> VerifyUserAsync()
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
