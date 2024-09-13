using DNTPersianUtils.Core;
using notifier.Application.ServiceNotifications.Queries.Search;
using notifier.Domain.Dto;
using notifier.Domain.Models;

namespace NotifierTests.HandlerTests.ServiceNotficationHandlers;



public class SearchServiceNotificationQueryHandlerTest
{
    private readonly IUnitsOfWorks _unitsOfWorksSubstitute;
    private readonly SearchServiceNotificationQueryHandler _handler;

    public SearchServiceNotificationQueryHandlerTest()
    {
        _unitsOfWorksSubstitute = Substitute.For<IUnitsOfWorks>();
        _handler = new SearchServiceNotificationQueryHandler(_unitsOfWorksSubstitute);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoNotificationsFound()
    {
        // Arrange
        var request = new SearchServiceNotificationQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            NotifeType = notifier.Domain.Enum.NotificationType.Sms,
            ServiceId = 1,
            ServicetestId = 1,
            ProjectId = 2
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.NotificationRepo.Search(startdateEN, enddateEn, request.NotifeType, request.ServicetestId, request.ServiceId, request.ProjectId)
            .Returns(await Task.FromResult<IEnumerable<ServiceNotificationDto?>>(null!));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Service Notification between {request.StartDate} {request.EndDate}");
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNotificationListIsEmpty()
    {
        // Arrange
        var request = new SearchServiceNotificationQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            NotifeType = notifier.Domain.Enum.NotificationType.Sms,
            ServiceId = 2,
            ServicetestId = 1,
            ProjectId = 3
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        _unitsOfWorksSubstitute.NotificationRepo.Search(startdateEN, enddateEn, request.NotifeType, request.ServicetestId, request.ServiceId, request.ProjectId)
            .Returns(await Task.FromResult(Enumerable.Empty<ServiceNotificationDto?>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be($"There is no Service Notification between {request.StartDate} {request.EndDate}");
        result.Value.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenNotificationsAreFound()
    {
        // Arrange
        var request = new SearchServiceNotificationQueryRequest
        {
            StartDate = "1402/05/10",
            EndDate = "1402/05/15",
            NotifeType = notifier.Domain.Enum.NotificationType.Sms,
            ServiceId = 3,
            ProjectId = 1,
            ServicetestId = 2
        };

        DateTime? startdateEN = request.StartDate.ToGregorianDateTime();
        DateTime? enddateEn = request.EndDate.ToGregorianDateTime();

        var mockNotifications = new List<ServiceNotificationDto>
        {
            new ServiceNotificationDto {NotificationType = notifier.Domain.Enum.NotificationType.Sms.ToString(), ServiceTestId = 3, RecordDate = DateTime.Now,MessageFormat = "kmsdm",  MessageSuccess = "test" }
        };

        _unitsOfWorksSubstitute.NotificationRepo.Search(startdateEN, enddateEn, request.NotifeType, request.ServicetestId, request.ServiceId,request.ProjectId)!
            .Returns(Task.FromResult<IEnumerable<ServiceNotificationDto>>(mockNotifications));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(mockNotifications);
    }
}
