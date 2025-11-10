namespace Clinic_Project.Helpers
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string? Message { get; }
        public enErrorType ErrorType { get; }
        public ValidationResult(bool isValid, string? message = null, enErrorType errorType = enErrorType.None)
        {
            Message = message;
            ErrorType = errorType;
            IsValid = isValid;
        }

        public static ValidationResult Success() => new ValidationResult(true);

        public static ValidationResult Fail(string errorMsg, enErrorType errorType) => 
            new ValidationResult(false, errorMsg, errorType);
    }




}
