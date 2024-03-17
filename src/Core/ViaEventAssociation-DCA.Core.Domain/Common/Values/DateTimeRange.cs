using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Common.Values;

public class DateTimeRange : ValueObject {
    protected DateTimeRange(DateTime start, DateTime end) {
        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }

    public static Result<DateTimeRange> Create(DateTime start, DateTime end) {
            var validation = Validate(start, end);
            
            return new DateTimeRange(start, end);
    }

    private static Result Validate(DateTime start, DateTime end) {
        var errors = new List<ExceptionModel>();
        
        if (start > end)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid DateTimeRange"));

        if (isPast(new DateTimeRange(start, end)))
        {
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "DateTimeRange is in the past"));
        }
        
        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Start;
        yield return End;
    }

    public static bool isPast(DateTimeRange dateTimeRange) {
        return dateTimeRange.Start < DateTime.Now;
    }

    public static bool isFuture(DateTimeRange dateTimeRange) {
        return dateTimeRange.Start > DateTime.Now;
    }

    public override string ToString() => $"Start: {Start}, End: {End}";
}