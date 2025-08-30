using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Reference;

using Microsoft.EntityFrameworkCore;
using WebApiDemo.Dal.Context;
using WebApiDemo.Dal.Records;

public class TestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private static readonly ServiceProvider EfInMemoryProvider =
        new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

    private readonly CategoryRecord[] _categories =
    [
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestA", SubCategory = "TestB" },
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestC", SubCategory = "TestD" },
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestE", SubCategory = "TestF" },
        new CategoryRecord()
        {
            Id = Guid.NewGuid(), Category = "TestG", SubCategory = "TestH", IsDeleted = true
        } // this should save as IsDeleted = false
    ];

    public int CategoryCount() => _categories.Length;
    public CategoryRecord GetFirstCategory() => _categories[0];

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // Remove app registrations
            var registrations = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<WebApiDemoDbContext>) ||
                    d.ServiceType == typeof(IWebApiDemoDbContext) ||
                    d.ServiceType == typeof(WebApiDemoDbContext))
                .ToList();
            foreach (var d in registrations)
            {
                services.Remove(d);
            }

            // Register InMemory DbContext using an isolated EF internal provider
            var dbName = $"InMemoryDbForTesting-{Guid.NewGuid()}";
            services.AddDbContext<WebApiDemoDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.UseInternalServiceProvider(EfInMemoryProvider);
            });

            // Map interface to concrete
            services.AddScoped<IWebApiDemoDbContext>(sp => sp.GetRequiredService<WebApiDemoDbContext>());

            // Seed
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WebApiDemoDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Set<CategoryRecord>().AddRange(_categories);
            db.SaveChanges();
            db.ChangeTracker.Clear();
        });
    }
}