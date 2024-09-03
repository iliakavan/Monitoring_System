namespace notifier.Infrastructure.Configurations;



internal sealed class ServiceTestLogConfig : IEntityTypeConfiguration<ServiceTestLog>
{
    public void Configure(EntityTypeBuilder<ServiceTestLog> builder)
    {
        builder.Property(S => S.ResponseCode).HasMaxLength(50).IsRequired();
    }
}
