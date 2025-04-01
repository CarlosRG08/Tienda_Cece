using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Correo Eletrónico")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }
    }
}
