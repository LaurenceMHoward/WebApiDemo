using FluentValidation;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Api.Validators.Validation;

using Dal.Context;
using Dal.Records;
using Reference;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
    private const string CategoryFormat = "Category must be a single word, 4 - 25 letters long";
    private const string CatSubCatDifferent = "Category and subcategory cannot be the same";
    private const string CatSubExists = "The submitted category/subcategory already exists";
    private const string SubCategoryFormat = "Subcategory must be a single word, 4 - 25 letters long";
    private readonly IWebApiDemoDbContext _context;

    public CategoryValidator()
    {
        _context = null!;
    }

    public CategoryValidator(IWebApiDemoDbContext context)
    {
        this._context = context;
        ClassLevelCascadeMode = CascadeMode.Stop;
        // basic validation
        RuleFor(x => x.Category).NotNull().Matches(RegexList.CategorySingleWordRegex)
            .WithMessage(CategoryFormat);
        RuleFor(x => x.SubCategory).NotNull().Matches(RegexList.CategorySingleWordRegex)
            .WithMessage(SubCategoryFormat);
        RuleFor(x => x.SubCategory).Must((o, _) => CategoryAndSubCategoryDiffer(o))
            .WithMessage(CatSubCatDifferent);

        // database checks
        // run for new and updates
        RuleFor(x => x).MustAsync(CheckCanAddCategoryToDb)
            .WithMessage(CatSubExists);
    }

    private static bool CategoryAndSubCategoryDiffer(CategoryDto createCategoryCommand)
    {
        return !createCategoryCommand.Category.Trim()
            .Equals(createCategoryCommand.SubCategory.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private async Task<bool> CheckCanAddCategoryToDb(CategoryDto categorySet, CancellationToken cancellationToken)
    {
        string category = categorySet.Category.Trim();
        string subCategory = categorySet.SubCategory.Trim();

        CategoryRecord? item = await _context.FindAnyMatchingLiveCategoryAndSubCategoryAsync(category, subCategory,
            cancellationToken);
        return item == null;
    }
}