namespace ViaEventAssociation.Core.Tools.OperationResult.Errors;

/// <summary>
/// Exception model
/// </summary>
public class ExceptionModel
{
    public ReasonEnum ErrorCode { get; set; }
    public string ErrorMessage { get; init; }
    public ExceptionModel(ReasonEnum errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}