using System;

namespace WebApiDemo.Dal.Records.Base;

public abstract class BaseRecord
{
    public Guid? Id { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public int CreatedBy { get; set; }
    public DateTimeOffset? LastUpdatedDate { get; set; }

    public int? LastUpdatedBy { get; set; } = null!;

    public bool IsDeleted { get; set; }
}