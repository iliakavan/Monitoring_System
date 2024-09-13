using notifier.Application.ProjectOfficials.Commands.UpdateProjectOffical;

namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;


public class UpdateProjectOfficialHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly UpdateProjectOfficialCommandHandler _handler;

    public UpdateProjectOfficialHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateProjectOfficialCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_ProjectOfficialExists_ShouldReturnSuccessResult()
    {
        // Arrange
        var request = new UpdateProjectOfficialCommandRequest
        {
            Id = 1,
            ProjectId = 2,
            Responsible = "Jane Doe",
            TelegramId = "telegram456",
            Mobile = "987654321"
        };

        var projectOfficial = new ProjectOfficial
        {
            Id = 1,
            ProjectId = 1,
            Responsible = "John Doe",
            TelegramId = "telegram123",
            Mobile = "123456789"
        };

        _unitOfWorkSubstitute.ProjectOffcialRepo.GetById(1)!
            .Returns(Task.FromResult(projectOfficial));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        projectOfficial.ProjectId.Should().Be(request.ProjectId);
        projectOfficial.Responsible.Should().Be(request.Responsible);
        projectOfficial.TelegramId.Should().Be(request.TelegramId);
        projectOfficial.Mobile.Should().Be(request.Mobile);
    }

    [Fact]
    public async Task Handle_ProjectOfficialDoesNotExist_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new UpdateProjectOfficialCommandRequest { Id = 1 ,Mobile = "09252155445",Responsible="sdsada",TelegramId="@qwerty"};

        _unitOfWorkSubstitute.ProjectOffcialRepo.GetById(1)!
            .Returns(Task.FromResult<ProjectOfficial>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}
