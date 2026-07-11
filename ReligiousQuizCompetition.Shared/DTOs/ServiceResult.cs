namespace ReligiousQuizCompetition.Shared.DTOs
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult Success() => new() { IsSuccess = true };
        public static ServiceResult Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public static new ServiceResult<T> Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
    }
}
