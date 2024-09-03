using notifier.Application.Authentication.Command.ChangePassword;
using notifier.Application.Authentication.Command.UserRegisteration;

namespace NotifierTests.ControllerTests;



public class UserControllerTests
{
    private readonly IMediator _mediator;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new UserController(_mediator);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new UserRegesterationCommandRequest { /* Set request properties here */ };
        var response = new ResultResponse { Success = true };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.RegisterUser(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(response);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnBadRequest_WhenRegistrationFails()
    {
        // Arrange
        var request = new UserRegesterationCommandRequest { /* Set request properties here */ };
        var response = new ResultResponse { Success = false };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.RegisterUser(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult?.Value.Should().Be(response);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnOk_WhenPasswordChangeIsSuccessful()
    {
        // Arrange
        var request = new ChangePasswordCommandRequest { /* Set request properties here */ };
        var response = new ResultResponse { Success = true };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.ChangePassword(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(response);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnUnauthorized_WhenPasswordChangeFails()
    {
        // Arrange
        var request = new ChangePasswordCommandRequest { /* Set request properties here */ };
        var response = new ResultResponse { Success = false };

        _mediator.Send(request).Returns(response);

        // Act
        var result = await _controller.ChangePassword(request);

        // Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

}
