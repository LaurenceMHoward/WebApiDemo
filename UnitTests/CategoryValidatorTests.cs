namespace UnitTests;

using Common;
using FluentAssertions;
using FluentValidation.TestHelper;
using WebApiDemo.Api.Validators.Validation;
using WebApiDemo.Dal.Context;
using WebApiDemo.Dal.Records;
using WebApiDemo.Service.Domain;

public class CategoryValidatorTests
{
    private readonly IWebApiDemoDbContext _context;
    private readonly CategoryValidator _validator;

    public CategoryValidatorTests()
    {
        _context = DbFactory.GetDbContext();
        _validator = new CategoryValidator(_context);
    }

    [Theory]
    [InlineData("Do", "Ca")] // length, only 1 error
    [InlineData("Dogs", "Cb")] // length alt
    [InlineData("Dogs", "Cat")] // min length 4
    [InlineData("SupercalifragilisticExpial", "Cats")] // max length
    [InlineData("Cats", "SupercalifragilisticExpial")] // max length
    [InlineData("Cats", "Cats")] // same
    [InlineData("Cat1", "Cats")] // numeric
    public async Task ValidateItems_BasicTests_Fail(string category, string subCategory)
    {
        CategoryDto newItem = new () { Category = category, SubCategory = subCategory, Id = null };
        TestValidationResult<CategoryDto>? result = await _validator.TestValidateAsync(newItem);
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ValidateItems_ExistsInDbButNullId_Success()
    {
        CategoryRecord item = DbFactory.GetFirstCategory();

        CategoryDto newItem = new () { Category = item.Category, SubCategory = item.SubCategory, Id = null };
        TestValidationResult<CategoryDto>? result = await _validator.TestValidateAsync(newItem);
        result.Errors.Count.Should().BeGreaterThan(0);
    }
}