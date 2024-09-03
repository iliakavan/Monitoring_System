namespace notifier.Application.Utils
{
    public interface ISendRequestHelper
    {
        Task<ResultResponse<PingDto>> PingRequestAsync(string ipAddress);
        Task<ResultResponse> ConnectToServerAsync(string ipAddress, int port);
        Task<ResultResponse> MakeHttpRequestAsync(string url);
    }
}
