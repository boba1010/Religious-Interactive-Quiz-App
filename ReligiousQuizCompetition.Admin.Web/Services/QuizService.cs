using ReligiousQuizCompetition.Admin.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Admin.Shared.Models;
using ReligiousQuizCompetition.Shared.DTOs;
using System.Net.Http.Json;

namespace ReligiousQuizCompetition.Admin.Web.Services
{
    public class QuizService(ApiClient apiClient)
    {
        //private readonly string url = "https://religious-interactive-quiz-api.onrender.com/api/admin/quizzes";
        private readonly string url = "https://localhost:7242/api/admin/quizzes";

        public async Task<ServiceResult<bool>> UpdateQuiz(RequestQuizUpdate request)
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.PostAsJsonAsync($"{url}/update", request));

                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return ServiceResult<bool>.Failure("UnAuthorized");
                else if (!response.IsSuccessStatusCode)
                    return ServiceResult<bool>.Failure("Error: Something wrong has happened while receiving the data");

                return ServiceResult<bool>.Success(response.IsSuccessStatusCode);
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

        public async Task<ServiceResult<bool>> DeleteQuiz(int id)
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.PostAsJsonAsync($"{url}/delete", id));

                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return ServiceResult<bool>.Failure("UnAuthorized");
                else if (!response.IsSuccessStatusCode)
                    return ServiceResult<bool>.Failure("Error: Something wrong has happened while receiving the data");

                return ServiceResult<bool>.Success(response.IsSuccessStatusCode);
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

        public async Task<ServiceResult<bool>> CreateQuiz(RequestQuizCreation request)
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.PostAsJsonAsync($"{url}/create", request));

                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return ServiceResult<bool>.Failure("UnAuthorized");
                else if (!response.IsSuccessStatusCode)
                    return ServiceResult<bool>.Failure("Error: Something wrong has happened while receiving the data");

                return ServiceResult<bool>.Success(response.IsSuccessStatusCode);
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

        public async Task<ServiceResult<Quiz>> GetQuizAsync(int id)
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.GetAsync($"{url}/{id}"));

                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ServiceResult<Quiz>
                        .Failure("UnAuthorized");
                }
                else if (!response.IsSuccessStatusCode)
                    return ServiceResult<Quiz>.Failure("Error: Something wrong has happened while receiving the data");
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return ServiceResult<Quiz>.Failure("There are no quizzes for today.");

                var result = await response.Content.ReadFromJsonAsync<Quiz>();
                return ServiceResult<Quiz>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<Quiz>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<Quiz>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<Quiz>.Failure($"Error: An unexpected system error occurred.");
            }
        }

        public async Task<ServiceResult<List<Quiz>>> GetQuizzesAsync()
        {
            try
            {
                var response = await apiClient.SendAsync(client => client.GetAsync(url));

                var text = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ServiceResult<List<Quiz>>
                        .Failure("UnAuthorized");
                }
                else if (!response.IsSuccessStatusCode)
                    return ServiceResult<List<Quiz>>.Failure("Error: Something wrong has happened while receiving the data.");
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return ServiceResult<List<Quiz>>.Failure("There are no quizzes.");

                var result = await response.Content.ReadFromJsonAsync<List<Quiz>>();
                return ServiceResult<List<Quiz>>.Success(result);
            }
            catch (TaskCanceledException)
            {
                return ServiceResult<List<Quiz>>.Failure("Error: Connection Timeout.");
            }
            catch (HttpRequestException)
            {
                return ServiceResult<List<Quiz>>.Failure("Error: Network connection failed.");
            }
            catch (Exception)
            {
                return ServiceResult<List<Quiz>>.Failure($"Error: An unexpected system error occurred.");
            }
        }
    }
}
