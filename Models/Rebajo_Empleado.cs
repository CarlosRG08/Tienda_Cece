using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Rebajo_Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Id_Empleado")]
        [Display(Name = "Empleado")]
        public int Id_Empleado { get; set; }
        public Empleado? Proveedor { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo de Salida")]
        public string Tipo_salida { get; set; }

        [Required]
        [Display(Name = "Fecha de Inicio")]
        public DateTime Fecha_inicio { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime? Fecha_Final { get; set; }




        public Empleado? Empleado { get; set; }
    }
}

