using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Locations;
using Xunit;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuests
{
    private readonly ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
    
    [Fact]
    public void S1_SetMaxGuests_ValidValuesLessThan50_GuestsSetAndStatusUnchanged()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        var selectedNumberOfGuests = 25;

        // Act
        var result = viaEvent.SetEventMaxGuests(selectedNumberOfGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(selectedNumberOfGuests, viaEvent.NumberOfGuests.Value);
        Assert.Equal(EventStatus.Draft, viaEvent.Status);
    }

    [Fact]
    public void S2_SetMaxGuests_ValidValuesBetween5And50_GuestsSet()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        var selectedNumberOfGuests = 10;

        // Act
        var result = viaEvent.SetEventMaxGuests(selectedNumberOfGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(selectedNumberOfGuests, viaEvent.NumberOfGuests.Value);
    }

    [Fact]
    public void S3_SetMaxGuests_ActiveStatus_NumberOfGuestsIncreased_GuestsSetAndStatusUnchanged()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.SetEventReady();
        viaEvent.Status = EventStatus.Active;
        var selectedNumberOfGuests = 30;
        viaEvent.SetEventMaxGuests(20); // Previous number of guests

        // Act
        var result = viaEvent.SetEventMaxGuests(selectedNumberOfGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(selectedNumberOfGuests, viaEvent.NumberOfGuests.Value);
        Assert.Equal(EventStatus.Active, viaEvent.Status);
    }

    [Theory]
    [InlineData(3)] // Less than 5
    [InlineData(55)] // More than 50
    public void F4_F5_SetMaxGuests_InvalidValues_FailureReturned(int numberOfGuests)
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.SetEventMaxGuests(numberOfGuests);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void F1_SetMaxGuests_ActiveStatus_NumberOfGuestsDecreased_FailureReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.SetEventReady();
        viaEvent.Status = EventStatus.Active;
        viaEvent.SetEventMaxGuests(20); // Previous number of guests

        // Act
        var result = viaEvent.SetEventMaxGuests(15); // Reducing the number of guests

        // Assert
        Assert.True(result.IsFailure);
    }

    // [Fact]
    // public void F2_SetMaxGuests_CancelledStatus_FailureReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.CancelEvent();
    //
    //     // Act
    //     var result = viaEvent.SetEventMaxGuests(30);
    //
    //     // Assert
    //     Assert.True(result.IsFailure);
    // }

    // [Fact]
    // public void F3_SetMaxGuests_ExceedLocationCapacity_FailureReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.Location = new Location { MaxCapacity = 20 }; // Assuming the location has a maximum capacity of 20
    //
    //     // Act
    //     var result = viaEvent.SetEventMaxGuests(30); // Trying to set more guests than location capacity
    //
    //     // Assert
    //     Assert.True(result.IsFailure);
    // }
    
}