using DNTPersianUtils.Core;
using notifier.Application.ServiceTestLogs.Query.SearchV1;

namespace NotifierTests.HandlerTests.ServiceTestLogHandlers;



public class SearchQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchServiceTestLogHandler _handler;

    public SearchQueryHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchServiceTestLogHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoLogsFound()
    {
        // Arrange
        var request = new SearchServiceTestLog { StartDate = "1402/05/10", EndDate = "1402/05/15", Serviceid = 1 };
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ServiceTestLogRepo.Search(startdateEN, enddateEn, request.Serviceid)
            .Returns(Task.FromResult<IEnumerable<ServiceTestLog>>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLogListIsEmpty()
    {
        // Arrange
        var request = new SearchServiceTestLog { StartDate = "1402/05/10", EndDate = "1402/05/15", Serviceid = 2 };
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ServiceTestLogRepo.Search(startdateEN, enddateEn, request.Serviceid)
            .Returns(Task.FromResult(Enumerable.Empty<ServiceTestLog>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLogsAreFound()
    {
        // Arrange
        var request = new SearchServiceTestLog { StartDate = "1402/05/10", EndDate = "1402/05/15", Serviceid = 3 };
        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var mockLogs = new List<ServiceTestLog>
        {
            new ServiceTestLog { Id = 1, ServiceId = 3, RecordDate = DateTime.Now ,ResponseCode = "404" }
        };

        _unitsOfWorksSubstitute.ServiceTestLogRepo.Search(startdateEN, enddateEn, request.Serviceid)
            .Returns(Task.FromResult<IEnumerable<ServiceTestLog>>(mockLogs));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(mockLogs);
    }
}
