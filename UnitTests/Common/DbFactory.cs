using WebApiDemo.Dal.Records;

namespace UnitTests.Common;

using Bogus;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Dal.Context;

public class DbFactory
{
    private static readonly CategoryRecord[] s_categories =
    {
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestA", SubCategory = "TestB" },
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestC", SubCategory = "TestD" },
        new CategoryRecord() { Id = Guid.NewGuid(), Category = "TestE", SubCategory = "TestF" },
        new CategoryRecord()
        {
            Id = Guid.NewGuid(), Category = "TestG", SubCategory = "TestH", IsDeleted = true
        } // this should save as IsDeleted = false
    };

    public static CategoryRecord GetFirstCategory()
    {
        return s_categories[0];
    }

    public static WebApiDemoDbContext GetDbContext(string named = "Tests")
    {
        var options = new DbContextOptionsBuilder<WebApiDemoDbContext>()
            .UseInMemoryDatabase(named).Options;
        var db = new WebApiDemoDbContext(options);
        db.Set<CategoryRecord>().AddRange(s_categories);
        db.SaveChangesAsync();

        return db;
    }
}