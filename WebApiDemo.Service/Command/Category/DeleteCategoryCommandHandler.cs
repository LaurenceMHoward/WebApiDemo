using WebApiDemo.Dal.Records;
using WebApiDemo.Service.BaseHandler;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Command.Category;

using Dal.Context;
using Mapping;

public class DeleteCategoryCommandHandler : HandlerBase<DeleteCategoryCommand, CategoryDto>
{
    public DeleteCategoryCommandHandler(IWebApiDemoDbContext context)
        : base(context)
    {
    }

    public override async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // need to fetch id, of cats that might be deleted, to map those.
        CategoryRecord newRecord = await _webApiDemoDbContext.FindCategoryByIdAsync(request.Id, cancellationToken);

        _webApiDemoDbContext.RemoveCategory(newRecord);

        await _webApiDemoDbContext.SaveChangesAsync(true, cancellationToken);

        return newRecord!.ToCategoryDto();
    }
}