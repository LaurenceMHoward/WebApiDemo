using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Reference;

using Microsoft.EntityFrameworkCore;
using WebApiDemo.Dal.Context;
using WebApiDemo.Dal.Records;

public class TestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
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

    public int CategoryCount()
    {
        return _categories.Length;
    }

    public CategoryRecord GetFirstCategory()
    {
        return _categories[0];
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove the original
            ServiceDescriptor? descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WebApiDemoDbContext>));
            services.Remove(descriptor!);

            // add the new one
            services.AddDbContext<WebApiDemoDbContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));

            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedService = scope.ServiceProvider;
            WebApiDemoDbContext db = scopedService.GetRequiredService<WebApiDemoDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Set<CategoryRecord>().AddRange(_categories);
            db.SaveChangesAsync();
            db.ChangeTracker.Clear();
        });
    }
}