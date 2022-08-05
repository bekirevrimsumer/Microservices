using Catalog.API.Dtos;
using Catalog.API.Models;
using Shared.Dtos;

namespace Catalog.API.Interfaces;

public interface ICategoryService
{
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(string id);
    Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
}
