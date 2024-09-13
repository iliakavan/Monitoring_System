using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
var services = builder.Services;

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

// Enhanced Swagger configuration for multiple base URLs
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notifier API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

services.RegisterInfrastructure(builder.Configuration);
services.RegisterApplication();
services.AddHttpClient();
services.AddSingleton<IHangfireMethods, HangfireMethods>();

services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("Default"), new SqlServerStorageOptions
    {
        SchemaName = "hangfire_notifier"
    }));

// Add the processing server as IHostedService
services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = GenerateSwaggerUri(httpReq);
        });
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();

var hangfires = app.Services.GetService<IHangfireMethods>();
hangfires?.Run();

#if !DEBUG
app.Use((context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/hangfire"))
    {
        context.Request.PathBase = "/notifier";
    }
    return next();
});
#endif

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangFireExtensions()],
});

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

app.MapControllers();

app.Run();


List<OpenApiServer> GenerateSwaggerUri(HttpRequest httpReq)
{
    var servers = new List<OpenApiServer> {
        new OpenApiServer { Url = $"https://{httpReq.Host.Value}/notifier" },
        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" },
    };
    return servers;
}