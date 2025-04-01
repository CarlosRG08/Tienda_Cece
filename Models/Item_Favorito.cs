using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Cece.Models
{
    public class Item_Favorito
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Item { get; set; }

        [Required]
        public int Id_Favorito { get; set; }

        [ForeignKey("Id_Favorito")]
        public virtual Lista_Favorito ListaFavorito { get; set; }

        [Required]
        public int Id_Producto { get; set; }

        [ForeignKey("Id_Producto")]
        public virtual Producto Producto { get; set; }


    }


}
