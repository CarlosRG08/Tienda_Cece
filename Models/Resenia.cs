using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Resenia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Users? Usuario { get; set; }

        [Required(ErrorMessage = "La reseña es obligatoria.")]
        [StringLength(250, ErrorMessage = "La reseña no puede tener más de 250 caracteres.")]
        [Display(Name = "Reseña")]
        public string resenia { get; set; }

    }
}
