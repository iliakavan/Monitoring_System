using notifier.Application.ProjectOfficials.Quries.Search;


namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;



public class SearchProjectOfficialHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchProjectOfficialQueryHandler _handler;

    public SearchProjectOfficialHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchProjectOfficialQueryHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoProjectOfficialsFound()
    {
        // Arrange
        var request = new SearchProjectOfficialQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Responsible = "John Doe",
            Mobile = "1234567890",
            TelegramId = "telegram123",
            ProjectId = 5
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.ProjectOffcialRepo.Search(startdateEN, enddateEn, request.Responsible, request.Mobile, request.TelegramId, request.ProjectId)
            .Returns(Task.FromResult(Enumerable.Empty<ProjectOfficialDto>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectOfficialsFound()
    {
        // Arrange
        var request = new SearchProjectOfficialQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            Responsible = "John Doe",
            Mobile = "1234567890",
            TelegramId = "telegram123",
            ProjectId = 2
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var mockProjectOfficials = new List<ProjectOfficialDto>
        {
            new ProjectOfficialDto {Responsible = "John Doe", Mobile = "1234567890", TelegramId = "telegram123", RecordDate = DateTime.Now, ProjectId = 1 }
        };

        _unitsOfWorksSubstitute.ProjectOffcialRepo.Search(startdateEN, enddateEn, request.Responsible, request.Mobile, request.TelegramId,request.ProjectId)
            .Returns(Task.FromResult<IEnumerable<ProjectOfficialDto>>(mockProjectOfficials));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(mockProjectOfficials);
    }
}
