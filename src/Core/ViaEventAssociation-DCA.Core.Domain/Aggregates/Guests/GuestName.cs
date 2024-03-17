using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;

public class GuestName : ValueObject
{
    private GuestName(string name)
    {
        Value = name;
    }
    public string Value { get; }
    private static readonly int MAX_LENGHT = 30;
    private static readonly int MIN_LENGHT = 1;

    public static Result<GuestName> Create(string name)
    {
        try {
            var validation = Validate(name);
            if (validation.IsFailure)
                return Result<GuestName>.Failure(validation.OperationErrors);

            return Result<GuestName>.Success(new GuestName(name));
        }
        catch (Exception exception) {
            return Result<GuestName>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
    
    private static Result Validate(string name) {
        var errors = new List<ExceptionModel>();

        if (name == null)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name cannot be null"));

        if (string.IsNullOrEmpty(name))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name cannot be empty"));

        if (name.Length < MIN_LENGHT)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Name must be at least {MIN_LENGHT} characters long"));

        if (name.Length > MIN_LENGHT)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Name must be at most {MAX_LENGHT} characters long"));
        
        if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Name can only contain letters"));
        

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}