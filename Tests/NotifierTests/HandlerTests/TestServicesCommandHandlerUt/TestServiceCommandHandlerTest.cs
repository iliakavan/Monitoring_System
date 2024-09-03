using notifier.Application.BackgroundService;
using notifier.Application.ServiceTests.Command.TestServices;
using notifier.Application.Utils;
using notifier.Domain.Enum;
using NSubstitute;

namespace NotifierTests.HandlerTests.TestServicesCommandHandlerUt;


public class TestServicesCommandHandlerTest
{
    private readonly IUnitsOfWorks _uowMock;
    private readonly ISendRequestHelper _reqHelperMock;
    private readonly ITelegramService _telServiceMock;
    private readonly TestServicesCommandHandler _handler;

    public TestServicesCommandHandlerTest()
    {
        _uowMock = Substitute.For<IUnitsOfWorks>();
        _reqHelperMock = Substitute.For<ISendRequestHelper>();
        _telServiceMock = Substitute.For<ITelegramService>();
        _handler = new TestServicesCommandHandler(_uowMock, _reqHelperMock, _telServiceMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAllServicesPass()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>
        {
            new ServiceNotfications
            {
                ServiceTest = new ServiceTest { TestType = TestType.Ping, Service = new Service { Method = "Get",Title = "Test Service 1", Url = "https://www.example.com"} },
                NotificationType = NotificationType.Telegram,
                MessageSuccess = "Ping successful",
                MessageFormat = "Ping failed"
            },
            new ServiceNotfications
            {
                ServiceTest = new ServiceTest { TestType = TestType.Ping, Service = new Service { Method = "Get",Title = "Test Service 1", Url = "https://www.example.com"} },
                NotificationType = NotificationType.Telegram,
                MessageSuccess = "TelNet successful",
                MessageFormat = "Telnet failed"
            },
            new ServiceNotfications
            {
                ServiceTest = new ServiceTest { TestType = TestType.Ping, Service = new Service { Method = "Get",Title = "Test Service 1", Url = "https://www.example.com"} },
                NotificationType = NotificationType.Telegram,
                MessageSuccess = "Http successful",
                MessageFormat = "Http failed"
            },
        };

        _uowMock.NotificationRepo.GetAllServices().Returns(await Task.FromResult(serviceTests));

        // Mocking Ping response
        _reqHelperMock.PingRequestAsync(Arg.Any<string>())
            .Returns(await Task.FromResult(new ResultResponse<PingDto> { Success = true, Value = new PingDto { SuccessPercent = 100 } }));

        // Mocking ConnectToServer response
        _reqHelperMock.ConnectToServerAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(await Task.FromResult(new ResultResponse { Success = true }));

        // Mocking MakeHttpRequestAsync response
        _reqHelperMock.MakeHttpRequestAsync(Arg.Any<string>())
            .Returns(await Task.FromResult(new ResultResponse { Success = true }));

        // Mocking Telegram service
        _telServiceMock.SendMessage(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        _uowMock.NotificationRepo.Received().Update(Arg.Any<ServiceNotfications>());
        await _uowMock.SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldRetryAndLog_WhenServiceFails()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>
        {
            new ServiceNotfications
            {
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Ping,
                    Service = new Service
                    {
                        Ip = "127.0.0.1",
                        Port = 80,
                        Title = "Test Service 1",
                        Method = "GET" // Ensure all used properties are set
                    }
                },
                NotificationType = NotificationType.Telegram,
                MessageFormat = "Ping failed",
                MessageSuccess = "Ping Success",
                RetryCount = 0
            }
        };
        PingDto ping = new()
        { 
            SuccessPercent = 10,
            BufferSize = string.Empty,
            Address = "127.0.0.1",
            DontFragment = string.Empty,
            RoundTriptime = string.Empty,
            Ttl = string.Empty
        };

        // Ensure the Unit of Work mock returns the test data
        _uowMock.NotificationRepo.GetAllServices().Returns(Task.FromResult(serviceTests));

        // Simulate a failed Ping response
        _reqHelperMock.PingRequestAsync(Arg.Any<string>())
            .Returns(Task.FromResult(new ResultResponse<PingDto> { Success = false, Value = ping }));

        // Mock Telegram IDs
        _uowMock.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>())
            .Returns(await Task.FromResult(new List<string> { "123456789" }));

        // Mocking Telegram service - ensure this does not return null
        _telServiceMock.SendMessage(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Ensure retry count is incremented and logs are written
        var serviceNotification = serviceTests.First();
        serviceNotification.RetryCount.Should().Be(1);

        // Verify that the log was inserted and the notification was updated
        await _uowMock.ServiceTestLogRepo.Received(1).Insert(Arg.Any<ServiceTestLog>());
        _uowMock.NotificationRepo.Received(1).Update(serviceNotification);

        // Verify that the SendMessage method was called
        await _telServiceMock.Received(1).SendMessage(Arg.Any<string>(), Arg.Any<string>());

    }

    [Fact]
    public async Task Handle_ShouldSendTelegramNotification_WhenTestFails()
    {

        // Arrange
        var serviceTests = new List<ServiceNotfications>
        {
        new ServiceNotfications
        {
            ServiceTest = new ServiceTest
            {
                TestType = TestType.Ping,
                Service = new Service
                {
                    Method = "Get",
                    Title = "Test Service 1",
                    Url = "https://www.example.com"
                }
            },
            NotificationType = NotificationType.Telegram,
            MessageSuccess = "Ping successful",
            MessageFormat = "Ping failed"
        }
        };

        // Mock ping response to simulate failure
        PingDto ping = new()
        {
            SuccessPercent = 10, // Simulate low success rate to trigger failure
            BufferSize = string.Empty,
            Address = "127.0.0.1",
            DontFragment = string.Empty,
            RoundTriptime = string.Empty,
            Ttl = string.Empty
        };

        _uowMock.NotificationRepo.GetAllServices().Returns(Task.FromResult(serviceTests));

        // Mocking failed Ping request response
        _reqHelperMock.PingRequestAsync(Arg.Any<string>())
            .Returns(Task.FromResult(new ResultResponse<PingDto>
            {
                Success = false,
                Message = "Ping failed",
                Value = ping
            }));

        // Mock Telegram IDs
        _uowMock.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>())
            .Returns(await Task.FromResult(new List<string> { "123456789" }));

        // Mocking the SendMessage to ensure it will be called
        _telServiceMock.SendMessage(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that the Telegram message was sent
        await _telServiceMock.Received(1).SendMessage("123456789", Arg.Any<string>());
    }
}
