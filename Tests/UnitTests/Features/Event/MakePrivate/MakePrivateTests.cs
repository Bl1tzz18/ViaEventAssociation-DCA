using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using Xunit;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateTests
{
    // Success scenarios
        [Theory]
        [InlineData(EventStatus.Draft, EventVisibility.Private)]
        [InlineData(EventStatus.Ready, EventVisibility.Private)] //TODO: This test is not valid, because the event is not private (modify so it also stays private for Ready)
        public void S1_MakeEventPrivate_AlreadyPrivate_NoChanges(EventStatus status, EventVisibility visibility)
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = status;
            viaEvent.Visibility = visibility;

            // Act
            var result = viaEvent.MakeEventPrivate();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventVisibility.Private, viaEvent.Visibility);
            Assert.Equal(status, viaEvent.Status);
        }

        // [Theory]
        // [InlineData(EventStatus.Draft)]
        // [InlineData(EventStatus.Ready)]
        // public void S2_MakeEventPrivate_PublicEvent_MadePrivate(EventStatus status)
        // {
        //     // Arrange
        //     var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
        //     var viaEvent = ViaEvent.Create(creator).Payload;
        //     viaEvent.Status = status;
        //     viaEvent.Visibility = EventVisibility.Public;
        //
        //     // Act
        //     var result = viaEvent.MakeEventPrivate();
        //
        //     // Assert
        //     Assert.True(result.IsSuccess);
        //     Assert.Equal(EventVisibility.Private, viaEvent.Visibility);
        //     Assert.Equal(EventStatus.Draft, viaEvent.Status);
        // }

        // Failure scenarios
        [Fact]
        public void F1_MakeEventPrivate_ActiveEvent_FailureMessage()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = EventStatus.Active;

            // Act
            var result = viaEvent.MakeEventPrivate();

            // Assert
            Assert.True(result.IsFailure);
            // var error = result.Errors.Single() as ExceptionModel;
            // Assert.NotNull(error);
            // Assert.Equal(ReasonEnum.BadRequest, error.Reason);
            // Assert.Equal("An active event cannot be made private.", error.Message);
        }

        [Fact]
        public void F2_MakeEventPrivate_CancelledEvent_FailureMessage()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("Catalin","315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = EventStatus.Cancelled;

            // Act
            var result = viaEvent.MakeEventPrivate();

            // Assert
            Assert.True(result.IsFailure);
            // var error = result.Errors.Single() as ExceptionModel;
            // Assert.NotNull(error);
            // Assert.Equal(ReasonEnum.BadRequest, error.Reason);
            // Assert.Equal("A cancelled event cannot be modified.", error.Message);
        }
}