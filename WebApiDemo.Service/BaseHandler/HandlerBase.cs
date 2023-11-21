using MediatR;

namespace WebApiDemo.Service.BaseHandler;

using Dal.Context;

public abstract class HandlerBase<TRequest, TResponse>
    (IWebApiDemoDbContext context) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IWebApiDemoDbContext _webApiDemoDbContext = context;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}