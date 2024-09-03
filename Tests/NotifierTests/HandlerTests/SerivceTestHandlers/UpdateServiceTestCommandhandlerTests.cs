using notifier.Application.ServiceTests.Command.AddServiceTest;
using notifier.Application.ServiceTests.Command.UpdateServiceTest;
using System.Reflection.Metadata;

namespace NotifierTests.HandlerTests.SerivceTestHandlers;


public class UpdateServiceTestCommandhandlerTests
{
    private readonly IUnitsOfWorks _UnitOfWorks;
    private readonly UpdateServiceTestCommandHandler _handler;

    public UpdateServiceTestCommandhandlerTests()
    {
        _UnitOfWorks = Substitute.For<IUnitsOfWorks>();
        _handler = new UpdateServiceTestCommandHandler(_UnitOfWorks);
    }
    [Fact]
    public async Task Handle_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        var serviceTest = new ServiceTest
        {
            Id = 1,
            PriodTime = 30,
            TestType = 0,
            ServiceId = 100
        };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _UnitOfWorks.ServiceTestRepo.GetById(Arg.Any<int>()).Returns(Task.FromResult(serviceTest));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _UnitOfWorks.SaveChanges().Returns(Task.CompletedTask);

        var request = new UpdateServiceTestCommandRequest
        {
            Id = 1,
            PriodTime = 20,  // Providing a new value
            TestType = null,            // Not providing a new value, should retain original
            ServiceId = null            // Not providing a new value, should retain original
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        serviceTest.PriodTime.Should().Be(20);  // Should be updated
        serviceTest.TestType.Should().Be(0);  // Should remain unchanged
        serviceTest.ServiceId.Should().Be(100);            // Should remain unchanged

        _UnitOfWorks.ServiceTestRepo.Received(1).Update(serviceTest);
        await _UnitOfWorks.Received(1).SaveChanges();
    }
}
