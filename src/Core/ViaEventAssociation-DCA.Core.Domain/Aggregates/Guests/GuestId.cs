using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;

public class GuestId : IdentityBase
{
    protected GuestId(string prefix) : base(prefix)
    {
        
    }
    
    public static Result<GuestId> GenerateId()
    {
        try
        {
            return new GuestId("GuestID");
        }
        catch (Exception exception)
        {
            return Result<GuestId>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
}