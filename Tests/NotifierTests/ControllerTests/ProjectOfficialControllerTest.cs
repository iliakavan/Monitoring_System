using notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;
using notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;
using notifier.Application.ProjectOfficials.Commands.UpdateProjectOffical;

namespace NotifierTests.ControllerTests;





public class ProjectOfficialControllerTest
{
    private readonly ProjectOfficialController _controller;

    private readonly IMediator _mediator;

    public ProjectOfficialControllerTest()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ProjectOfficialController(_mediator);

    }

    [Fact]

    public async Task GetProjectOfficials_ShouldReturnOkWhenProjectOfficialFetchedSuccessfully() 
    {
        // Arrange

        List<ProjectOfficial> P = 
            [
                new() {  Id = 1 , Responsible = "Admin",Mobile = "09119129114",TelegramId = "@qwertyui"},
                new() { Id = 1, Responsible = "Admin", Mobile = "09119129114", TelegramId = "@qwertyuizx" }
            ];
        
        var Response = new ResultResponse<IEnumerable<ProjectOfficial>>() { Success = true ,Value = P };
        _mediator.Send(Arg.Any<GetAllProjectOfficialQueryRequest>()).Returns(Response);

        // Act

        var result = await _controller.GetProjectOfficials();

        //Assert

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]

    public async Task GetProjectOfficials_ShouldReturnNotFoundWhenProjectOfficialFetchFailed() 
    {
        // Arrange
        var Response = new ResultResponse<IEnumerable<ProjectOfficial>>() { Success = false };
        _mediator.Send(Arg.Any<GetAllProjectOfficialQueryRequest>()).Returns(Response);

        // Act 
        var result = await _controller.GetProjectOfficials();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }


    [Fact]
    public async Task GetProjectOfficialById_ShouldReturnOk_WhenProjectIsFound()
    {
        // Arrange
        var request = new GetProjectOfficialByIdQueryRequest
        {
            Id = 1 
        };

        var expectedResponse = new ResultResponse<ProjectOfficialDto> { Success = true };
        _mediator.Send(Arg.Any<GetProjectOfficialByIdQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.GetProjectOfficialById(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetProjectOfficialById_ShouldReturnNotFound_WhenProjectIsNotFound()
    {
        // Arrange
        var request = new GetProjectOfficialByIdQueryRequest
        {
            Id = 999 
        };

        var expectedResponse = new ResultResponse<ProjectOfficialDto> { Success = false };
        _mediator.Send(Arg.Any<GetProjectOfficialByIdQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.GetProjectOfficialById(request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenSearchIsSuccessful()
    {
        // Arrange
        string startDate = "2024-01-01";
        string endDate = "2024-01-31";
        string responsible = "John Doe";
        string mobile = "1234567890";
        string telegramId = "john_doe";
        int? ProjectID = 1;

        var expectedResponse = new ResultResponse<IEnumerable<ProjectOfficialDto>>() { Success = true };
        _mediator.Send(Arg.Any<SearchProjectOfficialQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.Search(startDate, endDate, responsible, mobile, telegramId,ProjectID);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Search_ShouldReturnNotFound_WhenSearchFails()
    {
        // Arrange
        string startDate = "2024-01-01";
        string endDate = "2024-01-31";
        string responsible = "Jane Doe";
        string mobile = "0987654321";
        string telegramId = "jane_doe";
        int? ProjectID = 1;

        var expectedResponse = new ResultResponse<IEnumerable<ProjectOfficialDto>>() { Success = false };
        _mediator.Send(Arg.Any<SearchProjectOfficialQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.Search(startDate, endDate, responsible, mobile, telegramId,ProjectID);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenCalledWithNullParameters()
    {
        // Arrange
        string? startDate = null;
        string? endDate = null;
        string? responsible = null;
        string? mobile = null;
        string? telegramId = null;
        int? ProjectID = 1;

        var expectedResponse = new  ResultResponse<IEnumerable<ProjectOfficialDto>>(){ Success = true };
        _mediator.Send(Arg.Any<SearchProjectOfficialQueryRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.Search(startDate, endDate, responsible, mobile, telegramId,ProjectID);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AddProjectOfficial_ShouldReturnOk_WhenAddingIsSuccessful()
    {
        // Arrange
        var request = new AddProjectOfficialCommandRequest
        {
            Responsible = "Admin",
            Mobile = "09119129144",
            TelegramId = "@qwertyuio"
        };

        var expectedResponse = new ResultResponse { Success = true };
        _mediator.Send(Arg.Any<AddProjectOfficialCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.AddProjectOfficial(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AddProjectOfficial_ShouldReturnBadRequest_WhenAddingFails()
    {
        // Arrange
        var request = new AddProjectOfficialCommandRequest
        {
            Responsible = "Admin",
            Mobile = "09119129144",
            TelegramId = "@qwertyuio"
        };

        var expectedResponse = new ResultResponse() { Success = false };
        _mediator.Send(Arg.Any<AddProjectOfficialCommandRequest>()).Returns(expectedResponse);

        // Act
        var result = await _controller.AddProjectOfficial(request);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]

    public async Task UpdateProjectOfficial_ShouldReturnOk_WhenProjectOfficialUpdatedSuccessfully() 
    {
        // Arrange
        var request = new UpdateProjectOfficialCommandRequest()
        {
            Id = 1,
            Responsible = "Admin",
            Mobile = "09119129144",
            TelegramId = "@qwertyuio"
        };
        var response = new ResultResponse() { Success = true };
        _mediator.Send(Arg.Any<UpdateProjectOfficialCommandRequest>()).Returns(response);
        // Act
        var result = await _controller.UpdateProjectOfficial(request);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]

    public async Task UpdateProjectOfficial_ShouldReturnBadRequest_WhenProjectOfficialUpdateDontUpdateSuccessfully() 
    {
        // Arange
        var request = new UpdateProjectOfficialCommandRequest()
        {
            Id = 1,
            Responsible = "Admin",
            Mobile = "09119129144",
            TelegramId = "@qwertyuio"
        };
        var response = new ResultResponse() { Success = false };
        _mediator.Send(Arg.Any<UpdateProjectOfficialCommandRequest>()).Returns(response);
        // Act
        var result = await _controller.UpdateProjectOfficial(request);
        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]

    public async Task DeleteProjectOfficial_ShouldReturnOk_WhenProjectOfficialDeletedSuccessfully(int ID) 
    {
        // Arange
        var response = new ResultResponse() { Success = true };
        _mediator.Send(Arg.Any<DeleteProjectOfficialCommandRequest>()).Returns(response);
        // Act
        var result = await _controller.DeleteProjectOfficial(ID);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]

    public async Task DeleteProjectOfficial_ShoulReturnNotFounded_WhenProjectOfficialDontDeleteSuccessfully(int ID) 
    {
        // Arange
        var response = new ResultResponse() { Success = false };
        _mediator.Send(Arg.Any<DeleteProjectOfficialCommandRequest>()).Returns(response);
        // Act
        var result = await _controller.DeleteProjectOfficial(ID);
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }



}



