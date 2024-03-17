using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Entities;

public class ParticipationId : IdentityBase {
    private ParticipationId(string prefix) : base(prefix) { }

    public static Result<ParticipationId> GenerateId() {
        try {
            return new ParticipationId("PId");
        }
        catch (Exception exception) {
            return Result<ParticipationId>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });;
        }
    }
}