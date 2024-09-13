using notifier.Application.Projects.Queries.GetById;

namespace NotifierTests.HandlerTests.ProjectHandlers;



public class GetProjectByIdHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new GetProjectByIdQueryHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_NonExistentProject_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new GetProjectByIdQueryRequest { Id = 1 };
        _unitOfWorkSubstitute.ProjectRepo.GetById(1)!.Returns(Task.FromResult<Project>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Project doesn't exist.");
    }

    [Fact]
    public async Task Handle_ExistingProject_ShouldReturnSuccessResultWithProjectDto()
    {
        // Arrange
        var project = new Project { Id = 1, Title = "Test Project", Description = "Description" };
        var request = new GetProjectByIdQueryRequest { Id = 1 };
        _unitOfWorkSubstitute.ProjectRepo.GetById(1)!.Returns(Task.FromResult(project));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Test Project");
        result.Value.Description.Should().Be("Description");
    }

}
