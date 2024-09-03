using notifier.Application.ServiceNotifications.Queries.GetAllServiceNotification;


namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;


public class GetAllServiceNotificationQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly GetAllServiceNotificationQueryHandler _handler;

    public GetAllServiceNotificationQueryHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new GetAllServiceNotificationQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenNotificationsExist()
    {
        // Arrange
        var notifications = new List<ServiceNotfications>
        {
            new(){Id = 1,MessageFormat = "Test message1",
            MessageSuccess = "test",
            RetryCount = 3,
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            ServiceTestId = 1}, // Add properties as needed
            new(){Id = 2,MessageFormat = "Test message1",
            MessageSuccess = "test",
            RetryCount = 3,
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            ServiceTestId = 1}
        };

        _unitsOfWorks.NotificationRepo.GetAll().Returns(Task.FromResult(notifications.AsEnumerable()));

        var request = new GetAllServiceNotificationQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(notifications);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoNotificationsExist()
    {
        // Arrange
        _unitsOfWorks.NotificationRepo.GetAll().Returns(Task.FromResult(Enumerable.Empty<ServiceNotfications>()));

        var request = new GetAllServiceNotificationQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
