using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators;

public class CreatorName : ValueObject
{
    public string Value { get; }
    private static readonly int MAX_LENGTH = 30;

    public CreatorName(string value)
    {
        Value = value;
    }
    
    public static Result<CreatorName> Create(string name)
    {
        try
        {
            var validation = Validate(name);
            if (validation.IsSuccess) return new CreatorName(name);

            return Result<CreatorName>.Failure(validation.OperationErrors);
        }
        catch (Exception exception)
        {
            return Result<CreatorName>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
    
    public static Result Validate(string name)
    {
        List<ExceptionModel> errors = new List<ExceptionModel>();

        if (name == null)
            return Result<CreatorName>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Name cannot be null") });

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name cannot be empty"));

        if (name.Length > MAX_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name is too long"));

        if (errors.Any())
            return Result<CreatorName>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Name is invalid") });

        return Result<CreatorName>.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}