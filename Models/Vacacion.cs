using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tienda_Cece.Models;

namespace Tienda_Cece.Models
{
    public class Vacacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Empleado")]
        public int Id_Empleado { get; set; }

        public virtual Empleado? Empleado { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [StringLength(500)]
        public string Motivo { get; set; } = string.Empty;
    }


}
