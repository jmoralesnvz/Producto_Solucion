using Microsoft.AspNetCore.Mvc;
using ProductosApi.Models;
using ProductosApi.Services;

namespace ProductosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene todos los productos con filtros opcionales
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            // Llamar al servicio con paginación y búsqueda
            var (productos, totalPaginas, totalRegistros) = await _productService.GetAllProducts(search, page, pageSize);

            return Ok(new
            {
                products = productos,
                currentPage = page,
                totalPages = totalPaginas,
                totalItems = totalRegistros
            });
        }


        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound("Producto no encontrado");

            return Ok(product);
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newProduct = await _productService.CreateProduct(producto);
            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _productService.UpdateProduct(id, producto);
            if (!updated)
                return NotFound("Producto no encontrado");

            return NoContent();
        }

        /// <summary>
        /// Elimina un producto por ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Validar que el ID sea mayor que 0
            if (id <= 0)
                return BadRequest("El ID debe ser un número positivo.");

            var deleted = await _productService.DeleteProduct(id);

            if (!deleted)
                return NotFound("Producto no encontrado");

            return NoContent();
        }

    }
}
