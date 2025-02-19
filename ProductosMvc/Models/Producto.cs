using System.ComponentModel.DataAnnotations;


namespace ProductosMvc.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int CantidadEnStock { get; set; }

        public DateTime FechaDeCreacion { get; set; } = DateTime.UtcNow;
    }
}
