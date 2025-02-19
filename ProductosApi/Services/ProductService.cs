using Microsoft.EntityFrameworkCore;
using ProductosApi.Models;
using ProductosApi.Models.Context;

namespace ProductosApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los productos, permite filtrar por nombre y rango de precios
        /// </summary>
        public async Task<(List<Producto> productos, int totalPaginas, int totalRegistros)> GetAllProducts(
    string? search, int page = 1, int pageSize = 10)
        {
            var query = _context.Productos.AsQueryable();

            // Filtrar por búsqueda (Nombre o Precio)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Nombre.Contains(search) ||
                                         p.Precio.ToString().Contains(search)); // Buscar también en el precio
            }

            // Contar registros totales antes de paginar
            int totalRegistros = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            // Aplicar paginación
            var productos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (productos, totalPaginas, totalRegistros);
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public async Task<Producto?> GetProductById(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        /// <summary>
        /// Crea un nuevo producto en la base de datos
        /// </summary>
        public async Task<Producto> CreateProduct(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        public async Task<bool> UpdateProduct(int id, Producto producto)
        {
            var existingProduct = await _context.Productos.FindAsync(id);
            if (existingProduct == null)
                return false;

            existingProduct.Nombre = producto.Nombre;
            existingProduct.Descripcion = producto.Descripcion;
            existingProduct.Precio = producto.Precio;
            existingProduct.CantidadEnStock = producto.CantidadEnStock;

            _context.Productos.Update(existingProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Elimina un producto por ID
        /// </summary>
        public async Task<bool> DeleteProduct(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
