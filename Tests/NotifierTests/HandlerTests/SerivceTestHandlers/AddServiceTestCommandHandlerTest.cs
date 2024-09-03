using notifier.Application.ServiceTests.Command.AddServiceTest;

namespace NotifierTests.HandlerTests.SerivceTestHandlers;



public class AddServiceTestCommandHandlerTest
{
    private readonly IUnitsOfWorks _mockUnitOfWork;
    private readonly AddServiceTestCommandHandler _handler;

    public AddServiceTestCommandHandlerTest()
    {
        _mockUnitOfWork = Substitute.For<IUnitsOfWorks>();
        _handler = new AddServiceTestCommandHandler(_mockUnitOfWork);
    }

    [Fact]
    public async Task Handle_ServiceDoesNotExist_ReturnsErrorMessage()
    {
        // Arrange
        var request = new AddServiceTestCommandRequest { ServiceId = 1 };
        _mockUnitOfWork.ServiceRepo.GetById(request.ServiceId).Returns(Task.FromResult<Service>(null));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"Service with Id of {request.ServiceId} does not exist.");
    }

    [Fact]
    public async Task Handle_ServiceExists_AddsServiceTestAndSavesChanges()
    {
        // Arrange
        var service = new Service
        {
            Title = "Test Service",
            Url = "http://example.com",
            Ip = "192.168.1.1",
            Port = 80,
            Method = "GET"
        };
        var request = new AddServiceTestCommandRequest
        {
            ServiceId = service.Id,
            PriodTime = 30,
            TestType = notifier.Domain.Enum.TestType.Curl
        };
        _mockUnitOfWork.ServiceRepo.GetById(request.ServiceId).Returns(Task.FromResult(service));
        var serviceTestRepo = _mockUnitOfWork.ServiceTestRepo;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await serviceTestRepo.Received(1).Add(Arg.Is<ServiceTest>(st =>
            st.ServiceId == request.ServiceId &&
            st.PriodTime == request.PriodTime &&
            st.TestType == request.TestType &&
            st.RecordDate <= DateTime.UtcNow // Ensure the date is correctly set
        ));
        await _mockUnitOfWork.Received(1).SaveChanges();

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Test Added Successfully.");
    }
}
