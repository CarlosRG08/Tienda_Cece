using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Comprobante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Comprobante { get; set; }

        [Column(Order = 1)]
        [ForeignKey("Carrito")]
        public int Id_Carrito { get; set; }
        public Carrito Carrito { get; set; }

        public String? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }

        [Required]
        [Display(Name = "Producto")]
        [ForeignKey("Producto")]
        public int Id_Producto { get; set; }
        public Producto? Producto { get; set; }

        [Required]
        [Display(Name = "Total")]
        public double Total { get; set; }

        [Required]
        [Display(Name = "Precio Unitario")]
        public double Precio_Unitario { get; set; }

        [Required]
        [Display(Name = "Cantidad de Producto")]
        public int Cantidad_Producto { get; set; }


        public Venta? Venta { get; set; }
    }

}



