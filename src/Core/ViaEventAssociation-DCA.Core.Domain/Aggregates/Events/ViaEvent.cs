using ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Entities;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Util;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Locations;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;

public class ViaEvent : AggregateRoot<EventId>
{
    private const int MIN_NUMBER_OF_GUESTS = 5;
    private const int MAX_NUMBER_OF_GUESTS = 50;

    private const string DEFAULT_TITLE_EVENT = "I want to die";
    public Creator Creator { get; private set; }
    public EventTitle Title { get; set; }
    public EventDateTime TimeSpan { get; set; }
    public EventDescription Description { get; set; }
    public EventVisibility Visibility { get; set; }
    public EventStatus Status { get; set; }
    public NumberOfGuests NumberOfGuests { get; set; }
    public HashSet<Participation> Participations { get; private set; }
    public Location Location { get; private set; }
    private int ParticipationsConfirmed => Participations.Count(p => p.ParticipationStatus is ParticipationStatus.Accepted);

    
    public ViaEvent(EventId id) : base(id)
    {
    }
    
    public static Result<ViaEvent> Create(Creator creator) {
        var newEvent = new ViaEvent(EventId.GenerateId().Payload) {
            Creator = creator,
            Title = EventTitle.Create(DEFAULT_TITLE_EVENT).Payload,
            NumberOfGuests = NumberOfGuests.Create(MIN_NUMBER_OF_GUESTS).Payload,
            Status = EventStatus.Draft,
            Visibility = EventVisibility.Private,
            Description = EventDescription.Create("").Payload,
            Participations = new HashSet<Participation>()
        };
        return newEvent;
    }
    
    public Result MakeEventPublic() {
        if (Status is EventStatus.Cancelled)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled.") });
        Visibility = EventVisibility.Public;
        return Result.Success();
    }
    public Result MakeEventPrivate()
    {
        if (Status is EventStatus.Active)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Event is already active.") });

        if (Status is EventStatus.Cancelled)
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled.") });

        Visibility = EventVisibility.Private;
        return Result.Success();
    }

    
    public Result UpdateEventTitle(string title) {
        var errors = new List<ExceptionModel>();

        // Attempt to create a new EventTitle object with the provided title
        var newTitleResult = EventTitle.Create(title);
        if (newTitleResult.IsFailure)
        {
            // If creation fails, add the errors to the list and return a failure result
            errors.AddRange(newTitleResult.OperationErrors);
            return Result.Failure(errors);
        }

        // Check if the event status is active or cancelled
        if (Status is EventStatus.Active) 
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is already active."));
        if (Status is EventStatus.Cancelled)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled."));
    
        if (errors.Any())
            return Result.Failure(errors);
    
        // Update the Title property only if the creation of the new title was successful
        Title = newTitleResult.Payload;
        Status = EventStatus.Draft;
    
        return Result.Success();
    }

    
    public Result UpdateEventTimeSpan(DateTime start, DateTime end) {
        var errors = new List<ExceptionModel>();
        var newTimeSpan = EventDateTime.Create(start, end)
            .OnFailure(error => errors.Add(error));
        if (Status is EventStatus.Active)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is already active."));
        if (Status is EventStatus.Cancelled)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled."));
        if (errors.Any())
            return Result.Failure(errors);
        TimeSpan = newTimeSpan.Payload;
        Status = EventStatus.Draft;
        return Result.Success();
    }
    
    public Result UpdateEventDescription(string description) {
        var errors = new List<ExceptionModel>();
        var newDescription = EventDescription.Create(description)
            .OnFailure(error => errors.Add(error));
        if (Status is EventStatus.Active)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is already active."));
        if (Status is EventStatus.Cancelled)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled."));
        if (errors.Any())
            return Result.Failure(errors);
        Description = newDescription.Payload;
        Status = EventStatus.Draft;
        return Result.Success();
    }
    
    public Result SetEventMaxGuests(int maxGuests) {
        var errors = new List<ExceptionModel>();
        if (Status is EventStatus.Active && maxGuests < NumberOfGuests.Value)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Cannot set max guests to a number lower than the current number of guests."));
        if (Status is EventStatus.Cancelled)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled."));
        if (maxGuests < MIN_NUMBER_OF_GUESTS)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Number of guests cannot be lower than {MIN_NUMBER_OF_GUESTS}"));
        if (maxGuests > MAX_NUMBER_OF_GUESTS)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, $"Number of guests cannot be higher than {MAX_NUMBER_OF_GUESTS}"));
        if (errors.Any())
            return Result.Failure(errors);

        NumberOfGuests = NumberOfGuests.Create(maxGuests).Payload;
        return Result.Success();
    }
    public Result CancelEvent() {
        if (Status is not EventStatus.Active)
            return Result.Failure( new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Event is not active.") });

        Status = EventStatus.Cancelled;
        return Result.Success();
    }
    public Result SetEventReady() {
        var errors = new List<ExceptionModel>();
        if (TimeSpan is null)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event time span is not set."));
        if (Status is EventStatus.Cancelled)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event is cancelled."));
        if (TimeSpan is not null && TimeSpan?.Start! < DateTime.Now) 
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event start time is in the past."));
        if (Title.Value == DEFAULT_TITLE_EVENT)
            errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Event title is not set."));
        if (errors.Any())
            return Result.Failure(errors);

        Status = EventStatus.Ready;
        return Result.Success();
    }
    
    
    public bool isEventPast()
    {
        throw new NotImplementedException();
    }
}