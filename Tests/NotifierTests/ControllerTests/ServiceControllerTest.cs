using notifier.Application.Services.Queries.GetServiceById;

namespace NotifierTests.ControllerTests;


public class ServiceControllerTest
{
    private readonly IMediator _mediator;

    private readonly ServiceController _controller;

    public ServiceControllerTest()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ServiceController(_mediator);
    }

    [Fact]

    public async Task GetServiceById_ShouldReturnOk_WhenServiceFetchedFromDatabaseSuccessfully() 
    { 
        // Arange
        var request = new GetServiceByIdQueryRequest() { Id = 1 };
        var Response = new ResultResponse<ServiceDto>() { Success = true };
        _mediator.Send(Arg.Any<GetServiceByIdQueryRequest>()).Returns(Response);
        // Act
        var result = await _controller.GetServiceById(request);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

}
