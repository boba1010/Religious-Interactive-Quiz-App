using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Models;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Shared.Services;

public class QuizService(ApiClient apiClient)
{
    private const string url = "https://localhost:7242/api/quiz";

    public async Task<ServiceResult<QuizDTO>> GetQuizAsync()
    {
        try
        {
            var response = await apiClient.SendAsync(client => client.GetAsync(url));

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ServiceResult<QuizDTO>
                    .Failure("UnAuthorized");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return ServiceResult<QuizDTO>.Failure("Error: Something wrong has happened while receiving the data");
            }

            var result = await response.Content.ReadFromJsonAsync<QuizDTO>();
            return ServiceResult<QuizDTO>.Success(result);
        }
        catch (TaskCanceledException)
        {
            return ServiceResult<QuizDTO>.Failure("Error: Connection Timeout.");
        }
        catch (HttpRequestException)
        {
            return ServiceResult<QuizDTO>.Failure("Error: Network connection failed.");
        }
        catch (Exception)
        {
            return ServiceResult<QuizDTO>.Failure($"Error: An unexpected system error occurred.");
        }
    }

    public async Task<ServiceResult<SubmitQuizResponse>> SubmitQuizAsync(SubmitQuizRequest request)
    {
        try
        {
            var response = await apiClient.SendAsync(client => client.PostAsJsonAsync($"{url}/submit", request));

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ServiceResult<SubmitQuizResponse>
                    .Failure("UnAuthorized");
            }
            else if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                return ServiceResult<SubmitQuizResponse>.Failure(msg);
            }

            var result = await response.Content.ReadFromJsonAsync<SubmitQuizResponse>();

            return ServiceResult<SubmitQuizResponse>.Success(result);
        }
        catch (TaskCanceledException)
        {
            return ServiceResult<SubmitQuizResponse>.Failure("Error: Connection Timeout.");
        }
        catch (HttpRequestException)
        {
            return ServiceResult<SubmitQuizResponse>.Failure("Error: Network connection failed.");
        }
        catch (Exception)
        {
            return ServiceResult<SubmitQuizResponse>.Failure($"Error: An unexpected system error occurred.");
        }
    }
}
