using notifier.Application.ServiceNotifications.Commands.AddServiceNotification;

namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;


public class AddServiceNotificationCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly AddServiceNotificationCommandHandler _handler;

    public AddServiceNotificationCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new AddServiceNotificationCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldAddNotificationAndReturnSuccess()
    {
        // Arrange
        var request = new AddServiceNotificationCommandRequest
        {
            MessageFormat = "Test message",
            MessageSuccess = "test",
            RetryCount = 3,
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            ServiceTestId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that Add was called on the NotificationRepo with the correct parameters
        await _unitsOfWorks.NotificationRepo.Received(1).Add(Arg.Is<ServiceNotfications>(sn =>
            sn.MessageFormat == request.MessageFormat &&
            sn.RetryCount == request.RetryCount &&
            sn.NotificationType == request.NotificationType &&
            sn.ServiceTestId == request.ServiceTestId &&
            sn.RecordDate.Date == DateTime.Now.Date // Check only the date part to avoid time comparison issues
        ));

        // Verify that SaveChanges was called once
        await _unitsOfWorks.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAddFails()
    {
        // Arrange
        var request = new AddServiceNotificationCommandRequest
        {
            MessageFormat = "Test message",
            MessageSuccess = "test",
            RetryCount = 3,
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            ServiceTestId = 1
        };

        // Simulate an exception when trying to add a notification
        _unitsOfWorks.NotificationRepo.When(x => x.Add(Arg.Any<ServiceNotfications>()))
            .Do(x => { throw new Exception("Database error"); });

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
