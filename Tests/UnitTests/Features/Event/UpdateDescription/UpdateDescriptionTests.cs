using ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Entities;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using Xunit;

namespace UnitTests.Features.Event.UpdateDescription
{
    public class UpdateDescriptionTests
    {
        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliquaorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliquaorem.")]
        public void S1_UpdateEventDescription_ValidLength_DescriptionIsUpdated(string newDescription)
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;

            // Act
            var result = viaEvent.UpdateEventDescription(newDescription);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription, viaEvent.Description.Value);
        }

        [Fact]
        public void S2_UpdateEventDescription_EmptyDescription_DescriptionIsSetToEmpty()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;

            // Act
            var result = viaEvent.UpdateEventDescription("");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("", viaEvent.Description.Value);
        }

        [Fact]
        public void S3_UpdateEventDescription_ReadyStatus_DescriptionIsUpdatedAndStatusIsDraft()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = EventStatus.Ready;

            // Act
            var result = viaEvent.UpdateEventDescription("New Description");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Description", viaEvent.Description.Value);
            Assert.Equal(EventStatus.Draft, viaEvent.Status);
        }

        [Fact]
        public void F1_UpdateEventDescription_DescriptionTooLong_FailureMessageReturned()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            var longDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliquaorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliquaorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliquaorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            // Act
            var result = viaEvent.UpdateEventDescription(longDescription);

            // Assert
            Assert.False(result.IsSuccess);
            // Assert.Contains("Description is too long", result.OperationErrors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public void F2_UpdateEventDescription_CancelledStatus_FailureMessageReturned()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = EventStatus.Cancelled;

            // Act
            var result = viaEvent.UpdateEventDescription("New Description");

            // Assert
            Assert.False(result.IsSuccess);
            // Assert.Contains("Event is cancelled", result.OperationErrors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public void F3_UpdateEventDescription_ActiveStatus_FailureMessageReturned()
        {
            // Arrange
            var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
            var viaEvent = ViaEvent.Create(creator).Payload;
            viaEvent.Status = EventStatus.Active;

            // Act
            var result = viaEvent.UpdateEventDescription("New Description");

            // Assert
            Assert.False(result.IsSuccess);
            // Assert.Contains("Event is already active", result.OperationErrors.Select(error => error.ErrorMessage));
        }
    }
}
