using notifier.Application.User.Command.DeactiveUser;
using notifier.Application.User.Query.Search;

namespace NotifierTests.ControllerTests;



public class AdminUserControllerTests
{
    private readonly IMediator _mediator;
    private readonly AdminUserController _controller;

    public AdminUserControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new AdminUserController(_mediator);
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenUsersAreFound()
    {
        // Arrange
        var request = new SearchUserQueryRequest { /* Set request properties here */ };
        var response = new ResultResponse<IEnumerable<UserDto>> { Success = true, Value = new List<UserDto> { new UserDto() } };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.Search(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(response);
    }

    [Fact]
    public async Task Search_ShouldReturnNotFound_WhenNoUsersAreFound()
    {
        // Arrange
        var request = new SearchUserQueryRequest { /* Set request properties here */ };
        var response = new ResultResponse<IEnumerable<UserDto>> { Success = false, Value = null! };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.Search(request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeactiveUser_ShouldReturnOk_WhenDeactivationIsSuccessful()
    {
        // Arrange
        var id = 1;
        var response = new ResultResponse { Success = true };

        _mediator.Send(Arg.Any<DeactiveUserCommandRequest>()).Returns(response);

        // Act
        var result = await _controller.DeactiveUser(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeactiveUser_ShouldReturnBadRequest_WhenDeactivationFails()
    {
        // Arrange
        var id = 1;
        var response = new ResultResponse { Success = false };

        _mediator.Send(Arg.Any<DeactiveUserCommandRequest>()).Returns(response);

        // Act
        var result = await _controller.DeactiveUser(id);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
