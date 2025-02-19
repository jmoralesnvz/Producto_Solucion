using Microsoft.AspNetCore.Mvc;
using ProductosMvc.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProductosMvc.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7170/api/Product"; // Asegúrate de que el puerto sea correcto

        public ProductosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 🔹 Obtener todos los productos desde la API
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexData(string? search, int page = 1, int pageSize = 10)
        {
            // Construir la URL con parámetros
            string url = $"{_apiUrl}?search={search}&page={page}&pageSize={pageSize}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Error al cargar los productos." });

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<PaginacionViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Json(new { success = true, products = data.Products, currentPage = data.CurrentPage, totalPages = data.TotalPages });
        }


        // 🔹 Obtener detalles de un producto
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var producto = JsonSerializer.Deserialize<Producto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(producto);
        }

        // 🔹 Formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // 🔹 Enviar un producto a la API para crearlo
        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Verifica los datos ingresados." });

            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (!response.IsSuccessStatusCode)
                return Json(new { success = false, message = "Error al guardar el producto en la API." });

            return Json(new { success = true, message = "Producto agregado correctamente." });
        }

        // 🔹 Formulario de edición de producto
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var producto = JsonSerializer.Deserialize<Producto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(producto);
        }

        // 🔹 Actualizar un producto en la API
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Verifica los datos ingresados." });
            }

            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Error al actualizar el producto en la API." });
            }

            return Json(new { success = true, message = "Producto actualizado correctamente." });
        }


        // 🔹 Confirmación para eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var producto = JsonSerializer.Deserialize<Producto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(producto);
        }

        // 🔹 Eliminar producto en la API
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Error al eliminar el producto. Intenta nuevamente." });
            }

            return Json(new { success = true, message = "Producto eliminado correctamente." });
        }

    }
}


public class PaginacionViewModel
{
    public List<Producto> Products { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public string? Search { get; set; }
}