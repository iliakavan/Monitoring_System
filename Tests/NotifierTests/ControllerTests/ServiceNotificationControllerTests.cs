
using notifier.Application.ServiceNotifications.Commands.AddServiceNotification;
using notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;
using notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;
using notifier.Application.ServiceNotifications.Queries.GetAllServiceNotification;
using notifier.Application.ServiceNotifications.Queries.GetServiceNotification;
using notifier.Application.ServiceNotifications.Queries.Search;
using notifier.Domain.Enum;
using NSubstitute;

namespace NotifierTests.ControllerTests;

public class ServiceNotificationControllerTests
{
    private readonly ServiceNotificationController _controller;
    private readonly IMediator _mediator;

    public ServiceNotificationControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ServiceNotificationController(_mediator);
    }

    [Fact]
    public async Task GetServiceNotification_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var request = new GetServiceNotificationByIdQueryRequest { /* set properties if needed */ };
        var result = new ResultResponse<ServiceNotificationDto> { Success = true, Value = new ServiceNotificationDto() };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.GetServiceNotification(request) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);
        response?.Value.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task GetServiceNotification_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var request = new GetServiceNotificationByIdQueryRequest { /* set properties if needed */ };
        var result = new ResultResponse<ServiceNotificationDto> { Success = false };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.GetServiceNotification(request) as NotFoundResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAllServiceNotification_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var result = new ResultResponse<IEnumerable<ServiceNotfications>> { Success = true, Value = new List<ServiceNotfications>() };

        _mediator.Send(Arg.Any<GetAllServiceNotificationQueryRequest>()).Returns(result);

        // Act
        var response = await _controller.GetAllServiceNotification() as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);
        response?.Value.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task GetAllServiceNotification_ShouldReturnNoContent_WhenResultIsNotSuccessful()
    {
        // Arrange
        var result = new ResultResponse<IEnumerable<ServiceNotfications>> { Success = false };

        _mediator.Send(Arg.Any<GetAllServiceNotificationQueryRequest>()).Returns(result);

        // Act
        var response = await _controller.GetAllServiceNotification() as NoContentResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task Search_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var request = new SearchServiceNotificationQueryRequest
        {
            // Set up valid properties for the request if necessary
        };

        var serviceNotificationList = new List<ServiceNotificationDto>
        {
            new ServiceNotificationDto
            {
                Id = 1,
                MessageFormat = "Test message",
                MessageSuccess = "cvbnm,.",
                NotificationType = NotificationType.Telegram.ToString(),
                ServiceTestId = 1,
                ServiceName = "nm,.",
                ProjectName = "cvbnm,"
            }
        };

        // Create a successful result response
        var result = new ResultResponse<IEnumerable<ServiceNotificationDto>>
        {
            Success = true,
            Value = serviceNotificationList
        };

        // Mock the mediator to return the successful result
        _mediator.Send(Arg.Any<SearchServiceNotificationQueryRequest>())!.Returns(result);

        // Act
        var response = await _controller.Search("2024-01-01", "2024-01-31", NotificationType.Telegram, 1, 1, 1) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);

        var returnedValue = response?.Value as ResultResponse<IEnumerable<ServiceNotificationDto>>;
        returnedValue.Should().NotBeNull();
        returnedValue?.Success.Should().BeTrue();
        returnedValue?.Value.Should().HaveCount(1);
    }
    [Fact]
    public async Task Search_ShouldReturnNoContent_WhenResultIsNotSuccessful()
    {
        // Arrange
        var request = new SearchServiceNotificationQueryRequest
        {
            // You can set some valid properties here if needed
        };

        // Create a valid response with `Success = false` to simulate no content.
        var result = new ResultResponse<IEnumerable<ServiceNotificationDto>> { Success = false, Value = null! };

        // Mock mediator to return the result
        _mediator.Send(Arg.Any<SearchServiceNotificationQueryRequest>())!.Returns(result);

        // Act
        var response = await _controller.Search("2024-01-01", "2024-01-31", NotificationType.Telegram, 1, 1, 1) as NoContentResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task AddServiceNotification_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var request = new AddServiceNotificationCommandRequest { MessageFormat = "dfghjkl;'", MessageSuccess = "fsghjkl" };
        var result = new ResultResponse { Success = true };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.AddServiceNotification(request) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);
        response?.Value.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task AddServiceNotification_ShouldReturnBadRequest_WhenResultIsNotSuccessful()
    {
        // Arrange
        var request = new AddServiceNotificationCommandRequest { MessageFormat = "dfghjkl;'",MessageSuccess = "fsghjkl"};
        var result = new ResultResponse { Success = false };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.AddServiceNotification(request) as BadRequestResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateServiceNotification_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var request = new UpdateServiceNotificationCommandRequest { /* set properties if needed */ };
        var result = new ResultResponse { Success = true };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.UpdateServiceNotification(request) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);
        response?.Value.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task UpdateServiceNotification_ShouldReturnBadRequest_WhenResultIsNotSuccessful()
    {
        // Arrange
        var request = new UpdateServiceNotificationCommandRequest { /* set properties if needed */ };
        var result = new ResultResponse { Success = false };

        _mediator.Send(request).Returns(result);

        // Act
        var response = await _controller.UpdateServiceNotification(request) as BadRequestResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteServiceNotification_ShouldReturnOk_WhenResultIsSuccessful()
    {
        // Arrange
        var id = 1;
        var result = new ResultResponse { Success = true };

        _mediator.Send(Arg.Is<DeleteServiceNotificationCommandRequest>(r => r.Id == id)).Returns(result);

        // Act
        var response = await _controller.DeleteServiceNotification(id) as OkObjectResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(200);
        response?.Value.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task DeleteServiceNotification_ShouldReturnNotFound_WhenResultIsNotSuccessful()
    {
        // Arrange
        var id = 1;
        var result = new ResultResponse { Success = false };

        _mediator.Send(Arg.Is<DeleteServiceNotificationCommandRequest>(r => r.Id == id)).Returns(result);

        // Act
        var response = await _controller.DeleteServiceNotification(id) as NotFoundResult;

        // Assert
        response.Should().NotBeNull();
        response?.StatusCode.Should().Be(404);
    }
}
