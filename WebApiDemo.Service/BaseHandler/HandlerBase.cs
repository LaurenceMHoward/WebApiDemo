using MediatR;

namespace WebApiDemo.Service.BaseHandler;

using Dal.Context;

public abstract class HandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IWebApiDemoDbContext _webApiDemoDbContext;

    protected HandlerBase(IWebApiDemoDbContext context)
    {
        this._webApiDemoDbContext = context;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}