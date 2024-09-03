namespace notifier.Infrastructure.Configurations;




internal sealed class ProjectOffcialConfig : IEntityTypeConfiguration<ProjectOfficial>
{
    public void Configure(EntityTypeBuilder<ProjectOfficial> builder)
    {
        builder.Property(x => x.Responsible).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Mobile).HasMaxLength(11).IsRequired();
        builder.Property(x => x.TelegramId).HasMaxLength(25).IsRequired();
    }
}
