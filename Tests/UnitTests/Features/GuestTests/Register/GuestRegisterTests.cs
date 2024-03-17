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
    public void Create_UserWithValidEmail_ReturnsSuccess(string firstName, string lastName, string email)
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
    public void Create_InvalidUser_ReturnsFailure(string firstName, string lastName, string email)
    {
        // Act
        var result = Guest.Create(firstName, lastName, email);

        // Assert
        Assert.True(result.IsFailure);
    }
}
   