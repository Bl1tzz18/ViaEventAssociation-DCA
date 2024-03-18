using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;

public class NumberOfGuests : ValueObject
{
    public int Value { get; }
    
    private const int MIN_NUMBER_OF_GUESTS = 5;
    private const int MAX_NUMBER_OF_GUESTS = 50;

    public NumberOfGuests(int value)
    {
        Value = value;
    }

    public static Result<NumberOfGuests> Create(int numberOfGuests) {
        try {
            var validation = Validate(numberOfGuests);
            if (validation.IsFailure)
                return Result<NumberOfGuests>.Failure(new List<ExceptionModel> {
                    new ExceptionModel(ReasonEnum.BadRequest, "Invalid number of guests")
                });
            return Result<NumberOfGuests>.Success(new NumberOfGuests(numberOfGuests));
        }
        catch (Exception exception) {
            return Result<NumberOfGuests>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }

    private static Result Validate(int numberOfGuests) {
        if (numberOfGuests < MIN_NUMBER_OF_GUESTS)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Too few guests") });
        if (numberOfGuests > MAX_NUMBER_OF_GUESTS)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Too many guests") });
        
        return Result.Success();
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}