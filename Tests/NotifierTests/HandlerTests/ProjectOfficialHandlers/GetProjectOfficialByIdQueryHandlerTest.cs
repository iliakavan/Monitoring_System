using notifier.Application.ProjectOfficials.Quries.GetProjectOfficialById;

namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;

public class GetProjectOfficialByIdQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    private readonly GetProjectOfficialByIdQueryHandler _handler;

    public GetProjectOfficialByIdQueryHandlerTest()
    {
        _unitsOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new GetProjectOfficialByIdQueryHandler(_unitsOfWorks);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectOfficialExists()
    {
        // Arrange
        var request = new GetProjectOfficialByIdQueryRequest { Id = 1 };
        var projectOfficial = new ProjectOfficial
        {
            Mobile = "1234567890",
            Responsible = "John Doe",
            TelegramId = "telegram_id",
            RecordDate = DateTime.UtcNow,
            ProjectId = 1
        };

        _unitsOfWorks.ProjectOffcialRepo.GetById(1).Returns(projectOfficial);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Mobile.Should().Be(projectOfficial.Mobile);
        result.Value.Responsible.Should().Be(projectOfficial.Responsible);
        result.Value.TelegramId.Should().Be(projectOfficial.TelegramId);
        result.Value.RecordDate.Should().Be(projectOfficial.RecordDate);
        result.Value.ProjectId.Should().Be(projectOfficial.ProjectId);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectOfficialDoesNotExist()
    {
        // Arrange
        var request = new GetProjectOfficialByIdQueryRequest { Id = 1 };
        _unitsOfWorks.ProjectOffcialRepo.GetById(1).Returns((ProjectOfficial)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
