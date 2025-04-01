using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Producto { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int? Id_Categoria { get; set; }

        [ForeignKey("Id_Categoria")]
        public Categoria? Categoria { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre del Producto")]
        public string Nombre_Producto { get; set; }

        [Required]
        [Display(Name = "Descripción del Producto")]
        public string Descripcion_Producto { get; set; }

        [Required]
        [Display(Name = "Precio Unitario")]
        public double Precio_Unitario { get; set; }

        [Required]
        [Display(Name = "Cantidad en Stock")]
        public int Cantidad_Stock { get; set; }

        public bool EnPromocion { get; set; }
        public bool EnTendencia { get; set; }
        public bool Estado { get; set; }

        //Lista de imágenes del producto
        public ICollection<Producto_Imagen>? Producto_Imagenes { get; set; }
    }
}


