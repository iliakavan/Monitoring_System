
namespace notifier.Infrastructure;










public static class ConfigureService
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddDbContext<AppDbcontext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default"),
            builder => builder.MigrationsAssembly(typeof(AppDbcontext).Assembly.FullName)).EnableDetailedErrors(true));

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectOffcialRepository, ProjectOffcialRepository>();
        services.AddScoped<IServiceNotificationRepository,ServiceNotificationRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceTestRepository, ServiceTestRepository>();
        services.AddScoped<IServiceTestLogRepository, ServiceTestLogRepository>();
        services.AddScoped<ITelegramService, TelegramService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitsOfWorks,UnitOfWork>();

        return services;  
    }
}
