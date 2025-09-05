using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApiDemo.Dal.Context.EntityConfiguration;
using WebApiDemo.Dal.Records;
using WebApiDemo.Dal.Records.Base;

namespace WebApiDemo.Dal.Context;

public partial class WebApiDemoDbContext : DbContext
{
    private const string CreatedDate = "CreatedDate";
    private const string DefaultSchema = "dbo";
    private const string LastUpdatedBy = "LastUpdatedBy";
    private const string LastUpdatedDate = "LastUpdatedDate";
    private const string SoftDeleteColumn = "IsDeleted";

    public WebApiDemoDbContext(DbContextOptions<WebApiDemoDbContext> options)
        : base(options)
    {
    }

    protected WebApiDemoDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateStatuses();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new CategoryConfiguration()).Entity<CategoryRecord>();
    }

    private static void AddedEntityState(EntityEntry entry)
    {
        entry.Property(SoftDeleteColumn).CurrentValue = false;
        entry.Property(CreatedDate).CurrentValue = DateTimeOffset.UtcNow;
    }

    private static void DeleteEntityState(EntityEntry entry)
    {
        entry.State = EntityState.Modified;
        entry.Property(SoftDeleteColumn).CurrentValue = true;
        UpdateEntityState(entry);
    }

    private static void UpdateEntityState(EntityEntry entry)
    {
        entry.Property(LastUpdatedDate).CurrentValue = DateTimeOffset.UtcNow;
        entry.Property(LastUpdatedBy).CurrentValue = 0;
        entry.Property(CreatedDate).IsModified = false;
    }

    private void UpdateStatuses()
    {
        foreach (EntityEntry entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    if (entry.Entity is BaseRecord)
                    {
                        DeleteEntityState(entry);
                    }

                    break;

                case EntityState.Added:
                    if (entry.Entity is BaseRecord)
                    {
                        AddedEntityState(entry);
                    }

                    break;

                default:
                    if (entry.Entity is BaseRecord)
                    {
                        UpdateEntityState(entry);
                    }

                    break;
            }
        }
    }
}