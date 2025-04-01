
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cliente { get; set; }
        public String? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }



        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Primer apellido")]
        public string primer_apellido { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Segundo apellido")]
        public string segundo_apellido { get; set; }

        [Required]
        [Display(Name = "Número de teléfono")]
        public string numero_telefono { get; set; }
    }
}
