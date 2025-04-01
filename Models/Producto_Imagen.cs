using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Cece.Models
{
    public class Producto_Imagen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_ProductoImg { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int Id_Producto { get; set; }

        [ForeignKey("Id_Producto")]
        public Producto? Producto { get; set; }

        [Display(Name = "Imágen")]
        public byte[]? imagen { get; set; }
        public bool Estado { get; set; }
    }
}
