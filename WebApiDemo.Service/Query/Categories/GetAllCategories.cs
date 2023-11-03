using MediatR;
using WebApiDemo.Service.Domain;

namespace WebApiDemo.Service.Query.Categories;

public class GetAllCategories : IRequest<List<CategoryDto>>
{
}