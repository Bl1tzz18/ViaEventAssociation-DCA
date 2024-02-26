namespace ViaEventAssociation.Core.Tools.OperationResult.Errors;

public enum ReasonEnum
{
    /// <summary>
    /// Unknown reason
    /// </summary>
    Unknown,
    /// <summary>
    /// Bad request reason
    /// </summary>
    BadRequest = 400,
    /// <summary>
    /// Unauthorized reason
    /// </summary>
    Unauthorized = 401,
    /// <summary>
    /// Forbidden reason
    /// </summary>
    Forbidden = 403,
    /// <summary>
    /// Not found reason
    /// </summary>
    NotFound = 404,
    /// <summary>
    /// Not Allowed reason
    /// </summary>
    NotAllowed = 405,
    /// <summary>
    /// Not Accepted reason
    /// </summary>
    NotAccepted = 406,
    /// <summary>
    /// Conflict reason
    /// </summary>
    Conflict = 409,
    /// <summary>
    /// ImATeapot reason
    /// </summary>
    ImATeapot = 418,
}