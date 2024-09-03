namespace notifier.Infrastructure.Configurations;




internal sealed class ServiceConfig : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(S => S.Url).HasMaxLength(500).IsRequired();
        builder.Property(S => S.Title).HasMaxLength(50).IsRequired();
        builder.Property(S => S.Ip).HasMaxLength(15);
        builder.Property(S => S.Method).HasMaxLength(10).IsRequired();
    }
}
