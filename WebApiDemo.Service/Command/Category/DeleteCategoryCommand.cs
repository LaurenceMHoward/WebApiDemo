using MediatR;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Command.Category;

public class DeleteCategoryCommand : IRequest<CategoryDto>
{
    public string Category { get; set; } = default!;

    public Guid? Id { get; set; }

    public string SubCategory { get; set; } = default!;
}