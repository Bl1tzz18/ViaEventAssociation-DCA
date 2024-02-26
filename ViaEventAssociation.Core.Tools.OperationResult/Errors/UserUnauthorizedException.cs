namespace ViaEventAssociation.Core.Tools.OperationResult.Errors;

public class UserUnauthorizedException : Exception
{
    /// <inheritdoc />
    public UserUnauthorizedException(ReasonEnum reason, string? message, Exception? innerException) : base(message, innerException)
    {
        Reason = reason;
        Data[nameof(Reason)]=Reason;
    }

    /// <summary>
    /// Reason for the exception
    /// </summary>
    public ReasonEnum Reason { get; }
}