using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace notifier.Application.Authentication.Command.BasicAuthHandler;



public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUnitsOfWorks _unitsOfWorks;
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUnitsOfWorks uow) : base(options, logger, encoder, clock)
    {
        _unitsOfWorks = uow;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization")) 
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
            var bytes = Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(bytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            var user = await _unitsOfWorks.UserRepo.Authenticate(username, password);

            if(user is not null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role,user.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims,Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            else 
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
        }
        catch 
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
}
