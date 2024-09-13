using notifier.Application.User.Query.Search;
using notifier.Domain.Enum;

namespace NotifierTests.HandlerTests.UserHandlers;


public class SearchUserQueryHandlerTests
{
    private readonly IUnitsOfWorks _mockUow;
    private readonly SearchUserQueryHandler _handler;

    public SearchUserQueryHandlerTests()
    {
        _mockUow = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchUserQueryHandler(_mockUow);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenNoUsersFound()
    {
        // Arrange
        var request = new SearchUserQueryRequest
        {
            // You can set up request parameters here as needed
        };

        _mockUow.UserRepo.Search(Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<string?>(),Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<Role?>(), Arg.Any<string?>())!
            .Returns((IEnumerable<UserDto?>)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserListIsEmpty()
    {
        // Arrange
        var request = new SearchUserQueryRequest
        {
            // You can set up request parameters here as needed
        };

        _mockUow.UserRepo.Search(Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<Role?>(), Arg.Any<string?>())
            .Returns(Enumerable.Empty<UserDto>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUsersAreFound()
    {
        // Arrange
        var users = new List<UserDto>
        {
            new UserDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new UserDto { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };

        var request = new SearchUserQueryRequest
        {
            // You can set up request parameters here as needed
        };

        _mockUow.UserRepo.Search(Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<Role?>(), Arg.Any<string?>())
            .Returns(users);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(users);
    }
}
