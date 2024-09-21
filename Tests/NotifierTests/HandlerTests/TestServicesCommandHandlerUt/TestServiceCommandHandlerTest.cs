using MediatR;
using notifier.Application.BackgroundService;
using notifier.Application.ServiceTests.Command.TestServices;
using notifier.Application.Utils;
using notifier.Domain.Enum;
using NSubstitute;

namespace NotifierTests.HandlerTests.TestServicesCommandHandlerUt;


public class TestServicesCommandHandlerTest
{
    private readonly IUnitsOfWorks _uow;
    private readonly ISendRequestHelper _reqHelper;
    private readonly ITelegramService _telService;
    private readonly TestServicesCommandHandler _handler;

    public TestServicesCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitsOfWorks>();
        _reqHelper = Substitute.For<ISendRequestHelper>();
        _telService = Substitute.For<ITelegramService>();
        _handler = new TestServicesCommandHandler(_uow, _reqHelper, _telService);
    }

    [Fact]
    public async Task Handle_Should_Send_PingRequest_And_Notification_On_Failure()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 0,
                ErrorRetryCount = 0,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Ping,
                    Service = new Service
                    {
                        Title = "Ping Test Service",
                        Ip = "127.0.0.1"
                    },
                    LastStatus = LastStatus.Success
                },
                MessageFormat = "ghjkl;",
                MessageSuccess = "cvbnm,."
                
            }
        };

        var pingResult = new ResultResponse<PingDto>
        {
            Success = false,
            Message = "Ping failed",
            Value = new PingDto { SuccessPercent = 50, RoundTriptime = 100.ToString() }
        };

        var telegramUsers = new List<string> { "user1", "user2" };

        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3));
        _reqHelper.PingRequestAsync(Arg.Any<string>()).Returns(Task.FromResult(pingResult));
        _uow.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>()).Returns(await Task.FromResult(telegramUsers));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        _telService.Received(telegramUsers.Count)?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
        _uow.NotificationRepo.Received().Update(Arg.Any<ServiceNotfications>());
    }

    [Fact]
    public async Task Handle_Should_Not_Send_Notification_If_Service_Is_Successful()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 0,
                ErrorRetryCount = 0,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Ping,
                    Service = new Service
                    {
                        Title = "Ping Test Service",
                        Ip = "127.0.0.1"
                    },
                    LastStatus = LastStatus.Error
                },
                MessageFormat = "ghjkl;",
                MessageSuccess = "cvbnm,."
            }
        };

        var pingResult = new ResultResponse<PingDto>
        {
            Success = true,
            Message = "Ping succeeded",
            Value = new PingDto { SuccessPercent = 100, RoundTriptime = 50.ToString() }
        };

        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3));
        _reqHelper.PingRequestAsync(Arg.Any<string>()).Returns(Task.FromResult(pingResult));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        _telService.DidNotReceive()?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_Should_Send_TelNetRequest_And_Notification_On_Failure()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 0,
                ErrorRetryCount = 0,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.TelNet,
                    Service = new Service
                    {
                        Title = "TelNet Service",
                        Ip = "192.168.1.1",  // Ensure this is not null
                        Port = 8080          // Ensure this is not null
                    },
                    LastStatus = LastStatus.Success,
                },
                MessageFormat = "TelNet Test failed",
                MessageSuccess = "OK"
            }
        };

        // Check for null values in the objects
        serviceTests.Should().NotBeNull();
        serviceTests[0].ServiceTest.Should().NotBeNull();
        serviceTests[0].ServiceTest.Service.Should().NotBeNull();
        serviceTests[0].ServiceTest.Service.Ip.Should().NotBeNull();
        serviceTests[0].ServiceTest.Service.Port.Should().NotBeNull();

        var telNetResult = new ResultResponse
        {
            Success = false,
            Message = "TelNet connection failed"
        };

        var telegramUsers = new List<string> { "user1", "user2" };

        // Mock the dependencies
        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3));
        _reqHelper.ConnectToServerAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(telNetResult));
        _uow.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>()).Returns(await Task.FromResult(telegramUsers));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();  // Verify the result
        _telService.Received(telegramUsers.Count)?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
        _uow.NotificationRepo.Received().Update(Arg.Any<ServiceNotfications>());
    }

    [Fact]
    public async Task Handle_Should_Send_CurlRequest_And_Notification_On_Failure()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 0,
                ErrorRetryCount = 0,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Curl,
                    Service = new Service
                    {
                        Title = "HTTP Service",
                        Url = "https://example.com" // Ensure this is not null
                    },
                    LastStatus = LastStatus.Success,
                },
                MessageFormat = "Test failed",
                MessageSuccess = "ERTYUIOP"

            }
        };

        // Adding assertions to check for nulls
        serviceTests.Should().NotBeNull();
        serviceTests[0].ServiceTest.Should().NotBeNull();
        serviceTests[0].ServiceTest.Service.Should().NotBeNull();
        serviceTests[0].ServiceTest.Service.Url.Should().NotBeNull();

        var curlResult = new ResultResponse
        {
            Success = false,
            Message = "HTTP request failed"
        };

        var telegramUsers = new List<string> { "user1", "user2" };

        // Mocking dependencies
        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3));
        _reqHelper.MakeHttpRequestAsync(Arg.Any<string>()).Returns(Task.FromResult(curlResult));
        _uow.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>()).Returns(await Task.FromResult(telegramUsers));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull(); // Ensure result is not null
        result.Success.Should().BeTrue();
        _telService.Received(telegramUsers.Count)?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
        _uow.NotificationRepo.Received().Update(Arg.Any<ServiceNotfications>());
    }

    [Fact]
    public async Task Handle_Should_Send_Final_Notification_When_Max_Retries_Are_Reached()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 2, // One less than max retries
                ErrorRetryCount = 2,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Ping,
                    Service = new Service
                    {
                        Title = "Ping Test Service",
                        Ip = "127.0.0.1"
                    },
                    LastStatus = LastStatus.Error,
                },
                    MessageFormat = "Service failed",
                    MessageSuccess = "Ok"
            }
        };

        var pingResult = new ResultResponse<PingDto>
        {
            Success = false,
            Message = "Ping failed",
            Value = new PingDto { SuccessPercent = 50, RoundTriptime = 100.ToString() }
        };

        var telegramUsers = new List<string> { "user1", "user2" };

        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3)); // Max retries set to 3
        _reqHelper.PingRequestAsync(Arg.Any<string>()).Returns(Task.FromResult(pingResult));
        _uow.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>()).Returns(await Task.FromResult(telegramUsers));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        _telService.Received(telegramUsers.Count)?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
        _uow.NotificationRepo.Received().Update(Arg.Any<ServiceNotfications>());
        serviceTests[0].RetryCount.Should().Be(0); // Retry count should be reset after max attempts
    }

    [Fact]
    public async Task Handle_Should_Send_Success_Notification_After_Previous_Failure()
    {
        // Arrange
        var serviceTests = new List<ServiceNotfications>()
        {
            new ServiceNotfications
            {
                Id = 1,
                RetryCount = 0,
                ErrorRetryCount = 1,
                ServiceTest = new ServiceTest
                {
                    TestType = TestType.Ping,
                    Service = new Service
                    {
                        Title = "Ping Test Service",
                        Ip = "127.0.0.1"
                    },
                    LastStatus = LastStatus.Success,
                },
                MessageFormat = "Service was down",
                MessageSuccess = "ok"
            }
        };

        var pingResultFailure = new ResultResponse<PingDto>
        {
            Success = false,
            Message = "Ping request failed"
        };

        var pingResultSuccess = new ResultResponse<PingDto>
        {
            Success = true,
            Message = "Ping request succeeded",
            Value = new PingDto { RoundTriptime = 10.ToString(), SuccessPercent = 100 }
        };

        var telegramUsers = new List<string> { "user1", "user2" };

        // Mock the necessary repository and helper responses
        _uow.NotificationRepo.GetAllServices().Returns(Task.FromResult((List<ServiceNotfications>)serviceTests));
        _uow.NotificationRepo.GetMaxRetryCount(1).Returns(Task.FromResult(3));

        // First return a failure result, then return a success result in the same test
        _reqHelper.PingRequestAsync(Arg.Any<string>()).Returns(Task.FromResult(pingResultSuccess)); // Simulating success after previous failure

        _uow.ProjectOffcialRepo.FetchTelegramId(Arg.Any<int>()).Returns(await Task.FromResult(telegramUsers));

        // Act
        var result = await _handler.Handle(new TestServicesCommand(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Expect exactly 2 calls for notifications (one for each user)
        _telService.Received(telegramUsers.Count)?.SendMessage(Arg.Any<string>(), Arg.Any<string>());
    }







}
