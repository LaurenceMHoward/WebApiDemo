namespace WebApiDemo.Api.Controllers.Base;

using MediatR;

using Microsoft.AspNetCore.Mvc;


    [Route("api/{version:ApiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            this._mediator = mediator;
        }
    }
