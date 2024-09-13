using notifier.Application.ProjectOfficials.Quries.GetAllProjectOfficials;

namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;



public class GetAllProjectOfficialQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly GetAllProjectOfficialQueryHandler _handler;

    public GetAllProjectOfficialQueryHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new GetAllProjectOfficialQueryHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_ProjectOfficialsExist_ShouldReturnSuccessResult()
    {
        // Arrange
        var projectOfficials = new List<ProjectOfficial>
        {
            new() { Id = 1, Responsible = "John Doe",Mobile = "0923152546",TelegramId = "@JohnDOE" },
            new() { Id = 2, Responsible = "Jane Doe",Mobile = "0923565478",TelegramId = "@JaneDOE" }
        };

        _unitOfWorkSubstitute.ProjectOffcialRepo.GetAll()
            .Returns(Task.FromResult((IEnumerable<ProjectOfficial>)projectOfficials));

        var request = new GetAllProjectOfficialQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_NoProjectOfficialsExist_ShouldReturnFailureResult()
    {
        // Arrange
        _unitOfWorkSubstitute.ProjectOffcialRepo.GetAll()
            .Returns(Task.FromResult<IEnumerable<ProjectOfficial>>(null!));

        var request = new GetAllProjectOfficialQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_EmptyProjectOfficialsList_ShouldReturnFailureResult()
    {
        // Arrange
        _unitOfWorkSubstitute.ProjectOffcialRepo.GetAll()
            .Returns(Task.FromResult(Enumerable.Empty<ProjectOfficial>()));

        var request = new GetAllProjectOfficialQueryRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
