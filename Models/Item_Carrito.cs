using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Item_Carrito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Item { get; set; } // PK

        [Required]
        [Display(Name = "ID Carrito")]
        public int Id_Carrito { get; set; } // FK

        [Required]
        [Display(Name = "ID Producto")]
        public int Id_Producto { get; set; } // FK

        [Required]
        [Display(Name = "Cantidad de Producto")]
        public int Cant_Producto { get; set; }

        [ForeignKey("Id_Carrito")]
        public Carrito? ID_Carrito { get; set; }

        [ForeignKey("Id_Producto")]
        public Producto? ID_Producto { get; set; }

    }


}
