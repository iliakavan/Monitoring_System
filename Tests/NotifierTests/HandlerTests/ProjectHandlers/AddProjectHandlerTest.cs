namespace NotifierTests.HandlerTests.ProjectHandlers;




public class AddProjectHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly AddProjectCommandHandler _handler;

    public AddProjectHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new AddProjectCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_NullRequest_ShouldReturnFailureResult()
    {
        // Arrange
        AddProjectCommandRequest request = null;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        var request = new AddProjectCommandRequest
        {
            Title = "Test Project",
            Description = "Test Description"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Project Created");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldAddProjectToRepository()
    {
        // Arrange
        var request = new AddProjectCommandRequest
        {
            Title = "Test Project",
            Description = "Test Description"
        };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _unitOfWorkSubstitute.ProjectRepo.Received(1).Add(Arg.Is<Project>(p =>
            p.Title == request.Title &&
            p.Description == request.Description &&
            p.RecordDate != default
        ));
        await _unitOfWorkSubstitute.Received(1).SaveChanges();
    }

}
