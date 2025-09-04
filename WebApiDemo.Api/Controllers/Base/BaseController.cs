namespace WebApiDemo.Api.Controllers.Base;

using MediatR;

using Microsoft.AspNetCore.Mvc;


    [Route("api/{version:ApiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseController(IMediator mediator) : ControllerBase
    {
        protected readonly IMediator Mediator = mediator;
    }
