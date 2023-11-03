using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApiDemo.Dal.Context.EntityConfiguration.EntityBase;

using Records.Base;

internal abstract class BaseConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseRecord
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.CreatedDate)
            .HasColumnType("datetimeoffset(7)");

        builder.Property(p => p.LastUpdatedDate)
            .HasColumnType("datetimeoffset(7)");

        builder.Property(p => p.IsDeleted).HasColumnType("bit");
        builder.Property(p => p.LastUpdatedBy).HasColumnType("int");
        builder.Property(p => p.CreatedBy).HasColumnType("int");
    }
}