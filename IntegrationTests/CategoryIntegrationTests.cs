using IntegrationTests.Reference;
using WebApiDemo.Service.Domain;

namespace IntegrationTests;

using Bogus;
using FluentAssertions;

/// <summary>
/// Will not substitute for db tests, but useful integration testing.
/// </summary>
public class CategoryIntegrationTests
{
    private readonly ControllerActions _sut;

    public CategoryIntegrationTests()
    {
        _sut = new ControllerActions();
        SeededCategoryCount = _sut.CategoryCount;
    }

    private static int SeededCategoryCount { get; set; }
    
    [Fact]
    public async Task CreateCategoryAsync_AlreadyExists_Fail()
    {
        string target = "api/1/category";
        CategoryDto newCategory = new() { Category = "TestA", SubCategory = "TestB" };

        TestResult<CategoryDto> resultMessage = await _sut.PutAsync<CategoryDto>(target, newCategory);

        resultMessage.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.BadRequest);

        resultMessage.Content.Should().BeNull();
        resultMessage.ExceptionMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCategoryAsync_CatSubCatIsSame_Fail()
    {
        string target = "api/1/category";
        CategoryDto newCategory = new() { Category = "Science", SubCategory = "Science" };

        TestResult<CategoryDto> resultMessage = await _sut.PutAsync<CategoryDto>(target, newCategory);

        resultMessage.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.BadRequest);

        resultMessage.Content.Should().BeNull();
        resultMessage.ExceptionMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCategoryAsync_Success()
    {
        string target = "api/1/category";
        CategoryDto newCategory = (CategoryDto)new Faker<CategoryDto>()
            .CustomInstantiator(f => new CategoryDto()
            {
                Category = "TestZ",
                SubCategory = "TestY",
            });

        TestResult<CategoryDto> resultMessage = await _sut.PutAsync<CategoryDto>(target, newCategory);

        resultMessage.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.OK);

        resultMessage.Content.Should().NotBeNull();
        resultMessage.Content!.Id.Should().HaveValue();
        resultMessage.Content!.Category.Should().NotBe(newCategory.Category);
        resultMessage.Content!.SubCategory.Should().NotBe(newCategory.SubCategory);
        resultMessage.Content!.Category.ToLower().Should().Be(newCategory.Category.ToLower());
        resultMessage.Content!.SubCategory.ToLower().Should().Be(newCategory.SubCategory.ToLower());
        // first char should be upper
        resultMessage.Content!.Category[0].Should().Be(newCategory.Category.ToUpper()[0]);
        resultMessage.Content!.SubCategory[0].Should().Be(newCategory.SubCategory.ToUpper()[0]);
    }

    [Fact]
    public async Task DeleteCategory_And_GetCategories_CountReduced_Success()
    {
        Guid? deletingItem = _sut.FirstCategoryItem.Id;
        string target = $"api/1/category/{deletingItem}";

        TestResult<CategoryDto> resultMessageDeleted = await _sut.DeleteAsync<CategoryDto>(target);

        resultMessageDeleted.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.OK);
        resultMessageDeleted.Content!.Id.Should().Be(_sut.FirstCategoryItem.Id);

        target = "api/1/category";
        TestResult<CategoryDto[]> resultMessage = await _sut.GetAsync<CategoryDto[]>(target);

        resultMessage.Content!.Length.Should().Be(SeededCategoryCount - 1);
    }

    [Fact]
    public async Task DeleteCategory_And_SaveSameCategory_CountRemainsSame_Success()
    {
        // delete item
        Guid? deletingItem = _sut.FirstCategoryItem.Id;
        string target = $"api/1/category/{deletingItem}";
        TestResult<CategoryDto> resultMessageDeleted = await _sut.DeleteAsync<CategoryDto>(target);

        resultMessageDeleted.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.OK);
        resultMessageDeleted.Content!.Id.Should().Be(_sut.FirstCategoryItem.Id);

        // test deleted item reduced count
        target = "api/1/category";
        TestResult<CategoryDto[]> resultMessage = await _sut.GetAsync<CategoryDto[]>(target);
        resultMessage.Content!.Length.Should().Be(SeededCategoryCount - 1);

        // save deleted item under a different id should map to original
        CategoryDto deletedItemSave = new()
            { Id = null, Category = _sut.FirstCategoryItem.Category, SubCategory = _sut.FirstCategoryItem.SubCategory };
        target = "api/1/category";
        TestResult<CategoryDto> resultMessageSaved = await _sut.PutAsync<CategoryDto>(target, deletedItemSave);
        resultMessageSaved.Content!.Id.Should().Be(deletingItem);

        // check count restored with new save
        target = "api/1/category";
        resultMessage = await _sut.GetAsync<CategoryDto[]>(target);
        resultMessage.Content!.Length.Should().Be(SeededCategoryCount);
        resultMessage.Content.Any(x => x.Id == deletingItem).Should().BeTrue();
    }

    [Fact]
    public async Task GetCategories_Success()
    {
        string target = "api/1/category";
        TestResult<CategoryDto[]> resultMessage = await _sut.GetAsync<CategoryDto[]>(target);

        resultMessage.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.OK);

        resultMessage.Content.Should().NotBeNull().And.HaveCount(SeededCategoryCount);
        foreach (CategoryDto item in resultMessage.Content!)
        {
            item.Id.Should().NotBeEmpty();
            item.Category.Should().NotBeEmpty();
            item.SubCategory.Should().NotBeEmpty();
        }
    }

    [Fact]
    public async Task UpdateCategoryAsync_Success()
    {
        string target = "api/1/category";
        CategoryDto newCategory = new()
            { Id = null, Category = "TestUpdateCategory", SubCategory = "TestUpdateSubCategory" };

        TestResult<CategoryDto> resultMessage = await _sut.PutAsync<CategoryDto>(target, newCategory);

        resultMessage.HttpResponseStatus.Should().Be(System.Net.HttpStatusCode.OK);

        resultMessage.Content.Should().NotBeNull();
        resultMessage.Content!.Id.Should().NotBeNull();
        resultMessage.Content!.Category.Should().NotBe(newCategory.Category);
        resultMessage.Content!.SubCategory.Should().NotBe(newCategory.SubCategory);
        resultMessage.Content!.Category.ToLower().Should().Be(newCategory.Category.ToLower());
        resultMessage.Content!.SubCategory.ToLower().Should().Be(newCategory.SubCategory.ToLower());
        // first char should be upper
        resultMessage.Content!.Category[0].Should().Be(newCategory.Category[0]);
        resultMessage.Content!.SubCategory[0].Should().Be(newCategory.SubCategory[0]);
    }
}