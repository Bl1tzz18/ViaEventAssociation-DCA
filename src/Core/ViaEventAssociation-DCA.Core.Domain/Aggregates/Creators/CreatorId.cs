using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators;

public class CreatorId : IdentityBase
{
    protected CreatorId(string prefix) : base(prefix)
    {
    }
    
    public static Result<CreatorId> GenerateId()
    {
        try
        {
            return new CreatorId("CId");
        }
        catch (Exception exception)
        {
            return Result<CreatorId>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
}