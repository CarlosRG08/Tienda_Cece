
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_proveedor { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Nombre proveedor")]
        public string nom_proveedor { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Sitio web")]
        public string website { get; set; }

        public Producto? Producto { get; set; }

        [Required]
        public bool Estado { get; set; } 
    }
}
