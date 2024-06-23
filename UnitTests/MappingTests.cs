namespace UnitTests;

using FluentAssertions;
using WebApiDemo.Dal.Records;
using WebApiDemo.Service.Command.Category;
using WebApiDemo.Service.Domain;
using WebApiDemo.Service.Mapping;

public class MappingTests
{
    [Fact]
    public void ToCateGoryCommand_Success()
    {
        CategoryDto input = new () { Category = "cat", SubCategory = "subcAt", Id = Guid.NewGuid() };
        CategoryCommand result = input.ToCategoryCommand();
        result.Id.Should().Be(input.Id);
        result.Category.Should().Be(input.Category);
        result.SubCategory.Should().Be(input.SubCategory);
    }

    [Fact]
    public void ToCategoryDTO_Success()
    {
        CategoryRecord input = new () { Category = "cat", SubCategory = "subcAt", Id = Guid.NewGuid() };
        CategoryDto result = input.ToCategoryDto();
        result.Id.Should().Be(input.Id);
        result.Category.Should().Be(input.Category);
        result.SubCategory.Should().Be(input.SubCategory);
    }

    [Fact]
    public void ToCategoryRecord_ConvertToHeaderText_Success()
    {
        CategoryCommand input = new () { Category = "cat", SubCategory = "subcAt", Id = Guid.NewGuid() };
        CategoryRecord result = input.ToCategoryRecord();
        result.Id.ToString().Should().Be(input.Id.ToString());
        result.Category.Should().Be("Cat");
        result.SubCategory.Should().Be("Subcat");
    }
}