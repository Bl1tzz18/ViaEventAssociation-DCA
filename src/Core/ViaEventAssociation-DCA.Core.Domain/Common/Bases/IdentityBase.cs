namespace ViaEventAssociation_DCA.Core.Domain.Common.Bases;


// Represent the identity of an entity in domain model
public class IdentityBase : ValueObject
{
    private string Value { get; }
    
    protected IdentityBase(string prefix) {
        Value = prefix + Guid.NewGuid();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value;
}