namespace NotifierTests.HandlerTests.ProjectHandlers;




public class DeleteProjectHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new DeleteProjectCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_NullRequest_ShouldReturnFailureResult()
    {
        // Arrange
        DeleteProjectCommandRequest request = null!;

        // Act
        var result = await _handler.Handle(request!, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_NonExistentProject_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new DeleteProjectCommandRequest { Id = 1 };
        _unitOfWorkSubstitute.ProjectRepo.GetbyIdIncludeAll(1)!.Returns(Task.FromResult<Project>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Project doesn't exist.");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        var project = new Project { Id = 1, Title = "Test Project" };
        var request = new DeleteProjectCommandRequest { Id = 1 };
        _unitOfWorkSubstitute.ProjectRepo.GetbyIdIncludeAll(1)!.Returns(Task.FromResult(project));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Test Project Deleted Successfully");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldDeleteProjectFromRepository()
    {
        // Arrange
        var project = new Project { Id = 1, Title = "Test Project" };
        var request = new DeleteProjectCommandRequest { Id = 1 };
        _unitOfWorkSubstitute.ProjectRepo.GetbyIdIncludeAll(1)!.Returns(Task.FromResult(project));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkSubstitute.ProjectRepo.Received(1).Delete(project);
        await _unitOfWorkSubstitute.Received(1).SaveChanges();
    }
}
