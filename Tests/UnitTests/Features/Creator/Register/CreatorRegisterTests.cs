using ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests.Features.Creator.Register;

public class CreatorRegisterTests
{
    [Xunit.Theory]
    [InlineData("John Doe", "JHE@via.dk")] 
    [InlineData("Jane Smith", "315170@via.dk")] 
    [InlineData("Alice Wonderland", "ALW@via.dk")]
    public void Register_CreatorWithValidNameAndEmail_ReturnsSuccess(string name, string email)
    {
        // Act
        var result = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create(name, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(name, result.Payload.CreatorName.Value);
        Assert.Equal(email, result.Payload.CreatorEmail.Value);
    }

    [Xunit.Theory]
    [InlineData(null, "johndoe@via.dk")] // Null name
    [InlineData("", "johndoe@via.dk")] // Empty name
    [InlineData("John Doe", null)] // Null email
    [InlineData("John Doe", "")] // Empty email
    [InlineData("John Doe", "invalidemail@domain.com")] // Invalid email format
    public void Register_InvalidCreator_ReturnsFailure(string name, string email)
    {
        // Act
        var result = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create(name, email);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Xunit.Theory]
    [InlineData(null, "johndoe@via.dk")] // Null name
    [InlineData("", "johndoe@via.dk")] // Empty name
    [InlineData("John Doe", null)] // Null email
    [InlineData("John Doe", "")] // Empty email
    [InlineData("asdsadsadwefewfasfdsadsadsadsadsadsadsadsaddsadsadsa", "test@via.dk")] // Over 30 chars
    public void Register_InvalidName_ReturnsFailure(string name, string email)
    {
        // Act
        var result = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create(name, email);

        // Assert
        Assert.True(result.IsFailure);
    }
}