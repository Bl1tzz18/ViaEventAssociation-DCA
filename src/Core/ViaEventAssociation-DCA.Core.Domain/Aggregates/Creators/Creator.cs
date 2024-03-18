using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators;

public class Creator
{
    private CreatorId CreatorId { get; }
    public CreatorName CreatorName { get; }

    public Email CreatorEmail { get; }

    private Creator(CreatorId id, CreatorName name, Email email)
    {
        CreatorId = id;
        CreatorName = name;
        CreatorEmail = email;
    }

    public static Result<Creator> Create(string name, string email)
    {
        List<ExceptionModel> errors = new List<ExceptionModel>();

        var creatorIdResult = CreatorId.GenerateId();
        if (creatorIdResult.IsFailure)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, creatorIdResult.OperationErrors.First().ErrorMessage));

        var nameResult = CreatorName.Create(name);
        if (nameResult.IsFailure)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, nameResult.OperationErrors.First().ErrorMessage));

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, emailResult.OperationErrors.First().ErrorMessage));

        if (errors.Any())
            return Result<Creator>.Failure(errors);

        return new Creator(creatorIdResult.Payload, nameResult.Payload, emailResult.Payload);
    }
    
    public Result<ViaEvent> CreateEvent() {
        var eventResult = ViaEvent.Create(this);
        return eventResult.IsSuccess ? eventResult.Payload : eventResult.SingleOperationError;
    }
}