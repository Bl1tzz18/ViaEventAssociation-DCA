using System.Text.RegularExpressions;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Common.Values;

public class Email : ValueObject
{
    public string Value { get; }
    
    private Email(string value) {
        Value = value;
    }
    
    public static Result<Email> Create(string email) {
        try {
            var validation = Validate(email);
            if (validation.IsSuccess)
            {
                return Result<Email>.Success(new Email(email));
            }
            else
            {
                return Result<Email>.Failure(validation.OperationErrors.ToList());
            }
        }
        catch (Exception exception)
        {
            return Result<Email>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }
    
    private static Result Validate(string email)
    {
        var errors = new List<ExceptionModel>();
        
        if (email == null)
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Email cannot be null")); 
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Email cannot be empty"));
        }
        
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid email"));
        }
        
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@via\.dk$"))
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid email domain"));
        }
        
        var localPart = email.Split('@')[0];
        if (localPart.Length < 3 || localPart.Length > 6)
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid email"));
        }
        
        if (!(Regex.IsMatch(localPart, @"^[a-zA-Z]{3,4}$") || Regex.IsMatch(localPart, @"^\d{6}$")))
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid email"));
        }
        
        
        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}