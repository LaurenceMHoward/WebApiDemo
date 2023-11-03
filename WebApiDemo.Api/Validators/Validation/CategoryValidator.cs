using FluentValidation;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Api.Validators.Validation;

using Dal.Context;
using FluentValidation.Results;
using Reference;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
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
            .WithMessage("Category must be a single word, 4 - 25 letters long");
        RuleFor(x => x.SubCategory).NotNull().Matches(RegexList.CategorySingleWordRegex)
            .WithMessage("Subcategory must be a single word, 4 - 25 letters long");
        RuleFor(x => x.SubCategory).Must((o, _) => CategoryAndSubCategoryDiffer(o))
            .WithMessage("Category and subcategory cannot be the same");

        // database checks
        // run for new and updates
        RuleFor(x => x).MustAsync(CheckCanAddCategoryToDb)
            .WithMessage("The submitted category/subcategory already exists");
    }

    private static bool CategoryAndSubCategoryDiffer(CategoryDto createCategoryCommand)
    {
        return !createCategoryCommand.Category.Trim().ToLower()
            .Equals(createCategoryCommand.SubCategory.Trim().ToLower());
    }

    private async Task<bool> CheckCanAddCategoryToDb(CategoryDto categorySet, CancellationToken cancellationToken)
    {
        string category = categorySet.Category.Trim().ToLower();
        string subCategory = categorySet.SubCategory.Trim().ToLower();

        var item = await _context.FindAnyMatchingLiveCategoryAndSubCategoryAsync(category, subCategory,
            cancellationToken);
        return item == null;
    }
}