
namespace notifier.Infrastructure.Configurations;


internal sealed class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(P => P.Description).HasMaxLength(500);
        builder.Property(S => S.Title).HasMaxLength(50).IsRequired();
    }
}
