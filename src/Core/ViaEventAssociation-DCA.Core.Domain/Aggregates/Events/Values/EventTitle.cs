using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;

public class EventTitle : ValueObject {
    private const int MIN_TITLE_LENGTH = 3;
    private const int MAX_TITLE_LENGTH = 75;

    private EventTitle(string title) {
        Value = title;
    }

    public string Value { get; }

    public static Result<EventTitle> Create(string title) {
        try {
            var validation = Validate(title);
            if (validation.IsFailure)
                return validation.SingleOperationError;
            return new EventTitle(title);
        }
        catch (Exception exception)
        {
            return Result<EventTitle>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });}
    }

    private static Result Validate(string title) {
        var errors = new List<ExceptionModel>();

        if (title == null)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Title is null") });

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Title is empty"));

        if (title.Length < MIN_TITLE_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Title is too short, minimum length is {MIN_TITLE_LENGTH} characters"));

        if (title.Length > MAX_TITLE_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Title is too long, maximum length is {MAX_TITLE_LENGTH} characters"));
        if (errors.Any())
            return Result.Failure(errors);

        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}