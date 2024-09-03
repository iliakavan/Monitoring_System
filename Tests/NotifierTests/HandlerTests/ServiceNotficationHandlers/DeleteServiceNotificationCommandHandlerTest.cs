using notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;

namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;



public class DeleteServiceNotificationCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly DeleteServiceNotificationCommandHandler _handler;

    public DeleteServiceNotificationCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new DeleteServiceNotificationCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenNotificationExists()
    {
        // Arrange
        var request = new DeleteServiceNotificationCommandRequest { Id = 1 };
        var notification = new ServiceNotfications { Id = 1, MessageFormat = "smdslm", MessageSuccess = "Test" };

        // Mock the behavior of GetById to return a valid notification
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns(notification);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that Delete was called on the NotificationRepo with the correct notification
        _unitsOfWorks.NotificationRepo.Received(1).Delete(notification);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotificationDoesNotExist()
    {
        // Arrange
        var request = new DeleteServiceNotificationCommandRequest { Id = 1 };

        // Mock the behavior of GetById to return null (notification not found)
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns((ServiceNotfications)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        // Verify that Delete was not called since the notification does not exist
        _unitsOfWorks.NotificationRepo.DidNotReceive().Delete(Arg.Any<ServiceNotfications>());
    }
}
