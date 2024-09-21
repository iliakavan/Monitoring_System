namespace notifier.Infrastructure.Data;

public sealed class AppDbcontext(DbContextOptions<AppDbcontext> options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectOfficial> ProjectOfficials { get; set;}

    public DbSet<Service> Services { get; set; }

    public DbSet<ServiceNotfications> ServiceNotfications { get; set;}

    public DbSet<ServiceTest> ServiceTests { get; set; }

    public DbSet<ServiceTestLog> ServiceTestsLogs { get; set; }

    public DbSet<TelegramMassageLog> TelegramMassageLogs { get; set; }

    public DbSet<Users> Users { get; set; }

    public DbSet<ErrorLog> Errors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplySoftDeleteQueryFilter();
        modelBuilder.ApplyConfiguration(configuration: new ProjectConfig());
        modelBuilder.ApplyConfiguration(configuration: new ProjectOffcialConfig());
        modelBuilder.ApplyConfiguration(configuration: new ServiceConfig());
        modelBuilder.ApplyConfiguration(configuration: new ServiceNotificationConfig());
        modelBuilder.ApplyConfiguration(configuration: new ServiceTestLogConfig());
    }

    private void HandleSoftDelete()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            entity.IsActive = false;
            entry.State = EntityState.Modified;
        }
    }

    public override int SaveChanges()
    {
        HandleSoftDelete();
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        HandleSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }
}
