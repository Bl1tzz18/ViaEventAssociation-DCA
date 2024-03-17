namespace ViaEventAssociation_DCA.Core.Domain.Common.Bases;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : ValueObject
{
    protected AggregateRoot(TId id) : base(id) { }
    
}