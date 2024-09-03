using notifier.Application.BackgroundService;
using notifier.Application.Utils;


namespace notifier.Application.ServiceTests.Command.TestServices;

public class TestServicesCommand : IRequest<ResultResponse>
{
}
public class TestServicesCommandHandler(IUnitsOfWorks uow,ISendRequestHelper reqHelper,ITelegramService telService) : IRequestHandler<TestServicesCommand, ResultResponse>
{
    public async Task<ResultResponse> Handle(TestServicesCommand request, CancellationToken cancellationToken)
    {
        var serviceTests = await uow.NotificationRepo.GetAllServices();
        var result = new ResultResponse();
        var pingResult = new ResultResponse<PingDto>();
        string message = string.Empty;

        foreach (var item in serviceTests)
        {
            result = new ResultResponse();
            message = string.Empty;

            switch (item.ServiceTest.TestType)
            {
                case TestType.TelNet:
                    result = await reqHelper.ConnectToServerAsync(item.ServiceTest.Service.Ip!, item.ServiceTest.Service.Port!.Value);
                    break;
                case TestType.Ping:
                    var addressToPing = !string.IsNullOrEmpty(item.ServiceTest.Service.Ip)? item.ServiceTest.Service.Ip: item.ServiceTest.Service.Url;
                    pingResult = await reqHelper.PingRequestAsync(addressToPing!);
                    var pingDetails = pingResult.Value;
                    if (pingResult.Success)
                    {
                        message = $"{item.ServiceTest.Service.Title}:\n{item.MessageSuccess}\nAddress: {pingDetails.Address}\nRound-Trip Time: {pingDetails.RoundTriptime} ms\nSuccessPercent : {pingDetails.SuccessPercent}\nRequest Type: {item.ServiceTest.TestType}";
                    }
                    else
                    {
                        message = $"{item.ServiceTest.Service.Title}:\n{item.MessageFormat}\nError: Ping request failed.\nAddress: {addressToPing}\nSuccessPercent : {pingDetails.SuccessPercent}\nRequest Type: {item.ServiceTest.TestType}";
                    }
                    break;
                case TestType.Curl:
                    result = await reqHelper.MakeHttpRequestAsync(item.ServiceTest.Service.Url!);
                    break;
            }

            var address =  !string.IsNullOrEmpty(item.ServiceTest.Service.Url) ? item.ServiceTest.Service.Url :  item.ServiceTest.Service.Ip + ":" + item.ServiceTest.Service.Port;

            

            if (item.ServiceTest.TestType != TestType.Ping)
            {
                message = result.Success
                    ? $"{item.ServiceTest.Service.Title}:\n{item.MessageSuccess}\nMethod: {item.ServiceTest.Service.Method}\nAddress: {address}\nRequest Type: {item.ServiceTest.TestType}"
                    : $"{item.ServiceTest.Service.Title}:\n{item.MessageFormat}\nMethod: {item.ServiceTest.Service.Method}\nError: {result.Message}\nAddress: {address}\nRequest Type: {item.ServiceTest.TestType}";
            }

            if (item.NotificationType == NotificationType.Telegram)
            {
                var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                foreach (var user in users)
                {
                    await telService.SendMessage(user, message);
                }
            }


            item.RetryCount = result.Success == true ? 0 : item.RetryCount + 1;

            await uow.ServiceTestLogRepo.Insert(new ServiceTestLog()
            {
                RecordDate = DateTime.Now,
                ResponseCode = (result?.Message ?? pingResult?.Message) ?? "Unknown",
                ServiceId = item.ServiceTest.ServiceId,
                ServiceNotificationId = item.Id
            });

            uow.NotificationRepo.Update(item);
            await uow.SaveChanges();
        }

        return new ResultResponse()
        {
            Success = true
        };
    }
}
