using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Compra
    {
        [Key]
        public int Id_Compra { get; set; }

        [ForeignKey("Proveedor")]
        public int Id_Proveedor { get; set; }

        public Proveedor? Proveedor { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre_Producto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int Cant_Producto { get; set; }

        public string? link_Producto { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}