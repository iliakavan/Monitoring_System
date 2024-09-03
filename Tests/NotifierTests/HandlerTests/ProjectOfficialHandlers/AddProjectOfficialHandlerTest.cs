using notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;

namespace NotifierTests.HandlerTests.ProjectOfficialHandlers;



public class AddProjectOfficialHandlerTest
{
    private readonly IUnitsOfWorks _unitOfWorkSubstitute;
    private readonly AddProjectOfficialCommandHandler _handler;

    public AddProjectOfficialHandlerTest()
    {
        _unitOfWorkSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new AddProjectOfficialCommandHandler(_unitOfWorkSubstitute);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        var request = new AddProjectOfficialCommandRequest
        {
            Mobile = "123456789",
            Responsible = "John Doe",
            TelegramId = "telegram123",
            ProjectId = 1
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _unitOfWorkSubstitute.ProjectOffcialRepo
            .Received(1)
            .Add(Arg.Is<ProjectOfficial>(o =>
                o.Mobile == request.Mobile &&
                o.Responsible == request.Responsible &&
                o.TelegramId == request.TelegramId &&
                o.ProjectId == request.ProjectId &&
                o.RecordDate.Date == DateTime.Now.Date));

        await _unitOfWorkSubstitute.Received(1).SaveChanges();

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Project Official Added To Project");
    }
}
