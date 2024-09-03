using notifier.Application.ServiceTests.Command.AddServiceTest;
using notifier.Application.ServiceTests.Command.DeleteServiceTest;
using notifier.Application.ServiceTests.Command.TestServices;
using notifier.Application.ServiceTests.Command.UpdateServiceTest;
using notifier.Application.ServiceTests.Queries.GetAllServiceTest;
using notifier.Application.ServiceTests.Queries.GetServiceTestById;
using notifier.Application.ServiceTests.Queries.Search;

namespace NotifierTests.ControllerTests;



public class ServiceTestControllerTest
{


    private readonly IMediator _mediator;
    private readonly ServiceTestController _controller;

    public ServiceTestControllerTest()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ServiceTestController(_mediator);
    }

    [Fact]
    public async Task GetServiceTests_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var list = new List<ServiceTest>() { new ServiceTest() { } };
        var queryResult = new ResultResponse<IEnumerable<ServiceTest>> { Success = true , Value = list};
        _mediator.Send(Arg.Any<GetAllServiceTestQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.GetServiceTests();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(queryResult);
    }

    [Fact]
    public async Task GetServiceTests_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var queryResult = new ResultResponse<IEnumerable<ServiceTest>> { Success = false };
        _mediator.Send(Arg.Any<GetAllServiceTestQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.GetServiceTests();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetServiceTestById_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        ServiceTest test = new ServiceTest();
        var queryResult = new ResultResponse<ServiceTest> { Success = true , Value = test };
        _mediator.Send(Arg.Any<GetServiceTestByIdQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.GetServiceTestById(new GetServiceTestByIdQueryRequest());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(queryResult);
    }

    [Fact]
    public async Task GetServiceTestById_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var queryResult = new ResultResponse<ServiceTest> { Success = false };
        _mediator.Send(Arg.Any<GetServiceTestByIdQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.GetServiceTestById(new GetServiceTestByIdQueryRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Test_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse() { Success = true };
        _mediator.Send(Arg.Any<TestServicesCommand>()).Returns(commandResult);

        // Act
        var result = await _controller.Test();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(commandResult);
    }

    [Fact]
    public async Task Test_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<TestServicesCommand>()).Returns(commandResult);

        // Act
        var result = await _controller.Test();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var list = new List<ServiceTestDto>() { new ServiceTestDto() { } };


        var queryResult = new ResultResponse<IEnumerable<ServiceTestDto>> { Success = true , Value = list };
        _mediator.Send(Arg.Any<SearchServiceTestQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.Search("2023-01-01", "2023-01-31", 1, 2);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(queryResult);
    }

    [Fact]
    public async Task Search_ShouldReturnNoContent_WhenResultIsNotSuccessful()
    {
        // Arrange
        var queryResult = new ResultResponse<IEnumerable<ServiceTestDto>> { Success = false };
        _mediator.Send(Arg.Any<SearchServiceTestQueryRequest>()).Returns(queryResult);

        // Act
        var result = await _controller.Search("2023-01-01", "2023-01-31", 1,2);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task AddServiceTest_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = true };
        _mediator.Send(Arg.Any<AddServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.AddServiceTest(new AddServiceTestCommandRequest());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(commandResult);
    }

    [Fact]
    public async Task AddServiceTest_ShouldReturnBadRequest_WhenResultIsNotSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<AddServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.AddServiceTest(new AddServiceTestCommandRequest());

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task UpdateServiceTest_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = true };
        _mediator.Send(Arg.Any<UpdateServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.UpdateServiceTest(new UpdateServiceTestCommandRequest());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(commandResult);
    }

    [Fact]
    public async Task UpdateServiceTest_ShouldReturnBadRequest_WhenResultIsNotSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<UpdateServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.UpdateServiceTest(new UpdateServiceTestCommandRequest());

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteServiceTest_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse{ Success = true };
        _mediator.Send(Arg.Any<DeleteServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.DeleteServiceTest(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(commandResult);
    }

    [Fact]
    public async Task DeleteServiceTest_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var commandResult = new ResultResponse { Success = false };
        _mediator.Send(Arg.Any<DeleteServiceTestCommandRequest>()).Returns(commandResult);

        // Act
        var result = await _controller.DeleteServiceTest(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
