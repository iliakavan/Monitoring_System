using notifier.Application.ServiceTests.Queries.Search;


namespace NotifierTests.HandlerTests.SerivceTestHandlers;

public class SearchServiceTestQueryHandlerTests
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly SearchServiceTestQueryHandler _handler;

    public SearchServiceTestQueryHandlerTests()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchServiceTestQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenServiceTestIsEmptyOrNull()
    {
        // Arrange
        var request = new SearchServiceTestQueryRequest
        {
            StartDate = DateTime.Now.ToString(),
            EndDate = DateTime.Now.AddDays(1).ToString(),
            Serviceid = 1,
            Projectid = 1
        };

        _unitsOfWorks.ServiceTestRepo.Search(Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), request.Serviceid, request.Projectid)
            .Returns(Task.FromResult<IEnumerable<ServiceTestDto>>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("ServiceTest is Empty Or Null");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenServiceTestIsFound()
    {
        // Arrange
        var request = new SearchServiceTestQueryRequest
        {
            StartDate = DateTime.Now.ToString(),
            EndDate = DateTime.Now.AddDays(1).ToString(),
            Serviceid = 1,
            Projectid = 1
        };

        var serviceTests = new List<ServiceTestDto>
        {
            new ServiceTestDto { /* Initialize properties */ },
            new ServiceTestDto { /* Initialize properties */ }
        };

        _unitsOfWorks.ServiceTestRepo.Search(Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), request.Serviceid, request.Projectid)
            .Returns(Task.FromResult(serviceTests.AsEnumerable()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceTests);
    }
}
