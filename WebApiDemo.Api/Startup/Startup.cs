using Microsoft.EntityFrameworkCore;
using WebApiDemo.Api.Validators.Validation;
using WebApiDemo.Dal.Context;
using WebApiDemo.Service.Query.Categories;

namespace WebApiDemo.Api.Startup;

using FluentValidation;
using Microsoft.Extensions.Configuration;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly string _webApiDemoDbConfig = "WebApiDemoDb";

    public Startup(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(ep => { ep.MapControllers(); });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.Configure<RouteOptions>(e => e.LowercaseUrls = true);

        services.AddControllers();
        services.AddApiVersioning();

        services.AddEndpointsApiExplorer();
        services.AddResponseCaching();

        services.AddValidatorsFromAssemblyContaining<CategoryValidator>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(GetAllCategoriesHandler).Assembly));

        services.AddDbContext<IWebApiDemoDbContext, WebApiDemoDbContext>(opt =>
        {
            opt.UseSqlServer(_configuration.GetConnectionString(_webApiDemoDbConfig));
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });
        services.AddSwaggerGen();
    }
}