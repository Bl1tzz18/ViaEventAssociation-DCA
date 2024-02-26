namespace ViaEventAssociation.Core.Tools.OperationResult.Errors;

/// <summary>
/// Exception model
/// </summary>
public class ExceptionModel
{
    /// <summary>
    /// Error code (see <see cref="StatusCodes"/>)
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Error message
    /// </summary>
    public string ErrorMessage { get; init; }
    
    protected ExceptionModel(int errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}