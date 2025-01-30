using CrudApp.Models;

namespace CrudApp.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(ProductDto productDto);
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(int id);
    }
}
