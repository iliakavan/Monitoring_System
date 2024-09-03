using notifier.Application.Projects.Queries.Search;

namespace NotifierTests.ControllerTests;





public class ProjectControllerTests
{
    private readonly ProjectController _controller;
    private readonly IMediator _mediator;

    public ProjectControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ProjectController(_mediator);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturnOk_WhenProjectIsFound()
    {
        // Arrange
        var projectId = 1;
        var expectedResponse = new ResultResponse<ProjectDto> { Success = true };
        _mediator.Send(Arg.Any<GetProjectByIdQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.GetProjectById(projectId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturnNotFound_WhenProjectIsNotFound()
    {
        // Arrange
        var projectId = 1;
        var expectedResponse = new ResultResponse<ProjectDto> { Success = false };
        _mediator.Send(Arg.Any<GetProjectByIdQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.GetProjectById(projectId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task AddProject_ShouldReturnOk_WhenProjectIsAddedSuccessfully()
    {
        // Arrange
        var request = new AddProjectCommandRequest() { Title = "ndsksdk"};
        var expectedResponse = new ResultResponse { Success = true };
        _mediator.Send(Arg.Any<AddProjectCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.AddProject(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AddProject_ShouldReturnBadRequest_WhenAddingProjectFails()
    {
        // Arrange
        var request = new AddProjectCommandRequest() { Title = "ndsksdk" };
        var expectedResponse = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<AddProjectCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.AddProject(request);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnOk_WhenProjectIsUpdatedSuccessfully()
    {
        // Arrange
        var request = new UpdateProjectCommandRequest() { Title = "ndsksdk" };
        var expectedResponse = new ResultResponse { Success = true };
        _mediator.Send(Arg.Any<UpdateProjectCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.UpdateProject(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnBadRequest_WhenUpdatingProjectFails()
    {
        // Arrange
        var request = new UpdateProjectCommandRequest() { Title = "ndsksdk" };
        var expectedResponse = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<UpdateProjectCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.UpdateProject(request);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]

    public async Task DeleteProject_ShouldReturnOkProjectDeletedSuccessfully(int p) 
    {
        // Arrange
        
        var expectedResponse = new ResultResponse() { Success = true };
        _mediator.Send(Arg.Any<DeleteProjectCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.DeleteProject(p);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]

    public async Task DeleteProject_ShouldReturnNotFoundWhenDeletingProjectFails(int p) 
    {
        // Arrange
        var expectedResponse = new ResultResponse() { Success = false};
        _mediator.Send(Arg.Any<DeleteProjectCommandRequest>()).Returns(expectedResponse);

        // Act

        var result = await _controller.DeleteProject(p);

        // Assert

        result.Should().BeOfType<NotFoundResult>();

    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWhenGetAllProjectFromDataBaseSuccessfully() 
    {
        // Arrange
        List<Project> p = 
            [
                new Project() { Title = "title1"} 
            ];
        var expectedResponse = new ResultResponse<IEnumerable<Project>>() { Success = true, Value = p };
        _mediator?.Send(Arg.Any<GetAllProjectQueryRequest>())!.Returns(expectedResponse);

        // Act

        var result = await _controller.GetAll();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenSearchIsSuccessful()
    {
        // Arrange
        string startDate = "2024-01-01";
        string endDate = "2024-01-31";
        string title = "Sample Project";

        var expectedResponse = new ResultResponse<IEnumerable<Project>> { Success = true };
        _mediator.Send(Arg.Any<SearchProjectQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.Search(startDate, endDate, title);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Search_ShouldReturnNoContent_WhenSearchFails()
    {
        // Arrange
        string startDate = "2024-01-01";
        string endDate = "2024-01-31";
        string title = "Nonexistent Project";

        var expectedResponse = new ResultResponse<IEnumerable<Project>>() { Success = false };
        _mediator.Send(Arg.Any<SearchProjectQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.Search(startDate, endDate, title);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}