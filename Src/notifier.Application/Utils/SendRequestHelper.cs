using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using DNTPersianUtils.Core;

namespace notifier.Application.Utils
{
    public class SendRequestHelper : ISendRequestHelper
    {
        private Ping _ping;
        private readonly TcpClient _tcpClient;
        private readonly HttpClient _httpClient;
        

        public SendRequestHelper(Ping ping, TcpClient tcpClient, HttpClient httpClient)
        {
            _ping = ping;
            _tcpClient = tcpClient;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<ResultResponse<PingDto>> PingRequestAsync(string ipAddress)
        {
            try
            {
                var pingTasks = new List<Task<PingReply>>();
                for (int i = 0; i < 10; i++)
                {
                    _ping = new Ping();
                    pingTasks.Add(_ping.SendPingAsync(ipAddress, 1000)); // 1 second timeout
                }

                // Wait for all pings to complete
                var replies = await Task.WhenAll(pingTasks);

                // Filter out any failed pings where the reply is null or unsuccessful
                var successfulReplies = replies.Where(p => p.Status == IPStatus.Success).ToList();

                int successCount = successfulReplies.Count;
                int totalPings = replies.Length;
                int successPercent = (successCount * 100) / totalPings;

                if (successCount == 0)
                {
                    return new ResultResponse<PingDto>()
                    {
                        Success = false,
                        Message = "No successful pings.",
                        Value = new PingDto
                        {
                            SuccessPercent = successPercent
                        }
                    };
                }
                var firstSuccessfulReply = successfulReplies.First();
                PingDto result = new()
                {
                    Address = firstSuccessfulReply.Address.ToString(),
                    RoundTriptime = successfulReplies.Average(p => p.RoundtripTime).ToString(),
                    BufferSize = firstSuccessfulReply.Buffer.Length.ToString(),
                    DontFragment = firstSuccessfulReply.Options?.DontFragment.ToString() ?? "N/A",
                    Ttl = firstSuccessfulReply.Options?.Ttl.ToString() ?? "N/A",
                    SuccessPercent = successPercent,
                };

                return new ResultResponse<PingDto>() 
                {
                    Success = result.SuccessPercent >= 50,
                    Value = result , 
                    Message = (result.SuccessPercent >= 50) ? "Ping Tests was Successfull" : "Ping Tests was Unsuccessfull" 
                };
            }
            catch (Exception ex)
            {

                return new ResultResponse<PingDto>() 
                {
                    Success = false, Message = $"Ping test failed: {ex.Message}" ,
                    Value = new PingDto
                    {
                        SuccessPercent = 0
                    }
                };
            }
        }


        public async Task<ResultResponse> ConnectToServerAsync(string ipAddress, int port)
        {
            try
            {
                await _tcpClient.ConnectAsync(ipAddress, port);
                return new() { Success = true , Message = "Telnet Test Successfull"};
            }
            catch
            {
                return new() { Success = false , Message = "TelNet test Unsuccessfull"};
            }
        }

        public async Task<ResultResponse> MakeHttpRequestAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                return response.StatusCode switch
                {
                    HttpStatusCode.OK => new ResultResponse() { Success = true, Message = "Ok (200)" },
                    HttpStatusCode.BadRequest => new ResultResponse() { Success = false, Message = "Bad Request (400)" },
                    HttpStatusCode.Unauthorized => new ResultResponse() { Success = false, Message = "Unauthorized (401)" },
                    HttpStatusCode.Forbidden => new ResultResponse() { Success = false, Message = "Forbidden (403)" },
                    HttpStatusCode.ServiceUnavailable => new ResultResponse() { Success = false, Message = "Service Unavailable (503)" },
                    _ => new() { Success = false, Message = $"{(int)response.StatusCode} - {response.ReasonPhrase}" },
                };
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketEx && socketEx.SocketErrorCode == SocketError.HostNotFound)
            {
                return new ResultResponse() { Success = false, Message = "DNS Error: Host not found." };
            }
            catch (HttpRequestException)
            {
                return new ResultResponse() { Success = false, Message = $"Request failed" };
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                return new ResultResponse() { Success = false, Message = "Request timed out. Please try again." };
            }
            catch (Exception)
            {
                return new ResultResponse() { Success = false, Message = $"An unexpected error occurred" };
            }
        }
    }
}
