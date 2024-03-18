using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Locations;

public class LocationId : IdentityBase
{
    protected LocationId(string prefix) : base(prefix)
    {
        
    }
    
    public static Result<LocationId> GenerateId() {
        try {
            return new LocationId("LID");
        }
        catch (Exception exception) {
            return Result<LocationId>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
}