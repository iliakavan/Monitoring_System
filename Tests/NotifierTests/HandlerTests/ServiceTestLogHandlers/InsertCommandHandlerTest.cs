using notifier.Application.SeriveTestLogs.Command;
using NSubstitute.ExceptionExtensions;

namespace NotifierTests.HandlerTests.ServiceTestLogHandlers;


public class InsertCommandHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly InsertCommandHandler _handler;

    public InsertCommandHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new InsertCommandHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenInsertIsSuccessful()
    {
        // Arrange
        var request = new InsertCommandRequest
        {
            ResponseCode = "200",
            ServiceId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        // Verify that the insert method was called with the correct parameters
        await _unitsOfWorks.ServiceTestLogRepo.Received(1).Insert(Arg.Is<ServiceTestLog>(log =>
            log.ResponseCode == request.ResponseCode &&
            log.ServiceId == request.ServiceId &&
            log.RecordDate.Date == DateTime.Now.Date // Check only the date part
        ));

        // Verify that SaveChanges was called
        await _unitsOfWorks.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenInsertFails()
    {
        // Arrange
        var request = new InsertCommandRequest
        {
            ResponseCode = "500",
            ServiceId = 2
        };

        _unitsOfWorks.ServiceTestLogRepo.Insert(Arg.Any<ServiceTestLog>()).Throws(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");

        // Verify that SaveChanges was not called
        await _unitsOfWorks.DidNotReceive().SaveChanges();
    }
}
