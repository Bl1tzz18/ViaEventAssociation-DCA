using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Values;
using Xunit;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventTests
{
    [Fact]
    public void CreateEvent_WhenCreatorSelectsToCreateEvent_ThenEmptyEventIsCreatedWithStatusDraftAndMaxGuestsSetTo5()
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
                
        // Act
        var result = ViaEvent.Create(creator);

        // Assert
        Assert.True(result.IsSuccess); 
                
        var createdEvent = result.Payload;
        Assert.NotNull(createdEvent); 
                
        // Check event properties
        Assert.Equal(EventStatus.Draft, createdEvent.Status); 
        Assert.Equal(5, createdEvent.NumberOfGuests.Value); 
    }
    
    [Fact]
    public void CreateEvent_WhenCreatorSelectsToCreateEvent_DescriptionIsEmpty()
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
                
        // Act
        var result = ViaEvent.Create(creator);

        // Assert
        Assert.True(result.IsSuccess); 
                
        var createdEvent = result.Payload;
        Assert.NotNull(createdEvent); 
        
        Assert.Equal("", createdEvent.Description.Value);
    }

    
    [Fact]
    public void CreateEvent_WhenCreatorSelectsToCreateEvent_VisibilityIsPrivate()
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
                
        // Act
        var result = ViaEvent.Create(creator);

        // Assert
        Assert.True(result.IsSuccess); 
                
        var createdEvent = result.Payload;
        Assert.NotNull(createdEvent); 
        
        Assert.Equal(EventVisibility.Private, createdEvent.Visibility); // Assuming Visibility is a property of type EventVisibility
    }
    
    [Fact]
    public void CreateEvent_WhenCreatorSelectsToCreateEvent_TitleIsSetToWorkingTitle()
    {
        // Arrange
        var creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    
        // Act
        var result = ViaEvent.Create(creator);

        // Assert
        Assert.True(result.IsSuccess); 
            
        var createdEvent = result.Payload;
        createdEvent.UpdateEventTitle("Working Title");
        Assert.NotNull(createdEvent); 
            
        // Check event properties
        Assert.Equal("Working Title", createdEvent.Title.Value); // Assuming Title is a property of type EventTitle
    }


    
}