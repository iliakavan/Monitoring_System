using BCrypt.Net;
using notifier.Application.Authentication.Command.ChangePassword;
using Telegram.Bot.Types;

namespace NotifierTests.HandlerTests.UserHandlers;


public class ChangePasswordCommandHandlerTests
{
    private readonly IUnitsOfWorks _Uow;
    private readonly ChangePasswordCommandHandler _handler;

    public ChangePasswordCommandHandlerTests()
    {
        _Uow = Substitute.For<IUnitsOfWorks>();
        _handler = new ChangePasswordCommandHandler(_Uow);

    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessFalse_WhenUserIsNotFound()
    {
        // Arrange
        var request = new ChangePasswordCommandRequest
        {
            UsernameOrEmail = "test@example.com",
            CurrentPassword = "currentPassword",
            newPassword = "newPassword"
        };

        _Uow.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword).Returns((Users)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessTrue_WhenPasswordIsSuccessfullyChanged()
    {
        // Arrange
        var user = new Users
        {
            UserName = "testuser",
            Password = "oldHashedPassword"
        };

        var request = new ChangePasswordCommandRequest
        {
            UsernameOrEmail = "test@example.com",
            CurrentPassword = "currentPassword",
            newPassword = "newPassword"
        };

        _Uow.UserRepo.Authenticate(request.UsernameOrEmail, request.CurrentPassword).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        await _Uow.Received(1).SaveChanges(); // Ensure SaveChanges is called once
        user.Password.Should().NotBe("oldHashedPassword"); // Ensure password is updated
    }
}
