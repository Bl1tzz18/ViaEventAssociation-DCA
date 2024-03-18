using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Entities;

public class Participation : Entity<ParticipationId>
{
    public Guest Guest { get; private set; }
    public ViaEvent Event { get; private set; }
    public ParticipationType ParticipationType { get; private set; }
    public ParticipationStatus ParticipationStatus { get; protected set; }
    
    public Participation(ParticipationId id, ViaEvent @event, Guest guest, ParticipationType participationType, ParticipationStatus participationStatus) : base(id)
    {
        Event = @event;
        Guest = guest;
        ParticipationType = participationType;
        ParticipationStatus = participationStatus;
    }
    
    public Result CancelParticipation()
    {
        if (Event.isEventPast())
            return Result.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, "Event is past.") });        
        ParticipationStatus = ParticipationStatus.Rejected;
        return Result.Success();
    }
}