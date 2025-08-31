namespace WebApiDemo.Api.Controllers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Service.Domain;
using Service.Mapping;
using Service.Query.Categories;
using Base;

[ApiVersion("1.0")]
public class CategoryController(IMediator mediator) : BaseController(mediator)
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPut]
    public async Task<ActionResult<CategoryDto>> CreateCategoryAsync([FromBody] CategoryDto item,
        CancellationToken cancellationToken)
    {
        try
        {
            // If validation fails, [ApiController] + AutoValidation will return 400 before this executes.
            var result = await _mediator.Send(item.ToCategoryCommand(), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Exception Saving Category {ex}: {item}", ex, item);
            return StatusCode(500);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> DeleteCategoryAsync([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var dto = new CategoryDto { Id = id };
            var result = await _mediator.Send(dto.ToDeleteCategoryCommand(), cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Exception Deleting category: {ex}: {id}", ex, id);
            return StatusCode(500);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
    {
        try
        {
            return await _mediator.Send(new GetAllCategories());
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Exception Getting categories {ex}", ex);
            return StatusCode(500);
        }
    }
}