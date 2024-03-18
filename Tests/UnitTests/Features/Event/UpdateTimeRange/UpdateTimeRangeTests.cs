using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events;
using ViaEventAssociation_DCA.Core.Domain.Aggregates.Events.Enums;
using Xunit;

namespace UnitTests.Features.Event.UpdateTimeRange;

public class UpdateTimeRangeTests
{
    private readonly ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator creator;

    public UpdateTimeRangeTests()
    {
        creator = ViaEventAssociation_DCA.Core.Domain.Aggregates.Creators.Creator.Create("TestCreator", "315170@via.dk").Payload;
    }

    // Success scenarios
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void S1_SetEventTimes_ValidTimes_TimesUpdated(string startTime, string endTime)
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse(startTime), DateTime.Parse(endTime));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(DateTime.Parse(startTime), viaEvent.TimeSpan.Start);
        Assert.Equal(DateTime.Parse(endTime), viaEvent.TimeSpan.End);
        Assert.Equal(EventStatus.Draft, viaEvent.Status);
    }

    // [Theory]
    // [InlineData("2023/08/25 19:00", "2023/08/26 01:00")]
    // [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    // [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    // public void S2_SetEventTimes_ValidTimes_TimesUpdated(string startTime, string endTime)
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse(startTime), DateTime.Parse(endTime));
    //
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(DateTime.Parse(startTime), viaEvent.TimeSpan.Start);
    //     Assert.Equal(DateTime.Parse(endTime), viaEvent.TimeSpan.End);
    //     Assert.Equal(EventStatus.Draft, viaEvent.Status);
    // }

    // [Fact]
    // public void S3_SetEventTimes_ValidValues_TimesUpdatedAndStatusDraft()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 19:00"), DateTime.Parse("2023/08/25 23:59"));
    //     viaEvent.SetStatus(EventStatus.Ready);
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 19:00"), DateTime.Parse("2023/08/25 23:59"));
    //
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(DateTime.Parse("2023/08/25 19:00"), viaEvent.TimeSpan.Start);
    //     Assert.Equal(DateTime.Parse("2023/08/25 23:59"), viaEvent.TimeSpan.End);
    //     Assert.Equal(EventStatus.Draft, viaEvent.Status);
    // }

    // [Fact]
    // public void S4_SetEventTimes_ValidValuesInFuture_TimesUpdated()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     var futureStartTime = DateTime.UtcNow.AddHours(1);
    //     var futureEndTime = futureStartTime.AddHours(2);
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(futureStartTime, futureEndTime);
    //
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(futureStartTime, viaEvent.TimeSpan.Start);
    //     Assert.Equal(futureEndTime, viaEvent.TimeSpan.End);
    // }

    [Fact]
    public void S5_SetEventTimes_ValidValuesWithDurationLessThan10Hours_TimesUpdated()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 19:00"), DateTime.Parse("2023/08/25 21:00"));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(DateTime.Parse("2023/08/25 19:00"), viaEvent.TimeSpan.Start);
        Assert.Equal(DateTime.Parse("2023/08/25 21:00"), viaEvent.TimeSpan.End);
    }

    // Failure scenarios
    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/25 01:00")]
    [InlineData("2023/08/26 19:00", "2023/08/26 14:00")]
    [InlineData("2023/08/26 19:00", "2023/08/26 18:59")]
    [InlineData("2023/08/26 12:00", "2023/08/26 10:10")]
    [InlineData("2023/08/26 08:00", "2023/08/26 00:30")]
    public void FailureScenarios(string startTime, string endTime)
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse(startTime), DateTime.Parse(endTime));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F1_StartDateAfterEndDate_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/26 19:00"), DateTime.Parse("2023/08/25 01:00"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F2_StartDateEqualsEndDateStartTimeAfterEndTime_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/26 19:00"), DateTime.Parse("2023/08/26 18:59"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F3_StartDateEqualsEndDateStartTimeLessThan1HourBeforeEndTime_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/26 14:00"), DateTime.Parse("2023/08/26 14:50"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F4_StartDateBeforeEndDateStartTimeLessThan1HourBeforeEndTime_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 23:30"), DateTime.Parse("2023/08/26 00:15"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F5_StartTimeBefore0800_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 07:50"), DateTime.Parse("2023/08/25 14:00"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    [Fact]
    public void F6_StartTimeBefore0100EndTimeAfter0100_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/24 23:50"), DateTime.Parse("2023/08/25 01:01"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }

    // [Fact]
    // public void F7_ActiveEventTimesModified_FailureMessageReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.SetStatus(EventStatus.Active);
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 19:00"), DateTime.Parse("2023/08/25 23:59"));
    //
    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    // }

    // [Fact]
    // public void F8_CancelledEventTimesModified_FailureMessageReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     viaEvent.SetStatus(EventStatus.Cancelled);
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/25 19:00"), DateTime.Parse("2023/08/25 23:59"));
    //
    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    // }

    // [Fact]
    // public void F9_EventDurationExceeds10Hours_FailureMessageReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/30 08:00"), DateTime.Parse("2023/08/30 18:01"));
    //
    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    // }

    // [Fact]
    // public void F10_StartTimeInPast_FailureMessageReturned()
    // {
    //     // Arrange
    //     var viaEvent = ViaEvent.Create(creator).Payload;
    //     var pastStartTime = DateTime.UtcNow.AddHours(-2);
    //     var pastEndTime = pastStartTime.AddHours(2);
    //
    //     // Act
    //     var result = viaEvent.UpdateEventTimeSpan(pastStartTime, pastEndTime);
    //
    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    // }

    [Fact]
    public void F11_StartTimeBefore01EndAfter08_FailureMessageReturned()
    {
        // Arrange
        var viaEvent = ViaEvent.Create(creator).Payload;

        // Act
        var result = viaEvent.UpdateEventTimeSpan(DateTime.Parse("2023/08/31 00:30"), DateTime.Parse("2023/08/31 08:30"));

        // Assert
        Assert.False(result.IsSuccess);
        // Assert.Contains(result.Errors, e => e.Code == ErrorCode.InvalidOperation);
    }
}