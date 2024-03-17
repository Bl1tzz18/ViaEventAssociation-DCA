using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using System.Collections.Generic;
using System.Linq;

namespace ViaEventAssociation.Core.Tools.OperationResult
{
    public abstract class Result
    {
        public List<ExceptionModel> OperationErrors { get; protected set; } = new();
        public bool IsSuccess => OperationErrors.Count == 0;
        public bool IsFailure => !IsSuccess;
        public static Result Success() => new ResultNoPayload();
        public static Result Failure(List<ExceptionModel> errors) => new ResultNoPayload(errors);
    }

    internal class ResultNoPayload : Result
    {
        internal ResultNoPayload() { }
        internal ResultNoPayload(List<ExceptionModel> errors) : base() { OperationErrors = errors; }
    }

    public class Result<T> : Result
    {
        public Result(List<ExceptionModel> errors) : base() => OperationErrors = errors;
        public Result(T payload) => Payload = payload;
        public T Payload { get; private set; } = default!;
        
        
        
        public static Result<T> Success(T payload) => new Result<T>(payload);
        public new static Result<T> Failure(List<ExceptionModel> errors) => new Result<T>(errors);

        public static implicit operator Result<T>(T payload) => Success(payload);

        public static implicit operator Result<T>(ExceptionModel error) =>
            Failure(new List<ExceptionModel> { error });

        public static implicit operator Result<T>(List<ExceptionModel> errors) => Failure(errors);

        public static Result<T> Combine(params Result<T>[] results)
        {
            var combinedErrors = results.SelectMany(result => result.OperationErrors).ToList();

            if (combinedErrors.Count != 0)
            {
                return new Result<T>(combinedErrors);
            }

            var payload = results.Where(result => result.IsSuccess && !Equals(result.Payload, default(T)))
                .Select(result => result.Payload)
                .FirstOrDefault();

            return payload != null ? Success(payload) : Success(default(T)!);
        }
    }
}