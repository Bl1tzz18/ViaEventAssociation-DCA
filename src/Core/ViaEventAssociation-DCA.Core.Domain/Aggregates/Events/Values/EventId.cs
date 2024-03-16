namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;

public class EventId
{
    public Guid Value { get; }

    public EventId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("EventId cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static implicit operator Guid(EventId eventId)
    {
        return eventId.Value;
    }

    public static implicit operator EventId(Guid eventId)
    {
        return new(eventId);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}