using ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests;
using ViaEventAssociation_DCA.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;


namespace UnitTests.Features.GuestTests.Register;
using Xunit;

public class GuestRegisterTests
{
    
    [Theory]
    [InlineData("John", "Doe", "315170@via.dk")] 
    [InlineData("John", "Doe", "JHE@via.dk")] 
    [InlineData("John", "Doe", "ALHE@via.dk")] 
    [InlineData("John", "Doe", "315198@via.dk")]
    [InlineData("J", "H", "315170@via.dk")] // One letter first name and last name
    public void Register_UserWithValidEmailAndFirstNameAndLastName_ReturnsSuccess(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
        Assert.Equal(lastName, result.Payload.LastName.Value);
        Assert.Equal(email, result.Payload.Email.Value);
    }

    [Theory]
    [InlineData(null, "Doe", "john.doe@via.dk")] // Null first name
    [InlineData("", "Doe", "john.doe@via.dk")] // Empty first name
    [InlineData("John", null, "john.doe@via.dk")] // Null last name
    [InlineData("John", "", "john.doe@via.dk")] // Empty last name
    [InlineData("John", "Doe", null)] // Null email
    [InlineData("John", "Doe", "")] // Empty email
    [InlineData("John", "Doe", "invalidemail@domain.com")] // Invalid email format
    public void Register_InvalidUser_ReturnsFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Theory]
    [InlineData(null, "Doe", "315170@via.dk")] // Null first name
    [InlineData("", "Doe", "315170@via.dk")] // Empty first name
    [InlineData("John", null, "315170@via.dk")] // Null last name
    [InlineData("John", "", "315170@via.dk")] // Empty last name
    [InlineData("asdsadsadwefewfasfdsadsadsadsadsadsadsadsaddsadsadsa", "test", "315170@via.dk")] // Over 30 chars
    public void Register_InvalidName_ReturnsFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
    }
}

   