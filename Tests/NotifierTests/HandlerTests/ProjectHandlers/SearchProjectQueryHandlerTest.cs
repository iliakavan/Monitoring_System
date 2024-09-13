using DNTPersianUtils.Core;
using notifier.Application.Projects.Queries.Search;

namespace NotifierTests.HandlerTests.ProjectHandlers;


public class SearchProjectQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchProjectQueryHandler _handler;

    public SearchProjectQueryHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchProjectQueryHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoProjectsFound()
    {
        // Arrange
        var request = new SearchProjectQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Nonexistent Project"
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ProjectRepo.Search(startdateEN, enddateEn, request.Title)!
            .Returns(Task.FromResult<IEnumerable<Project>>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Project between {request.StartDate} {request.EndDate} or with the title {request.Title}");
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectListIsEmpty()
    {
        // Arrange
        var request = new SearchProjectQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Empty Project"
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ProjectRepo.Search(startdateEN, enddateEn, request.Title)!
            .Returns(Task.FromResult(Enumerable.Empty<Project>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Project between {request.StartDate} {request.EndDate} or with the title {request.Title}");
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectsAreFound()
    {
        // Arrange
        var request = new SearchProjectQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Title = "Test Project"
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var mockProjects = new List<Project>
        {
            new Project { Id = 1, Title = "Test Project", Description = "Description", RecordDate = DateTime.Now }
        };

        _unitsOfWorksSubstitute.ProjectRepo.Search(startdateEN, enddateEn, request.Title)!
            .Returns(Task.FromResult<IEnumerable<Project>>(mockProjects));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(mockProjects);
    }
}
