using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using Xunit;

namespace UnitTests.Features.Event.MakePublic;

public class MakePublicTests
{
    // Success scenarios
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void S1_MakeEventPublic_ValidStatus_PublicAndStatusUnchanged(EventStatus status)
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.Status = status;

        // Act
        var result = viaEvent.MakeEventPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, viaEvent.Visibility);
        Assert.Equal(status, viaEvent.Status);
    }

    // Failure scenarios
    [Fact]
    public void F1_MakeEventPublic_CancelledStatus_FailureMessage()
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
        var viaEvent = ViaEvent.Create(creator).Payload;
        viaEvent.Status = EventStatus.Cancelled;

        // Act
        var result = viaEvent.MakeEventPublic();

        // Assert
        Assert.True(result.IsFailure);
    //     var error = result.Errors.Single() as ExceptionModel;
    //     Assert.NotNull(error);
    //     Assert.Equal(ReasonEnum.BadRequest, error.Reason);
    //     Assert.Equal("Event is cancelled.", error.Message);
    //
    }
}