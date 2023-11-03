namespace WebApiDemo.Dal.Context.EntityConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Records;
using WebApiDemo.Dal.Context.EntityConfiguration.EntityBase;

internal class CategoryConfiguration : BaseConfiguration<CategoryRecord>
{
    private const string _tableName = "Category";
    private const string _tableNames = "Categories";

    public override void Configure(EntityTypeBuilder<CategoryRecord> builder)
    {
        builder.ToTable(_tableNames);
        base.Configure(builder);

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id).HasColumnName($"{_tableName}ID");

        builder.Property(p => p.Category).HasColumnType("nvarchar").HasMaxLength(25).IsRequired();
        builder.Property(p => p.SubCategory).HasColumnType("nvarchar").HasMaxLength(25).IsRequired();
    }
}