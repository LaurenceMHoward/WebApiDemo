namespace UnitTests;

using System.Net;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using WebApiDemo.Api.Controllers;
using WebApiDemo.Api.Validators.Validation;
using WebApiDemo.Service.Command.Category;
using WebApiDemo.Service.Domain;

public class CategoryControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly CategoryController _sut;
    private readonly Mock<CategoryValidator> _validator;

    public CategoryControllerTests()
    {
        _validator = new Mock<CategoryValidator>();
        _mediator = new Mock<IMediator>();
        _sut = new CategoryController(_mediator.Object, _validator.Object);
    }

    [Fact]
    public async Task SaveCategory_Failure()
    {
        // arrange
        CategoryDto item = new () { Category = "Bumpy", SubCategory = "Tummy" };
        CategoryDto returnItem = new () { Category = "Bumpy", SubCategory = "Tummy", Id = Guid.NewGuid() };

        _validator.Setup(
                x => x.ValidateAsync(It.IsAny<ValidationContext<CategoryDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mediator.Setup(x => x.Send(It.IsAny<CategoryCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Odds Bodkins"));

        ActionResult<CategoryDto> result = await _sut.CreateCategoryAsync(item, default);

        StatusCodeFromActionResult(result).Should().Be(HttpStatusCode.InternalServerError);
        CategoryDtoFromActionResult(result).Should().Be(null);
    }

    [Fact]
    public async Task SaveCategory_InvalidModeState_Failure()
    {
        // arrange
        CategoryDto item = new () { Category = "Bumpy", SubCategory = "Tummy" };
        CategoryDto returnItem = new () { Category = "Bumpy", SubCategory = "Tummy", Id = Guid.NewGuid() };
        List<ValidationFailure> list = [new ValidationFailure("A", "Not So good")];

        _validator.Setup(
                x => x.ValidateAsync(It.IsAny<ValidationContext<CategoryDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult() { Errors = list });

        _mediator.Setup(x => x.Send(It.IsAny<CategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnItem);

        ActionResult<CategoryDto> result = await _sut.CreateCategoryAsync(item, default);

        StatusCodeFromActionResult(result).Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SaveCategory_Success()
    {
        // arrange
        CategoryDto item = new () { Category = "Bumpy", SubCategory = "Tummy" };
        CategoryDto returnItem = new () { Category = "Bumpy", SubCategory = "Tummy", Id = Guid.NewGuid() };

        _validator.Setup(
                x => x.ValidateAsync(It.IsAny<ValidationContext<CategoryDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

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