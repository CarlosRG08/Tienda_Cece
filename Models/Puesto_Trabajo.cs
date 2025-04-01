using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Puesto_Trabajo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_puesto { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre puesto")]
        public string nombre_puesto { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Descripción")]
        public string descripcion_puesto { get; set; }

        public bool Estado { get; set; }
    }
}
