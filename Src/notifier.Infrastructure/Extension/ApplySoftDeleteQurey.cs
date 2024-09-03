using notifier.Domain.BaseModel;
using System.Linq.Expressions;

namespace notifier.Infrastructure.Extension;


public static class ApplySoftDeleteQurey
{
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var methodToCall = typeof(ApplySoftDeleteQurey)
                    .GetMethod(nameof(GetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                var filter = methodToCall.Invoke(null, new object[] { });
                entityType.SetQueryFilter((LambdaExpression?)filter);
            }
        }
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> filter = x => x.IsActive;
        return filter;
    }
}
