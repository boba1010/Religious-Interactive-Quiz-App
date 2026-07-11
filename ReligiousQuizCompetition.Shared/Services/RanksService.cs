using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Models;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Shared.Services
{
    public class RanksService(ApiClient apiClient)
    {
        private const string Uri = "https://religious-interactive-quiz-api.onrender.com/api/ranks";

        public async Task<ServiceResult<UsersRankDTO>> GetRanks()
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.GetAsync(Uri));

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ServiceResult<UsersRankDTO>
                        .Failure("UnAuthorized");
                }
                else if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<UsersRankDTO>.Failure("Error: Something wrong has happened while receiving the data");
                }

                var result = await response.Content.ReadFromJsonAsync<UsersRankDTO>();
                return ServiceResult<UsersRankDTO>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<UsersRankDTO>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<UsersRankDTO>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<UsersRankDTO>.Failure($"Error: An unexpected system error occurred.");
            }
        }
    }
}
