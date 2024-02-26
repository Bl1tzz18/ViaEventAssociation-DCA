using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation.Core.Tools.OperationResult
{
    public class Result
    {
        private bool _isFailure = false;
        private List<ExceptionModel> ErrorMessage = new();
        public bool IsSuccess => !_isFailure;
        
        public static Result Success() => new();
        public static Result Failure(List<ExceptionModel> errorMessages) => new() { _isFailure = true, ErrorMessage = errorMessages };
        
        public IEnumerable<ExceptionModel> Errors => ErrorMessage;
    }
    
    public class Result<T>
    {
        private bool _isFailure;
        public T Value { get; private set; } = default!;
        public List<ExceptionModel> ErrorMessage { get; private set; } = new();
        public Result(T value) => Value = value;
        public Result(List<ExceptionModel> errorMessage) => ErrorMessage = errorMessage;
        public static Result<T> Success(T value) => new Result<T>(value);
        public new static Result<T> Failure(List<ExceptionModel> errorMessages) => new Result<T>(errorMessages);
        public new IEnumerable<ExceptionModel> Errors => ErrorMessage;
    }
}