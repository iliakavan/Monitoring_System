using notifier.Application.ServiceNotifications.Queries.GetServiceNotification;

namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;


public  class GetServiceNotificationByIdQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly GetServiceNotificationByIdQueryHandler _handler;

    public GetServiceNotificationByIdQueryHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new GetServiceNotificationByIdQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenNotificationExists()
    {
        // Arrange
        var request = new GetServiceNotificationByIdQueryRequest { Id = 1 };
        var existingNotification = new ServiceNotfications
        {
            Id = 1,
            MessageFormat = "Format",
            MessageSuccess = "test",
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            RecordDate = DateTime.Now,
            ServiceTestId = 2,
            RetryCount = 3
        };

        // Mock the behavior of GetById to return a valid notification
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns(existingNotification);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.MessageFormat.Should().Be(existingNotification.MessageFormat);
        result.Value.NotificationType.Should().Be(existingNotification.NotificationType.ToString());
        result.Value.RecordDate.Should().Be(existingNotification.RecordDate);
        result.Value.ServiceTestId.Should().Be(existingNotification.ServiceTestId);
        result.Value.RetryCount.Should().Be(existingNotification.RetryCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotificationDoesNotExist()
    {
        // Arrange
        var request = new GetServiceNotificationByIdQueryRequest { Id = 1 };

        // Mock the behavior of GetById to return null (notification not found)
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns((ServiceNotfications)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
