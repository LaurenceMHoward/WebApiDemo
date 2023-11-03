using WebApiDemo.Dal.Records;

namespace WebApiDemo.Dal.Context;

public interface IWebApiDemoDbContext : IDisposable
{
    public Task AddCategoryAsync(CategoryRecord newRecord, CancellationToken cancellationToken);

    public Task<CategoryRecord?> FindAnyMatchingDeletedCategoryAndSubCategoryAsync(CategoryRecord newRecord,
        CancellationToken cancellationToken);

    public Task<CategoryRecord?> FindAnyMatchingLiveCategoryAndSubCategoryAsync(string category, string subCategory,
        CancellationToken cancellationToken);

    public Task<CategoryRecord> FindCategoryByIdAsync(Guid? id, CancellationToken cancellationToken);

    public Task<List<CategoryRecord>> GetAllLiveCategoriesAsync(CancellationToken cancellationToken);

    public void RemoveCategory(CategoryRecord record);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default);

    public void UpdateCategory(CategoryRecord newRecord);
}