namespace WebApiDemo.Dal.Records;

using WebApiDemo.Dal.Records.Base;

public class CategoryRecord : BaseRecord
{
    public string Category { get; set; } = string.Empty;

    public string SubCategory { get; set; } = string.Empty;
}