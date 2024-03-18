using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;
using Xunit;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleTests
{
    [Theory]
[InlineData("Scary Movie Night!")]
[InlineData("Graduation Gala")]
[InlineData("VIA Hackathon")]
public void UpdateEventTitle_WhenTitleIsWithinValidLength_AndEventIsInDraftStatus_TitleIsUpdated(string newTitle)
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;

    // Act
    var result = viaEvent.UpdateEventTitle(newTitle);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(newTitle, viaEvent.Title.Value);
    Assert.Equal(EventStatus.Draft, viaEvent.Status);
}

[Theory]
[InlineData("Scary Movie Night!")]
[InlineData("Graduation Gala")]
[InlineData("VIA Hackathon")]
public void UpdateEventTitle_WhenTitleIsWithinValidLength_AndEventIsInReadyStatus_TitleIsUpdatedAndEventIsDraft(string newTitle)
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;
    viaEvent.Status = EventStatus.Ready;

    // Act
    var result = viaEvent.UpdateEventTitle(newTitle);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(newTitle, viaEvent.Title.Value);
    Assert.Equal(EventStatus.Draft, viaEvent.Status);
}

[Theory]
[InlineData("")]
[InlineData(null)]
public void UpdateEventTitle_WhenTitleIsNullOrEmpty_FailureMessageReturned(string newTitle)
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;

    // Act
    var result = viaEvent.UpdateEventTitle(newTitle);

    // Assert
    Assert.False(result.IsSuccess);
}

[Theory]
[InlineData("XY")]
[InlineData("a")]
public void UpdateEventTitle_WhenTitleIsLessThan3Characters_FailureMessageReturned(string newTitle)
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;

    // Act
    var result = viaEvent.UpdateEventTitle(newTitle);

    // Assert
    Assert.False(result.IsSuccess);
}

[Theory]
[InlineData ("This is a very long title that is more than 75 characters long and should not be allowed")]

public void UpdateEventTitle_WhenTitleIsMoreThan75Characters_FailureMessageReturned(string newTitle)
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;

    // Act
    var result = viaEvent.UpdateEventTitle(newTitle);

    // Assert
    Assert.False(result.IsSuccess);
}

[Fact]
public void UpdateEventTitle_WhenEventIsInActiveStatus_FailureMessageReturned()
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;
    viaEvent.Status = EventStatus.Active;

    // Act
    var result = viaEvent.UpdateEventTitle("New Title");

    // Assert
    Assert.False(result.IsSuccess);
}

[Fact]
public void UpdateEventTitle_WhenEventIsInCancelledStatus_FailureMessageReturned()
{
    // Arrange
    var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    var viaEvent = ViaEvent.Create(creator).Payload;
    viaEvent.Status = EventStatus.Cancelled;

    // Act
    var result = viaEvent.UpdateEventTitle("New Title");

    // Assert
    Assert.False(result.IsSuccess);
}

}