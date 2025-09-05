using Microsoft.EntityFrameworkCore;
using WebApiDemo.Dal.Records;

namespace WebApiDemo.Dal.Context;

public partial class WebApiDemoDbContext : IWebApiDemoDbContext
{
    public async Task AddCategoryAsync(CategoryRecord newRecord, CancellationToken cancellationToken)
    {
        await Set<CategoryRecord>().AddAsync(newRecord, cancellationToken);
    }

    public async Task<CategoryRecord?> FindAnyMatchingDeletedCategoryAndSubCategoryAsync(CategoryRecord newRecord,
        CancellationToken cancellationToken = default)
    {
        DbSet<CategoryRecord> ctx = Set<CategoryRecord>();
        return await ctx.AsNoTracking().FirstOrDefaultAsync(x =>
            x.IsDeleted && x.Category.ToLower() == newRecord.Category.ToLower() &&
            x.SubCategory.ToLower() == newRecord.SubCategory.ToLower(), cancellationToken);
    }

    public async Task<CategoryRecord?> FindAnyMatchingLiveCategoryAndSubCategoryAsync(string category,
        string subCategory,
        CancellationToken cancellationToken)
    {
        return await Set<CategoryRecord>().AsNoTracking().SingleOrDefaultAsync(x =>
                x.Category.ToLower() == category.ToLower() && x.SubCategory.ToLower() == subCategory.ToLower() &&
                !x.IsDeleted,
            cancellationToken);
    }

    public async Task<CategoryRecord> FindCategoryByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        DbSet<CategoryRecord> ctx = Set<CategoryRecord>();
        return await ctx.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<CategoryRecord>> GetAllLiveCategoriesAsync(CancellationToken cancellationToken)
    {
        return await Set<CategoryRecord>().Where(x => x.IsDeleted != true)
            .ToListAsync(cancellationToken);
    }

    public void RemoveCategory(CategoryRecord record)
    {
        Set<CategoryRecord>().Remove(record);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        UpdateStatuses();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public void UpdateCategory(CategoryRecord newRecord)
    {
        Set<CategoryRecord>().Update(newRecord);
    }
}