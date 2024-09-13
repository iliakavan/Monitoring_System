using notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;

namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;



public class UpdateServiceNotificationCommnadHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly UpdateServiceNotificationCommandHandler _handler;

    public UpdateServiceNotificationCommnadHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateServiceNotificationCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenNotificationExists()
    {
        // Arrange
        var request = new UpdateServiceNotificationCommandRequest
        {
            Id = 1,
            ServiceTestId = 2,
            MessageFormat = "New Format",
            MessageSuccess = "test",
            NotificationType = notifier.Domain.Enum.NotificationType.Telegram,
            RetryCount = 3
        };

        var existingNotification = new ServiceNotfications { Id = 1 ,MessageFormat = "Old" ,MessageSuccess = "test", };

        // Mock the behavior of GetById to return a valid notification
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns(existingNotification);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that Update was called with the updated notification
        _unitsOfWorks.NotificationRepo.Received(1).Update(Arg.Is<ServiceNotfications>(n =>
            n.ServiceTestId == request.ServiceTestId &&
            n.MessageFormat == request.MessageFormat &&
            n.NotificationType == request.NotificationType &&
            n.RetryCount == request.RetryCount));

        await _unitsOfWorks.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotificationDoesNotExist()
    {
        // Arrange
        var request = new UpdateServiceNotificationCommandRequest { Id = 1 , MessageFormat = "smdslm" };

        // Mock the behavior of GetById to return null (notification not found)
        _unitsOfWorks.NotificationRepo.GetById(request.Id).Returns((ServiceNotfications)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        // Verify that Update and SaveChanges were not called
        _unitsOfWorks.NotificationRepo.DidNotReceive().Update(Arg.Any<ServiceNotfications>());
        await _unitsOfWorks.DidNotReceive().SaveChanges();
    }
}
