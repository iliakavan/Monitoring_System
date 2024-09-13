using notifier.Application.Services.Queries.GetAllService;

namespace NotifierTests.HandlerTests.ServiceHandlers;



public class GetAllServiceQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly GetAllServiceQueryHandler _handler;

    public GetAllServiceQueryHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new GetAllServiceQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenNoServicesFound()
    {
        // Arrange
        _unitsOfWorks.ServiceRepo.GetAll().Returns(Task.FromResult<IEnumerable<Service>>(null!));

        var request = new GetAllServiceQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to fetch");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenServiceRepoReturnsEmptyList()
    {
        // Arrange
        _unitsOfWorks.ServiceRepo.GetAll().Returns(Task.FromResult(Enumerable.Empty<Service>()));

        var request = new GetAllServiceQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to fetch");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessfulResult_WhenServicesAreFound()
    {
        // Arrange
        var services = new List<Service> { new Service() {  Title = "Test Service",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET",
            ProjectId = 1}, new Service() {  Title = "Test Service1",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET",
            ProjectId = 1} };
        _unitsOfWorks.ServiceRepo.GetAll().Returns(Task.FromResult(services.AsEnumerable()));

        var request = new GetAllServiceQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(services);
    }
}
