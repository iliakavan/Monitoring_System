using notifier.Application.Services.Queries.Search;

namespace NotifierTests.HandlerTests.ServiceHandlers;


public class SearchServiceHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchServiceQueryHandler _handler;

    public SearchServiceHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchServiceQueryHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoServicesFound()
    {
        // Arrange
        var request = new SearchServiceQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Nonexistent Service",
            Url = "http://nonexistent.com",
            Ip = "192.168.1.1",
            Port = 8080,
            ProjectId = 1
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ServiceRepo.Search(startdateEN, enddateEn, request.Title, request.Url, request.Ip, request.Port, request.ProjectId)
            .Returns(Task.FromResult<IEnumerable<ServiceDto?>>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Service between {request.StartDate} {request.EndDate}");
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenServiceListIsEmpty()
    {
        // Arrange
        var request = new SearchServiceQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Empty Service",
            Url = "http://emptyservice.com",
            Ip = "192.168.1.1",
            Port = 8080,
            ProjectId = 1
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ServiceRepo.Search(startdateEN, enddateEn, request.Title, request.Url, request.Ip, request.Port, request.ProjectId)
            .Returns(await Task.FromResult(Enumerable.Empty<ServiceDto>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Service between {request.StartDate} {request.EndDate}");
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenServicesAreFound()
    {
        // Arrange
        var request = new SearchServiceQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Test Service",
            Url = "http://testservice.com",
            Ip = "192.168.1.2",
            Port = 8081,
            ProjectId = 2
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var mockServices = new List<ServiceDto?>
        {
            new ServiceDto { Id = 1, Title = "Test Service", Url = "http://testservice.com", Ip = "192.168.1.2", Port = 8081, ProjectId = 2,Method = "Get" }
        };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _unitsOfWorksSubstitute?.ServiceRepo.Search(StartDate: startdateEN, enddateEn, request.Title, request.Url, request.Ip, request.Port, request.ProjectId)
            .Returns(Task.FromResult<IEnumerable<ServiceDto>>(mockServices));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(mockServices);
    }
}
