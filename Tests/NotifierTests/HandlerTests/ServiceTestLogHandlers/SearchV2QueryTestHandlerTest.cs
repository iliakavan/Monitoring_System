using notifier.Application.ServiceTestLogs.Query.SearchV2;

namespace NotifierTests.HandlerTests.ServiceTestLogHandlers;



public class SearchV2QueryTestHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchServiceTestLogV2QueryHandler _handler;

    public SearchV2QueryTestHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchServiceTestLogV2QueryHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoLogsFound()
    {
        // Arrange
        var request = new SearchServiceTestLogV2QueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            ServiceId = 1,
            ResponseCode = "200",
            TestType = notifier.Domain.Enum.TestType.TelNet,
            ProjectId = 123,
            Ip = "127.0.0.1",
            Port = 8080,
            Url = "http://example.com"
        };
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ServiceTestLogRepo.Search(
            startdateEN,
            enddateEn,
            request.ServiceId,
            request.ResponseCode,
            request.TestType,
            request.ProjectId,
            request.Ip,
            request.Port,
            request.Url
        ).Returns(Task.FromResult<IEnumerable<object>>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLogsFound()
    {
        // Arrange
        var request = new SearchServiceTestLogV2QueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            ServiceId = 1,
            ResponseCode = "200",
            TestType = notifier.Domain.Enum.TestType.Curl,
            ProjectId = 123,
            Ip = "127.0.0.1",
            Port = 8080,
            Url = "http://example.com"
        };
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var serviceTestLogs = new List<ServiceTestLog>
        {
            new ServiceTestLog() {Id = 1 , ResponseCode = "90",RecordDate = DateTime.Now, ServiceId = 10 },
            new ServiceTestLog()    { Id = 2 , ResponseCode = "90",RecordDate = DateTime.Now, ServiceId = 10}
        };

        _unitsOfWorksSubstitute.ServiceTestLogRepo.Search(
            startdateEN,
            enddateEn,
            request.ServiceId,
            request.ResponseCode,
            request.TestType,
            request.ProjectId,
            request.Ip,
            request.Port,
            request.Url
        ).Returns(Task.FromResult<IEnumerable<object>>(serviceTestLogs));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(serviceTestLogs);
    }
}
