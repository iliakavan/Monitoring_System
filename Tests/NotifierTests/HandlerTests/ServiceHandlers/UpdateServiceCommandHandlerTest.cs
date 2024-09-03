using notifier.Application.Services.Commands.UpdateService;

namespace NotifierTests.HandlerTests.ServiceHandlers;



public class UpdateServiceCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly UpdateServiceCommandHandler _handler;

    public UpdateServiceCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateServiceCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenServiceDoesNotExist()
    {
        // Arrange
        var request = new UpdateServiceCommandRequest
        {
            Title = "Test Service",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET"
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _unitsOfWorks.ServiceRepo.GetById(1).Returns(Task.FromResult<Service>(null));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Service doesnt Exist");
    }

    [Fact]
    public async Task Handle_ShouldUpdateService_WhenServiceExists()
    {
        // Arrange
        var service = new Service { 
            Id = 1, Title = "Old Title" ,
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET"
        };
        var request = new UpdateServiceCommandRequest
        {
            Id = 1,
            Title = "New Title",
            Url = "http://newurl.com",
            Method = "GET",
            Port = 80,
            Ip = "192.168.1.1",
            ProjectId = 2
        };

        _unitsOfWorks.ServiceRepo.GetById(1).Returns(Task.FromResult(service));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("1 Updated Successfully");

        // Verify that the service was updated with the new values
        service.Title.Should().Be(request.Title);
        service.Url.Should().Be(request.Url);
        service.Method.Should().Be(request.Method);
        service.Port.Should().Be(request.Port);
        service.Ip.Should().Be(request.Ip);
        service.ProjectId.Should().Be(request.ProjectId);

        // Verify that the update method was called
        _unitsOfWorks.ServiceRepo.Received(1).Update(service);
    }
}
