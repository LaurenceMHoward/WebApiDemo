using WebApiDemo.Dal.Records;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Query.Categories;

using BaseHandler;
using Dal.Context;
using Mapping;

public class GetAllCategoriesHandler : HandlerBase<GetAllCategories, List<CategoryDto>>
{
    public GetAllCategoriesHandler(IWebApiDemoDbContext context)
        : base(context)
    {
    }

    public override async Task<List<CategoryDto>> Handle(GetAllCategories request,
        CancellationToken cancellationToken)
    {
        List<CategoryRecord> categoryRecords = await _webApiDemoDbContext.GetAllLiveCategoriesAsync(cancellationToken);

        List<CategoryDto> results = categoryRecords.Select(x => x.ToCategoryDto()).OrderBy(x => x.Category)
            .ThenBy(x => x.SubCategory).ToList();

        return results;
    }
}