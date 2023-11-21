using FluentValidation;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Api.Validators.Validation;

using Dal.Context;
using Reference;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
    private const string _categoryFormat = "Category must be a single word, 4 - 25 letters long";
    private const string _catSubCatDifferent = "Category and subcategory cannot be the same";
    private const string _catSubExists = "The submitted category/subcategory already exists";
    private const string _subCategoryFormat = "Subcategory must be a single word, 4 - 25 letters long";
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
            .WithMessage(_categoryFormat);
        RuleFor(x => x.SubCategory).NotNull().Matches(RegexList.CategorySingleWordRegex)
            .WithMessage(_subCategoryFormat);
        RuleFor(x => x.SubCategory).Must((o, _) => CategoryAndSubCategoryDiffer(o))
            .WithMessage(_catSubCatDifferent);

        // database checks
        // run for new and updates
        RuleFor(x => x).MustAsync(CheckCanAddCategoryToDb)
            .WithMessage(_catSubExists);
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