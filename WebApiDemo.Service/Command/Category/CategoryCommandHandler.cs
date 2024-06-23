using WebApiDemo.Dal.Records;
using WebApiDemo.Service.BaseHandler;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Command.Category;

using Dal.Context;
using Mapping;

public class CategoryCommandHandler : HandlerBase<CategoryCommand, CategoryDto>
{
    public CategoryCommandHandler(IWebApiDemoDbContext context)
        : base(context)
    {
    }

    public override async Task<CategoryDto> Handle(CategoryCommand request, CancellationToken cancellationToken)
    {
        CategoryRecord newRecord = request.ToCategoryRecord();

        // need to fetch id, of cats that might be deleted, to map those.
        CategoryRecord? deletedId =
            await _webApiDemoDbContext.FindAnyMatchingDeletedCategoryAndSubCategoryAsync(newRecord, cancellationToken);
        if (deletedId?.Id is not null)
        {
            newRecord.Id = deletedId.Id;
        }

        if (newRecord.Id == null)
        {
            await _webApiDemoDbContext.AddCategoryAsync(newRecord, cancellationToken);
        }
        else
        {
            _webApiDemoDbContext.UpdateCategory(newRecord);
        }

        await _webApiDemoDbContext.SaveChangesAsync(true, cancellationToken);

        return newRecord.ToCategoryDto();
    }
}