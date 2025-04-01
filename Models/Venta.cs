using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tienda_Cece.Models;

namespace Tienda_Cece.Models
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_venta { get; set; }

        public string? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Users? Usuario { get; set; }

        [Display(Name = "Producto")]
        public int Id_Producto { get; set; }

        [ForeignKey("Id_Producto")]
        public Producto? Producto { get; set; }

        [Required]
        [Display(Name = "Precio Unitario")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio_Unitario { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad_Producto { get; set; }

        [Required]
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total
        {
            get { return Precio_Unitario * Cantidad_Producto; }
            private set { }
        }

        public bool Estado { get; set; }
    }
}