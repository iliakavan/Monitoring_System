using notifier.Application.Projects.Queries.GetAllProjects;

namespace NotifierTests.HandlerTests.ProjectHandlers;


public class GetAllProjectQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly GetAllProjectQueryHandler _handler;

    public GetAllProjectQueryHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new GetAllProjectQueryHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_NoProjects_ShouldReturnFailureResultWithNullValue()
    {
        // Arrange
        var request = new GetAllProjectQueryRequest();
        _unitOfWorkSubstitute.ProjectRepo.GetAll().Returns(Task.FromResult<IEnumerable<Project>>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to fetch");
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_EmptyProjects_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new GetAllProjectQueryRequest();
        _unitOfWorkSubstitute.ProjectRepo.GetAll().Returns(Task.FromResult<IEnumerable<Project>>(new List<Project>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to fetch");
    }

    [Fact]
    public async Task Handle_ValidProjects_ShouldReturnSuccessResult()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project { Id = 1, Title = "Project 1", Description = "Description 1" },
            new Project { Id = 2, Title = "Project 2", Description = "Description 2" }
        };
        var request = new GetAllProjectQueryRequest();
        _unitOfWorkSubstitute.ProjectRepo.GetAll().Returns(Task.FromResult<IEnumerable<Project>>(projects));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(projects);
    }
}
