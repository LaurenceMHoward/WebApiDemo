using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApiDemo.Dal.Context.EntityConfiguration;
using WebApiDemo.Dal.Records;
using WebApiDemo.Dal.Records.Base;

namespace WebApiDemo.Dal.Context;

public partial class WebApiDemoDbContext : DbContext
{
    private const string _createdDate = "CreatedDate";
    private const string _defaultSchema = "dbo";
    private const string _lastUpdatedBy = "LastUpdatedBy";
    private const string _lastUpdatedDate = "LastUpdatedDate";
    private const string _softDeleteColumn = "IsDeleted";

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
        modelBuilder.HasDefaultSchema(_defaultSchema);
        modelBuilder.ApplyConfiguration(new CategoryConfiguration()).Entity<CategoryRecord>();
    }

    private static void AddedEntityState(EntityEntry entry)
    {
        entry.Property(_softDeleteColumn).CurrentValue = false;
        entry.Property(_createdDate).CurrentValue = DateTimeOffset.UtcNow;
    }

    private static void DeleteEntityState(EntityEntry entry)
    {
        entry.State = EntityState.Modified;
        entry.Property(_softDeleteColumn).CurrentValue = true;
        UpdateEntityState(entry);
    }

    private static void UpdateEntityState(EntityEntry entry)
    {
        entry.Property(_lastUpdatedDate).CurrentValue = DateTimeOffset.UtcNow;
        entry.Property(_lastUpdatedBy).CurrentValue = 0;
        entry.Property(_createdDate).IsModified = false;
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