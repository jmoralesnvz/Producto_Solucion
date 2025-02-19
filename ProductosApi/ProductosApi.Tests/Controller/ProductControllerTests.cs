using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductosApi.Controllers;
using ProductosApi.Models;
using ProductosApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductosApi.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object);
        }

        // 🔹 1. Prueba para GET /api/products
        [Fact]
        public async Task Get_ReturnsProductsList()
        {
            // Arrange (Preparación)
            var fakeProducts = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Producto 1", Precio = 100 },
                new Producto { Id = 2, Nombre = "Producto 2", Precio = 200 }
            };

            _productServiceMock
                .Setup(s => s.GetAllProducts(null, 1, 10))
                .ReturnsAsync((fakeProducts, 1, 2)); // Simula la respuesta del servicio

            // Act (Ejecutar)
            var result = await _controller.Get(null, 1, 10);

            // Assert (Verificar)
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult.Value as dynamic;

            Assert.NotNull(responseData);
            //Assert.Equal(2, responseData["totalItems"]);
            //Assert.Equal(1, responseData.currentPage);
            //Assert.Single(responseData.products);
        }

        // 🔹 2. Prueba para GET /api/products/{id}
        [Fact]
        public async Task Get_WithValidId_ReturnsProduct()
        {
            // Arrange
            var fakeProduct = new Producto { Id = 1, Nombre = "Producto Test", Precio = 500 };

            _productServiceMock
                .Setup(s => s.GetProductById(1))
                .ReturnsAsync(fakeProduct);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Producto>(okResult.Value);
            Assert.Equal(1, product.Id);
            Assert.Equal("Producto Test", product.Nombre);
        }

        // 🔹 3. Prueba para POST /api/products
        [Fact]
        public async Task Post_ValidProduct_ReturnsCreated()
        {
            // Arrange
            var newProduct = new Producto { Id = 3, Nombre = "Nuevo Producto", Precio = 300 };

            _productServiceMock
                .Setup(s => s.CreateProduct(It.IsAny<Producto>()))
                .ReturnsAsync(newProduct);

            // Act
            var result = await _controller.Post(newProduct);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdProduct = Assert.IsType<Producto>(createdResult.Value);
            Assert.Equal(3, createdProduct.Id);
        }

        // 🔹 4. Prueba para DELETE /api/products/{id}
        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _productServiceMock
                .Setup(s => s.DeleteProduct(1))
                .ReturnsAsync(true); // Simular que el producto fue eliminado correctamente

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _productServiceMock
                .Setup(s => s.DeleteProduct(99))
                .ReturnsAsync(false); // Simula que el producto no existe

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
