using Microsoft.AspNetCore.Authentication;
using notifier.Application.Authentication.Command.BasicAuthHandler;
using notifier.Application.Utils;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace notifier.Application;




public static class ConfigureService
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services) 
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureService).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<Ping>();
        services.AddTransient<TcpClient>();
        services.AddTransient<ISendRequestHelper, SendRequestHelper>();
        services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        return services;
    }
}
