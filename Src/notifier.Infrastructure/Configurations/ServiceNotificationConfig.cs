
namespace notifier.Infrastructure.Configurations;



internal sealed class ServiceNotificationConfig : IEntityTypeConfiguration<ServiceNotfications>
{
    public void Configure(EntityTypeBuilder<ServiceNotfications> builder)
    {
        builder.Property(S => S.MessageFormat).HasMaxLength(500).IsRequired();
    }
}
