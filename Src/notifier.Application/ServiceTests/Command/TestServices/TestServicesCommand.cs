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
        var pingResult = new ResultResponse<PingDto>();

        foreach (var item in serviceTests)
        {
            ResultResponse result = new();
            string message = string.Empty;
            int MaxRetryAttempts = await uow.NotificationRepo.GetMaxRetryCount(item.Id);

            switch (item.ServiceTest.TestType)
            {
                case TestType.TelNet:
                    result = await reqHelper.ConnectToServerAsync(item.ServiceTest.Service.Ip!, item.ServiceTest.Service.Port!.Value);
                    break;
                case TestType.Ping:
                    var addressToPing = !string.IsNullOrEmpty(item.ServiceTest.Service.Ip)? item.ServiceTest.Service.Ip: item.ServiceTest.Service.Url;
                    pingResult = await reqHelper.PingRequestAsync(addressToPing!);
                    var pingDetails = pingResult.Value;
                    if (!pingResult.Success)
                    {
                        message = $"Monitor : {item.ServiceTest.Service.Title}\nMessage : {item.MessageFormat}\nError: {pingResult.Message}.\nAddress: {addressToPing}\nSuccessPercent : {pingDetails.SuccessPercent}\nRequest Type: {item.ServiceTest.TestType}";
                    }
                    break;
                case TestType.Curl:
                    result = await reqHelper.MakeHttpRequestAsync(item.ServiceTest.Service.Url!);
                    break;

            }

            var address =  !string.IsNullOrEmpty(item.ServiceTest.Service.Url) ? item.ServiceTest.Service.Url :  item.ServiceTest.Service.Ip + ":" + item.ServiceTest.Service.Port;


            if (!result.Success || !pingResult!.Success)
            {
                if(item.ServiceTest.LastStatus == LastStatus.Success && item.ServiceTest.TestType != TestType.Ping) 
                {
                    message = $"Monitor : {item.ServiceTest.Service.Title}\nMessage : {item.MessageFormat}\nMethod : {item.ServiceTest.Service.Method}\nError: {result.Message}\nAddress: {address}\nRequest Type: {item.ServiceTest.TestType}";
                    
                    // Send notification to all users
                    var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                    foreach (var user in users)
                    {
                        await telService.SendMessage(user, message);
                    }
                }
                else if(item.ServiceTest.LastStatus == LastStatus.Success && item.ServiceTest.TestType == TestType.Ping) 
                {
                    var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                    foreach (var user in users)
                    {
                        await telService.SendMessage(user, message);
                    }
                }
                item.RetryCount += 1;
                item.ErrorRetryCount += 1;
                
                if(item.RetryCount == MaxRetryAttempts) 
                {
                    message = $"{item.ServiceTest.Service.Title} is still down after {item.ErrorRetryCount} attempts.\nStatus : {result.Message ?? pingResult!.Message}\nAddress : {address}\n Test Type : {item.ServiceTest.TestType}";

                    // Send notification to all users
                    var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                    foreach (var user in users)
                    {
                        await telService.SendMessage(user, message);
                    }
                    // Reset retry count for continuous monitoring
                    item.RetryCount = 0;
                }
                item.ServiceTest.LastStatus = LastStatus.Error;
            }
            else 
            {
                // If the service becomes successful after a failure, send a success notification
                if (item.ServiceTest.LastStatus == LastStatus.Error && item.ServiceTest.TestType != TestType.Ping)
                {
                    message = $"Monitor : {item.ServiceTest.Service.Title}\nMessage : {item.MessageSuccess}\nMethod : {item.ServiceTest.Service.Method}\nAddress: {address}\nRequest Type: {item.ServiceTest.TestType}";

                    // Send notification to all users
                    var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                    foreach (var user in users)
                    {
                        await telService.SendMessage(user, message);
                    }
                }
                else if (item.ServiceTest.LastStatus == LastStatus.Error && item.ServiceTest.TestType == TestType.Ping)
                {
                    message = $"Monitor : {item.ServiceTest.Service.Title}\nRound-Trip Time : {pingResult.Value.RoundTriptime}\nSuccess Percent : {pingResult.Value.SuccessPercent}"; 
                    var users = await uow.ProjectOffcialRepo.FetchTelegramId(item.ServiceTest.Service.ProjectId);
                    foreach (var user in users)
                    {
                        await telService.SendMessage(user, message);
                    }
                }
                // Reset retry count and update status to successful
                item.RetryCount = 0;
                item.ServiceTest.LastStatus = LastStatus.Success;
            }


            await uow.ServiceTestLogRepo.Insert(new ServiceTestLog()
            {
                RecordDate = DateTime.Now,
                ResponseCode = (result?.Message ?? pingResult?.Message)!,
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
