namespace WebApiDemo.Api.Controllers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using Service.Domain;
using Service.Mapping;
using Service.Query.Categories;
using Validators.Validation;
using WebApiDemo.Api.Controllers.Base;

[ApiVersion("1.0")]
public class CategoryController(IMediator mediator, CategoryValidator categoryDbValidator) : BaseController(mediator)
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
            await DatabaseValidation(item, cancellationToken);

            if (ModelState.IsValid)
            {
                item = await _mediator.Send(item.ToCategoryCommand(), cancellationToken);
                return this.Ok(item);
            }

            return this.BadRequest(GetErrorsMessages());
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Exception Saving Category", ex, item);
            return this.StatusCode(500);
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
            CategoryDto item = new()
            {
                Id = id
            };
            if (!item.Id.HasValue)
            {
                return BadRequest("No Identifier specified");
            }

            item = await _mediator.Send(item.ToDeleteCategoryCommand(), cancellationToken);
            return Ok(item);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Exception Deleting category", ex, id);
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
            Log.Logger.Error("Exception Getting categories", ex);
            return StatusCode(500);
        }
    }

    private async Task DatabaseValidation(CategoryDto item, CancellationToken token)
    {
        ValidationResult? validation = await categoryDbValidator.ValidateAsync(item, token);
        validation.AddToModelState(ModelState, "database");
    }

    private List<string> GetErrorsMessages()
    {
        List<string> errors = [];
        foreach (ModelStateEntry item in ModelState.Values)
        {
            foreach (ModelError error in item.Errors)
            {
                errors.Add(error.ErrorMessage);
            }
        }

        return errors;
    }
}