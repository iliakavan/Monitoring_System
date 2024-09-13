using notifier.Application.Services.Commands.AddService;

namespace NotifierTests.HandlerTests.ServiceHandlers;



public class AddServiceCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly AddServiceCommandHandler _handler;

    public AddServiceCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new AddServiceCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRequestIsNull()
    {
        // Arrange
        AddServiceCommandRequest request = null!;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("request not valid");
    }

    [Fact]
    public async Task Handle_ShouldAddService_WhenRequestIsValid()
    {
        // Arrange
        var request = new AddServiceCommandRequest
        {
            Title = "Test Service",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET",
            ProjectId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that the service was added to the repository
        await _unitsOfWorks.ServiceRepo.Received(1).Add(Arg.Is<Service>(s =>
            s.Title == request.Title &&
            s.Url == request.Url &&
            s.Ip == request.Ip &&
            s.Port == request.Port &&
            s.Method == request.Method &&
            s.RecordDate.Date == DateTime.Now.Date && // Compare only date part
            s.ProjectId == request.ProjectId));

        // Verify that SaveChanges was called
        await _unitsOfWorks.Received(1).SaveChanges();
    }
}
