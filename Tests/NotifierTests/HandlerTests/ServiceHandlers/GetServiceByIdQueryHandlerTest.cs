using notifier.Application.Services.Queries.GetServiceById;

namespace NotifierTests.HandlerTests.ServiceHandlers;


public class GetServiceByIdQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly GetServiceByIdQueryHandler _handler;

    public GetServiceByIdQueryHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new GetServiceByIdQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ServiceExists_ReturnsServiceDto()
    {
        // Arrange
        var serviceId = 1;
        var service = new Service
        {
            Title = "Test Service",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET"
        };

        _unitsOfWorks.ServiceRepo.GetById(serviceId).Returns(service);

        var request = new GetServiceByIdQueryRequest { Id = serviceId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be(service.Title);
        result.Value.Url.Should().Be(service.Url);
        result.Value.Ip.Should().Be(service.Ip);
        result.Value.Port.Should().Be(service.Port);
        result.Value.Method.Should().Be(service.Method);
    }

    [Fact]
    public async Task Handle_ServiceDoesNotExist_ReturnsErrorMessage()
    {
        // Arrange
        var serviceId = 1;
        _unitsOfWorks.ServiceRepo.GetById(serviceId).Returns((Service)null);

        var request = new GetServiceByIdQueryRequest { Id = serviceId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Service doesn't exist.");
    }
}
