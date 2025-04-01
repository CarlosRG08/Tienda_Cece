using Microsoft.AspNetCore.Identity;

namespace Tienda_Cece.Models
{
    public class Users : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool EstadoCuenta { get; set; }
    }
}
