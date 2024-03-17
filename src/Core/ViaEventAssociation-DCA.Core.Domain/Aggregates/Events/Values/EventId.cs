using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;

public class EventId : IdentityBase
{
    protected EventId(string prefix) : base(prefix)
    {
    }
    
    public static Result<EventId> GenerateId()
    {
        try
        {
            return new EventId("EID");
        }
        catch (Exception exception)
        {
            return Result<EventId>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
}