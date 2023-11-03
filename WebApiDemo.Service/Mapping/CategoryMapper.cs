using WebApiDemo.Dal.Records;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Mapping;

using Command.Category;

public static class CategoryMapper
{
    public static CategoryCommand ToCategoryCommand(this CategoryDto original)
    {
        var newItem = new CategoryCommand
        {
            Category = original.Category,
            SubCategory = original.SubCategory,
            Id = original.Id,
        };

        return newItem;
    }

    public static DeleteCategoryCommand ToDeleteCategoryCommand(this CategoryDto original)
    {
        var newItem = new DeleteCategoryCommand
        {
            Category = original.Category,
            SubCategory = original.SubCategory,
            Id = original.Id,
        };

        return newItem;
    }

    public static CategoryDto ToCategoryDto(this CategoryRecord original)
    {
        var newItem = new CategoryDto
        {
            Category = original.Category,
            SubCategory = original.SubCategory,
            Id = original.Id,
        };

        return newItem;
    }

    public static CategoryRecord ToCategoryRecord(this CategoryCommand original)
    {
        var newItem = new CategoryRecord
        {
            Category = original.Category.Trim().ToUpper()[0] + original.Category.Trim().ToLower()[1..],
            SubCategory = original.SubCategory.Trim().ToUpper()[0] + original.SubCategory.Trim().ToLower()[1..],
            Id = original.Id,
        };

        return newItem;
    }
}