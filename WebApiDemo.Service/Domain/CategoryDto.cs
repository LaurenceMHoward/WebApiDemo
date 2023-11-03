namespace WebApiDemo.Service.Domain;

public class CategoryDto
{
    public string Category { get; set; } = string.Empty;

    public Guid? Id { get; set; }

    public string SubCategory { get; set; } = string.Empty;
}