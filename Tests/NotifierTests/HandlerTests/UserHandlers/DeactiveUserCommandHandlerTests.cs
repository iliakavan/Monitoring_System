using notifier.Application.User.Command.DeactiveUser;

namespace NotifierTests.HandlerTests.UserHandlers;



public class DeactiveUserCommandHandlerTests
{
    private readonly IUnitsOfWorks _mockUow;
    private readonly DeactiveUserCommandHandler _handler;

    public DeactiveUserCommandHandlerTests()
    {
        _mockUow = Substitute.For<IUnitsOfWorks>();
        _handler = new DeactiveUserCommandHandler(_mockUow);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserIsNotFound()
    {
        // Arrange
        var request = new DeactiveUserCommandRequest { Id = 1 };

        _mockUow.UserRepo.GetUserByID(request.Id).Returns((Users?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        _mockUow.UserRepo.DidNotReceive().DeactiveUser(Arg.Any<Users>());
        await _mockUow.DidNotReceive().SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUserIsFoundAndDeactivated()
    {
        // Arrange
        var user = new Users { Id = 1, UserName = "johndoe", IsActive = true };
        var request = new DeactiveUserCommandRequest { Id = 1 };

        _mockUow.UserRepo.GetUserByID(request.Id).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        _mockUow.UserRepo.Received(1).DeactiveUser(user);
        await _mockUow.Received(1).SaveChanges();
    }
}
