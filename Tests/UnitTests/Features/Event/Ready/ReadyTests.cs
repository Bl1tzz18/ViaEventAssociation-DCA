using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using Xunit;

namespace UnitTests.Features.Event.Ready;

public class ReadyTests
{
    
    private readonly ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;

    // [Fact]
    // public void S1_ReadEvent_ValidData_EventReadied()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.UpdateEventTitle("Sample Event Title");
    //     viaEvent.UpdateEventDescription("Sample Event Description");
    //     viaEvent.UpdateEventTimeSpan(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));
    //     viaEvent.SetEventMaxGuests(20);
    //     viaEvent.Visibility = EventVisibility.Public;
    //
    //     // Act
    //     var result = viaEvent.SetEventReady();
    //
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(EventStatus.Ready, viaEvent.Status);
    // }

    [Theory]
    [InlineData(null, "Sample Event Description", EventStatus.Draft)]
    [InlineData("Sample Event Title", null, EventStatus.Draft)]
    [InlineData("Sample Event Title", "Sample Event Description", EventStatus.Draft)]
    [InlineData("Sample Event Title", "Sample Event Description", EventStatus.Draft, false)]
    [InlineData("Sample Event Title", "Sample Event Description", EventStatus.Draft, true, 55)]
    public void F1_ReadEvent_InvalidData_FailureReturned(string title, string description, EventStatus status, bool setVisibility = true, int? maxGuests = null)
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.UpdateEventTitle(title ?? "Sample Event Title");
        viaEvent.UpdateEventDescription(description ?? "Sample Event Description");
        viaEvent.UpdateEventTimeSpan(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));
        if (maxGuests.HasValue)
            viaEvent.SetEventMaxGuests(maxGuests.Value);
        if (setVisibility)
            viaEvent.Visibility = EventVisibility.Public;

        // Act
        var result = viaEvent.SetEventReady();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void F2_ReadEvent_CancelledStatus_FailureReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.CancelEvent();

        // Act
        var result = viaEvent.SetEventReady();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void F3_ReadEvent_StartDateTimeInPast_FailureReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.UpdateEventTitle("Sample Event Title");
        viaEvent.UpdateEventDescription("Sample Event Description");
        viaEvent.UpdateEventTimeSpan(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

        // Act
        var result = viaEvent.SetEventReady();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void F4_ReadEvent_DefaultTitle_FailureReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.SetEventReady();

        // Assert
        Assert.True(result.IsFailure);
    }
}