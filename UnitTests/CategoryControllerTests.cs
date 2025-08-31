namespace UnitTests;

using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using WebApiDemo.Api.Controllers;
using WebApiDemo.Service.Command.Category;
using WebApiDemo.Service.Domain;

public class CategoryControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly CategoryController _sut;

    public CategoryControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _sut = new CategoryController(_mediator.Object);
    }

    [Fact]
    public async Task SaveCategory_Failure()
    {
        // arrange
        CategoryDto item = new () { Category = "Bumpy", SubCategory = "Tummy" };
        CategoryDto returnItem = new () { Category = "Bumpy", SubCategory = "Tummy", Id = Guid.NewGuid() };

        _mediator.Setup(x => x.Send(It.IsAny<CategoryCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Odds Bodkins"));

        ActionResult<CategoryDto> result = await _sut.CreateCategoryAsync(item, default);

        StatusCodeFromActionResult(result).Should().Be(HttpStatusCode.InternalServerError);
        CategoryDtoFromActionResult(result).Should().Be(null);
    }

    [Fact]
    public async Task SaveCategory_Success()
    {
        // arrange
        CategoryDto item = new () { Category = "Bumpy", SubCategory = "Tummy" };
        CategoryDto returnItem = new () { Category = "Bumpy", SubCategory = "Tummy", Id = Guid.NewGuid() };

        _mediator.Setup(x => x.Send(It.IsAny<CategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnItem);

        ActionResult<CategoryDto> result = await _sut.CreateCategoryAsync(item, default);

        StatusCodeFromActionResult(result).Should().Be(HttpStatusCode.OK);
        CategoryDtoFromActionResult(result)!.Id.Should().Be(returnItem.Id);
    }

    private static CategoryDto? CategoryDtoFromActionResult<T>(ActionResult<T> actionResult)
    {
        return (CategoryDto?)(actionResult.Result as ObjectResult)?.Value;
    }

    private static HttpStatusCode? StatusCodeFromActionResult<T>(ActionResult<T> actionResult)
    {
        IConvertToActionResult convertToActionResult = actionResult;
        IStatusCodeActionResult? actionResultWithStatusCode = convertToActionResult.Convert() as IStatusCodeActionResult;
        return (HttpStatusCode)actionResultWithStatusCode?.StatusCode!;
    }
}