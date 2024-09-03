using notifier.Application.Services.Commands.DeleteService;

namespace NotifierTests.HandlerTests.ServiceHandlers;


public class DeleteServiceCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly DeleteServiceCommandHandler _handler;

    public DeleteServiceCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new DeleteServiceCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenServiceDoesNotExist()
    {
        // Arrange
        var request = new DeleteServiceCommandRequest { Id = 1 };
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _unitsOfWorks.ServiceRepo?.GetById(1).Returns(Task.FromResult<Service>(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Project doesn't exist.");
    }

    [Fact]
    public async Task Handle_ShouldDeleteService_WhenServiceExists()
    {
        // Arrange
        var service = new Service { Id = 1, Title = "Test Service",Url = "dskdskslkskakl",Ip = "123.123.123",Port = 123,Method = "Get" };
        var request = new DeleteServiceCommandRequest { Id = 1 };

        _unitsOfWorks.ServiceRepo.GetById(1).Returns(Task.FromResult(service));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Test Service Deleted Successfully");

        // Verify that the service was deleted from the repository
        _unitsOfWorks.ServiceRepo.Received(1).Delete(service);

        // Verify that SaveChanges was called
        await _unitsOfWorks.Received(1).SaveChanges();
    }
}
