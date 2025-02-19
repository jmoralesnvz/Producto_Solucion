using System;
using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Models
{
    public class Producto
    {
        [Key] // Clave primaria, autogenerada
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock no puede ser negativa.")]
        public int CantidadEnStock { get; set; }

        [Required]
        public DateTime FechaDeCreacion { get; set; } = DateTime.UtcNow; // Autogenerado
    }
}
