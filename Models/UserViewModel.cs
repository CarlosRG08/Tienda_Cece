using System.ComponentModel.DataAnnotations;

namespace Tienda_Cece.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public byte[]? ProfilePicture { get; set; }

    }

}
