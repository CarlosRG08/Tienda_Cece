using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Empleado
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Empleado { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Primer Apellido")]
        [StringLength(100)]
        public string Primer_Apellido { get; set; }

        [Display(Name = "Segundo Apellido")]
        [StringLength(100)]
        public string Segundo_Apellido { get; set; }

        [Required]
        [Display(Name = "Número de Teléfono")]
        [StringLength(20)]
        public string Numero_Telefono { get; set; }

        [ForeignKey("Puesto")]
        public int Id_Puesto { get; set; }

        public Puesto_Trabajo? Puesto { get; set; }
    }
}


