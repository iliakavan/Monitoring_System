namespace notifier.Middleware;





// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception error)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                NullReferenceException e => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError,// unhandled error
            };
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MiddlewareExtensions1
{
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandler>();
    }
}
