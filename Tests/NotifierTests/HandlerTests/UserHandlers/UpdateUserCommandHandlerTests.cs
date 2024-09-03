using notifier.Application.User.Command.UpdateUser;

namespace NotifierTests.HandlerTests.UserHandlers;


public class UpdateUserCommandHandlerTests
{
    private readonly IUnitsOfWorks _mockUow;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _mockUow = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateUserCommandHandler(_mockUow);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserIsNotFound()
    {
        // Arrange
        var request = new UpdateUserCommandRequest { Id = 1 };

        _mockUow.UserRepo.GetUserByID(request.Id).Returns((Users?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();

        _mockUow.UserRepo.DidNotReceive().Update(Arg.Any<Users>());
        await _mockUow.DidNotReceive().SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldUpdateUserSuccessfully_WhenUserIsFound()
    {
        // Arrange
        var user = new Users { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
        var request = new UpdateUserCommandRequest
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "0987654321"
        };

        _mockUow.UserRepo.GetUserByID(request.Id).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        user.FirstName.Should().Be(request.FirstName);
        user.LastName.Should().Be(request.LastName);
        user.Email.Should().Be(request.Email);
        user.PhoneNumber.Should().Be(request.PhoneNumber);

        _mockUow.UserRepo.Received(1).Update(user);
        await _mockUow.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ShouldApplyPartialUpdate_WhenSomeFieldsAreNull()
    {
        // Arrange
        var user = new Users { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
        var request = new UpdateUserCommandRequest
        {
            Id = 1,
            FirstName = "Jane",
            LastName = null, // No update
            Email = null,    // No update
            PhoneNumber = "0987654321"
        };

        _mockUow.UserRepo.GetUserByID(request.Id).Returns(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        user.FirstName.Should().Be(request.FirstName);      // Updated
        user.LastName.Should().Be("Doe");                  // Not updated
        user.Email.Should().Be("john.doe@example.com");    // Not updated
        user.PhoneNumber.Should().Be(request.PhoneNumber); // Updated

        _mockUow.UserRepo.Received(1).Update(user);
        await _mockUow.Received(1).SaveChanges();
    }
}
