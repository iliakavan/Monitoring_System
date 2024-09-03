using notifier.Application.Projects.Commads.UpdateProjectCommand;

namespace NotifierTests.HandlerTests.ProjectHandlers;



public class UpdateProjectHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateProjectCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_NullRequest_ShouldReturnFailureResult()
    {
        // Arrange
        UpdateProjectCommandRequest request = null;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_NonExistentProject_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new UpdateProjectCommandRequest { Id = 1, Title = "New Title", Description = "New Description" };
        _unitOfWorkSubstitute.ProjectRepo.GetById(1).Returns(Task.FromResult<Project>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Project doesnt Exist");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        var project = new Project { Id = 1, Title = "Old Title", Description = "Old Description" };
        var request = new UpdateProjectCommandRequest { Id = 1, Title = "New Title", Description = "New Description" };
        _unitOfWorkSubstitute.ProjectRepo.GetById(1).Returns(Task.FromResult(project));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("1 Updated Successfully");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateProjectInRepository()
    {
        // Arrange
        var project = new Project { Id = 1, Title = "Old Title", Description = "Old Description" };
        var request = new UpdateProjectCommandRequest { Id = 1, Title = "New Title", Description = "New Description" };
        _unitOfWorkSubstitute.ProjectRepo.GetById(1).Returns(Task.FromResult(project));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkSubstitute.ProjectRepo.Received(1).Update(Arg.Is<Project>(p =>
            p.Id == 1 &&
            p.Title == "New Title" &&
            p.Description == "New Description"
        ));
    }
}
