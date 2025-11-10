namespace Clinic_Project.Helpers
{
    public enum enErrorType
    {
        None,
        NotFound,
        BadRequest,
        Conflict,
        Forbiden,
        Unauthorized
    }
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public enErrorType ErrorType { get; set; }

        public static Result<T> Ok(T? data) => new() { Success = true, Data = data };
        public static Result<T> Fail(string errorMessage, enErrorType errorType = enErrorType.NotFound) => new() 
        { 
            Success = false,
            ErrorMessage = errorMessage,
            ErrorType = errorType
        }; 
    }
}
