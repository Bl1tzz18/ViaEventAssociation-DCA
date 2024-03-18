using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;
using ViaEventAssociation_DCA.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Locations;

public class Location
{
    private const int MIN_NUMBER_OF_GUESTS = 1;

    public LocationName Name { get; set; }
    public NumberOfGuests MaxNumberOfGuests { get; set; }
    public List<ViaEvent> Events { get; }

    public DateTimeRange AvailableTime { get; set; }

    private Location(LocationName name, NumberOfGuests maxNumberOfGuests) {
        Name = name;
        MaxNumberOfGuests = maxNumberOfGuests;
        Events = new List<ViaEvent>();
    }

    public static Result<Location> Create() {
        try {
            var locationName = LocationName.Create("Working Title");
            if (locationName.IsFailure)
                return Result<Location>.Failure(locationName.OperationErrors);
            var maxGuests = NumberOfGuests.Create(MIN_NUMBER_OF_GUESTS);
            if (maxGuests.IsFailure)
                return Result<Location>.Failure(maxGuests.OperationErrors);
            return Result<Location>.Success(new Location(locationName.Payload, maxGuests.Payload));
        }
        catch (Exception exception) {
            return Result<Location>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }
    }

    public Result UpdateName(string name) {
        var locationName = LocationName.Create(name);
        if (locationName.IsFailure)
            return Result<Location>.Failure(locationName.OperationErrors);
        Name = locationName.Payload;
        return Result.Success();
    }

    public Result AddEvent(ViaEvent @event) {
        try {
            Events.Add(@event);
        }
        catch (Exception exception) {
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
        }

        return Result.Success();
    }

    public Result SetMaxNumberOfGuests(int maxNumberOfGuests) {
        var maxGuests = NumberOfGuests.Create(maxNumberOfGuests);
        if (maxGuests.IsFailure)
            return Result<Location>.Failure(maxGuests.OperationErrors);
        MaxNumberOfGuests = maxGuests.Payload;
        return Result.Success();
    }

    public Result setsAvailableTimeSpan(DateTimeRange timeRange) {
        var errors = new List<ExceptionModel>();

        if (DateTimeRange.isPast(timeRange))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "The time range is in the past."));

        foreach (var @event in Events)
            if (!IsEventWithinTimeSpan(@event, timeRange)) {
                errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "An event is outside the new availability time span."));
                break;
            }

        if (errors.Any())
            return Result.Failure(errors);

        AvailableTime = timeRange;
        return Result.Success();
    }

    private bool IsEventWithinTimeSpan(ViaEvent @event, DateTimeRange timeRange) {
        return @event.TimeSpan.Start >= timeRange.Start && @event.TimeSpan.End <= timeRange.End;
    }

    public bool isAvailable(DateTimeRange timeRange) {
        return Validate(timeRange).IsSuccess ? true : false;
    }

    private Result Validate(DateTimeRange timeRange) {
        var errors = new List<ExceptionModel>();

        if (AvailableTime.Start < timeRange.Start && AvailableTime.End > timeRange.End)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "The location is not available."));

        if (Events.Any(e => e.TimeSpan.Overlaps(timeRange)))
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "An event is overlapping the time range."));

        if (errors.Any())
            return Result.Failure(errors);

        return Result.Success();
    }

    public override string ToString() {
        return $"Location: {Name} - MaxNumberOfGuests: {MaxNumberOfGuests} - AvailableTimeSpan: {AvailableTime}";
    }
}