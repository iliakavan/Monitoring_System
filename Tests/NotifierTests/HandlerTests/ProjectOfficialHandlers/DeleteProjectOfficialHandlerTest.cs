using notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;

namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;



public class DeleteProjectOfficialHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly DeleteProjectOfficialCommandHandler _handler;

    public DeleteProjectOfficialHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new DeleteProjectOfficialCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_ProjectOfficialExists_ShouldReturnSuccessResult()
    {
        // Arrange
        var request = new DeleteProjectOfficialCommandRequest { Id = 1 };
        var projectOfficial = new ProjectOfficial() { Id = 1,Mobile = "0955566898",Responsible = "dsdad",TelegramId = "@sdldsdas" };

        _unitOfWorkSubstitute.ProjectOffcialRepo.GetById(1)!
            .Returns(Task.FromResult(projectOfficial));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkSubstitute.ProjectOffcialRepo
            .Received(1)
            .Delete(Arg.Is<ProjectOfficial>(o => o.Id == projectOfficial.Id));

        await _unitOfWorkSubstitute.Received(1).SaveChanges();

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ProjectOfficialDoesNotExist_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new DeleteProjectOfficialCommandRequest { Id = 1 };

        _unitOfWorkSubstitute.ProjectOffcialRepo.GetById(1)!
            .Returns(Task.FromResult<ProjectOfficial>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        _unitOfWorkSubstitute.ProjectOffcialRepo.DidNotReceive().Delete(Arg.Any<ProjectOfficial>());
        await _unitOfWorkSubstitute.DidNotReceive().SaveChanges();
    }
}
