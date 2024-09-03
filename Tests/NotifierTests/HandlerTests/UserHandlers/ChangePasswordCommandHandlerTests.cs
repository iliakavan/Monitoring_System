using notifier.Application.Authentication.Command.ChangePassword;

namespace NotifierTests.HandlerTests.UserHandlers;


public class ChangePasswordCommandHandlerTests
{
    private readonly IUnitsOfWorks _mockUow;
    private readonly ChangePasswordCommandHandler _handler;

    public ChangePasswordCommandHandlerTests()
    {
        _mockUow = Substitute.For<IUnitsOfWorks>();
        _handler = new ChangePasswordCommandHandler(_mockUow);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = new ChangePasswordCommandRequest
        {
            UsernameOrEmail = "johndoe@example.com",
            CurrentPassword = "wrongpassword",
            newPassword = "newpassword"
        };

        _mockUow.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword).Returns((Users?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        _mockUow.UserRepo.DidNotReceive().Update(Arg.Any<Users>());
        await _mockUow.DidNotReceive().SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new Users { UserName = "johndoe", Password = "hashedpassword" };
        var request = new ChangePasswordCommandRequest
        {
            UsernameOrEmail = "johndoe@example.com",
            CurrentPassword = "currentpassword",
            newPassword = "newpassword"
        };

        _mockUow.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        user.Password.Should().Be(request.newPassword); // Password should be updated

        _mockUow.UserRepo.Received(1).Update(user);
        await _mockUow.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldUpdatePassword_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new Users { UserName = "johndoe", Password = "hashedpassword" };
        var request = new ChangePasswordCommandRequest
        {
            UsernameOrEmail = "johndoe@example.com",
            CurrentPassword = "currentpassword",
            newPassword = "newpassword"
        };

        _mockUow.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        user.Password.Should().Be(request.newPassword); // Verify that the password was updated

        _mockUow.UserRepo.Received(1).Update(user);
        await _mockUow.Received(1).SaveChanges();
    }
}
