
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tienda_Cece.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_pedido { get; set; }

        [ForeignKey("id_usuario")]
        public int id_usuario { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }

        public Carrito? Carrito { get; set; }

    }
}
