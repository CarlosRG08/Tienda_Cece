using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Carrito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Carrito { get; set; }

        public String? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }

        [Required]
        [ForeignKey("Id_Producto")]
        [Display(Name = "Producto")]
        public int Id_Producto { get; set; }
        public Producto? Producto { get; set; }

        public virtual List<Item_Carrito> Items { get; set; }

        public string? CarritoTemporalId { get; set; }

        [Required]
        [Display(Name = "Total")]
        public double Total { get; set; }

    }
}



