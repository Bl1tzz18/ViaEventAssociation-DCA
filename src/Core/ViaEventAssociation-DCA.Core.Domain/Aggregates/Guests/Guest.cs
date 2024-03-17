using ViaEventAssociation_DCA.Core.Domain.Common.Bases;
using ViaEventAssociation_DCA.Core.Domain.Common.Values;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;

public class Guest : AggregateRoot<GuestId>
{
    
    //TODO: Implement participation
    public GuestName FirstName { get; }
    public GuestName LastName { get; }
    public Email Email { get; }
    
    
    public Guest(GuestId id, GuestName fName, GuestName lName, Email email) : base(id)
    {
        FirstName = fName;
        LastName = lName;
        Email = email;
    }
}