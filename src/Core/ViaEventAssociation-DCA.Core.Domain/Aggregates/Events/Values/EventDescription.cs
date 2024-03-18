using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;

public class EventDescription : ValueObject
{
    private static readonly int MIN_DESCRIPTION_LENGTH = 0;
    private static readonly int MAX_DESCRIPTION_LENGTH = 250;
    public string Value { get; }

    public EventDescription(string value)
    {
        Value = value;
    }

    public static Result<EventDescription> Create(string description)
    {
        try {
            var validation = Validate(description);

            if (validation.IsFailure)
                return Result<EventDescription>.Failure(new List<ExceptionModel> {
                    new ExceptionModel(
                        ReasonEnum.BadRequest, validation.OperationErrors.First().ErrorMessage
                    )});
            return new EventDescription(description);
        }
        catch (Exception exception) {
            return Result<EventDescription>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
    
    private static Result Validate(string description)
    {
        var errors = new List<ExceptionModel>();

        if (description.Length < MIN_DESCRIPTION_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Description is too short."));

        if (description.Length > MAX_DESCRIPTION_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Description is too long."));

        if (errors.Any())
            return Result<EventDescription>.Failure(errors);

        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}