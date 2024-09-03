using notifier.Application.Authentication.Command.UserRegisteration;

namespace NotifierTests.HandlerTests.UserHandlers;


public class UserRegesterationCommandHandlerTests
{
    private readonly IUnitsOfWorks _mockUow;
    private readonly UserRegesterationCommandHandler _handler;

    public UserRegesterationCommandHandlerTests()
    {
        _mockUow = Substitute.For<IUnitsOfWorks>();
        _handler = new UserRegesterationCommandHandler(_mockUow);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenRequestIsNull()
    {
        // Arrange
        UserRegesterationCommandRequest request = null;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenRequestIsValid()
    {
        // Arrange
        var request = new UserRegesterationCommandRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            UserName = "johndoe",
            Password = "securepassword"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _mockUow.UserRepo.Received(1).Register(Arg.Is<Users>(u =>
            u.FirstName == request.FirstName &&
            u.LastName == request.LastName &&
            u.Email == request.Email &&
            u.UserName == request.UserName &&
            !string.IsNullOrEmpty(u.Password) // Ensure password is hashed
        ));

        await _mockUow.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldHashPassword_WhenRequestIsValid()
    {
        // Arrange
        var request = new UserRegesterationCommandRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            UserName = "johndoe",
            Password = "securepassword"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _mockUow.UserRepo.Received(1).Register(Arg.Is<Users>(u =>
            u.Password != request.Password // Ensure the password is not stored in plain text
        ));
    }
}
