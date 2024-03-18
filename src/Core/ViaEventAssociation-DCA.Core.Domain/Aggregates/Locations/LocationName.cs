using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Locations;

public class LocationName : ValueObject
{
    public string Value { get; private set; }
    private const int MIN_NAME_LENGTH = 1;
    private const int MAX_NAME_LENGTH = 100;
    
    private LocationName(string value)
    {
        Value = value;
    }

    public static Result<LocationName> Create(string name)
    {
        try
        {
            var validation = Validate(name);
            if (validation.IsFailure)
                return Result<LocationName>.Failure(validation.OperationErrors);
            return Result<LocationName>.Success(new LocationName(name));
        }
        catch (Exception exception)
        {
            return Result<LocationName>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }

    private static Result Validate(string name)
    {
        var errors = new List<ExceptionModel>();

        if (name == null)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name is null"));

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name is empty or whitespace"));

        if (name.Length < MIN_NAME_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Name is shorter than {MIN_NAME_LENGTH} characters"));

        if (name.Length > MAX_NAME_LENGTH)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Name is longer than {MAX_NAME_LENGTH} characters"));

        if (errors.Any())
            return Result.Failure(errors);

        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}