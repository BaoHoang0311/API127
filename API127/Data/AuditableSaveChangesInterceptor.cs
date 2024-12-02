using API127;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ERP.Infrastructure.Persistence.Interceptors;

public class AuditableSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.Now;
            }

            if (entry.State == EntityState.Modified )
            {
                entry.Entity.UpdatedDate = DateTime.Now;
            }
        }
    }
}
public abstract class BaseAuditableEntity
{
    public DateTime? CreatedDate { get; set; }
    //public long CreatedByUserID { get; set; }
    //public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
    //public long? LastModifiedByUserID { get; set; }
    //public string? LastModifiedBy { get; set; }
}