using notifier.Application.ServiceTests.Command.DeleteServiceTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifierTests.HandlerTests.SerivceTestHandlers;

public class DeleteServiceTestCommandHandlerTests
{
    private readonly IUnitsOfWorks _uow;
    private readonly DeleteServiceTestCommandHandler _handler;

    public DeleteServiceTestCommandHandlerTests()
    {
        _uow = Substitute.For<IUnitsOfWorks>();
        _handler = new DeleteServiceTestCommandHandler(_uow);
    }

    [Fact]
    public async Task Handle_Should_Delete_ServiceTest_When_It_Exists()
    {
        // Arrange
        var serviceTestId = 1;
        var serviceTest = new ServiceTest { Id = serviceTestId }; // Assuming ServiceTest is an entity with an Id property

        _uow.ServiceTestRepo.GetByIdincludeAll(serviceTestId)!.Returns(Task.FromResult(serviceTest));

        var command = new DeleteServiceTestCommandRequest { Id = serviceTestId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().BeNullOrEmpty();

        // Verify Delete and SaveChanges were called
        _uow.ServiceTestRepo.Received(1).Delete(serviceTest);
        await _uow.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_ServiceTest_Does_Not_Exist()
    {
        // Arrange
        var serviceTestId = 1;

        // Simulate the case where the service test is not found
        _uow.ServiceTestRepo.GetByIdincludeAll(serviceTestId)!.Returns(Task.FromResult<ServiceTest>(null!));

        var command = new DeleteServiceTestCommandRequest { Id = serviceTestId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Service Test Doesnt Exist !!");

        // Verify Delete and SaveChanges were not called
        _uow.ServiceTestRepo.DidNotReceive().Delete(Arg.Any<ServiceTest>());
        await _uow.DidNotReceive().SaveChanges();
    }
}
