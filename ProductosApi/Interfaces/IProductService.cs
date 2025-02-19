using ProductosApi.Models;

namespace ProductosApi.Services
{
    public interface IProductService
    {
        Task<(List<Producto> productos, int totalPaginas, int totalRegistros)> GetAllProducts(
   string? search, int page = 1, int pageSize = 10);

        Task<Producto?> GetProductById(int id);
        Task<Producto> CreateProduct(Producto producto);
        Task<bool> UpdateProduct(int id, Producto producto);
        Task<bool> DeleteProduct(int id);
    }
}
