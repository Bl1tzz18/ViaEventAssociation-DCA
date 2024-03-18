using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation_DCA.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Util;

public class EventDateTime : DateTimeRange
{
    private static readonly TimeSpan EarliestStart = new(8, 0, 0); // 08:00 AM
    private static readonly TimeSpan LatestStart = new(0, 0, 0); // 00:00 AM, events cannot start after midnight
    private static readonly TimeSpan LATEST_END; // 01:00 AM
    private static readonly TimeSpan MaxDuration = new(10, 0, 0); // 10 hours
    private static readonly TimeSpan MinEventDuration = new(1, 0, 0); // 30 minutes

    protected EventDateTime(DateTime start, DateTime end) : base(start, end)
    {
    }

    public static Result<EventDateTime> Create(DateTime start, DateTime end)
    {
        try
        {
            var validation = Validate(start, end);
            return validation.IsSuccess ? new EventDateTime(start, end) : validation.OperationErrors;
        }
        catch (Exception exception)
        {
            return Result<EventDateTime>.Failure(new List<ExceptionModel>
                { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }

    private static Result Validate(DateTime start, DateTime end)
    {
        var errors = new List<ExceptionModel>();
        
        if (start >= end)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid date range."));

        // Check if the start time is before 08:00 and after 00:00 AM.
        if (start.TimeOfDay < EarliestStart && start.TimeOfDay > LatestStart)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest,
                "Invalid start date time. Start time must be after 08:00 AM and before midnight."));

        // Check if MinEventDuration is satisfied
        if (end - start < MinEventDuration)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest,
                $"Event duration is too short. Minimum required duration is {MinEventDuration}"));

        // Check if the end time is set to valid operating hours
        
        if (end.Date == start.Date)
        {
            // If it's the same day, ensure the end is no later than midnight, and the start must be after 08:00 AM
            if (end.TimeOfDay >= LATEST_END && start.TimeOfDay < EarliestStart)
                errors.Add(new ExceptionModel(ReasonEnum.BadRequest,
                    "Invalid end date time. End time must be no later than 01:00 AM."));
        }
        else if (end.Date > start.Date)
        {
            // If it's the next day, ensure the end time is no later than 01:00 AM.
            if (end.TimeOfDay > LATEST_END)
                errors.Add(new ExceptionModel(ReasonEnum.BadRequest,
                    "Invalid end date time. End time must be no later than 01:00 AM."));
        }
        
        return errors.Any() ? Result<EventDateTime>.Failure(errors) : Result.Success();
    }
}