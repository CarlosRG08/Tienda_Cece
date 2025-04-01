using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Comprobante> Comprobantes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Rebajo_Empleado> Rebajo_Empleados { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Producto_Imagen> Producto_Imagenes { get; set; }
        public DbSet<Item_Carrito> Items_Carrito { get; set; }
        public DbSet<Resenia> Reseñas { get; set; }
        public DbSet<Puesto_Trabajo> Puestos_de_Trabajo { get; set; }
        public DbSet<Lista_Favorito> Lista_favoritos { get; set; }
        public DbSet<Item_Favorito> Items_Favoritos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Incapacidad> Incapacidades { get; set; }
        public DbSet<Vacacion> Vacaciones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(x => x.Id);

            // Configuración adicional para Lista_Favorito y Item_Favorito
            modelBuilder.Entity<Lista_Favorito>()
                .HasMany(lf => lf.ItemsFav)
                .WithOne(ifav => ifav.ListaFavorito)
                .HasForeignKey(ifav => ifav.Id_Favorito);

            modelBuilder.Entity<Item_Favorito>()
                .HasOne(ifav => ifav.Producto)
                .WithMany()
                .HasForeignKey(ifav => ifav.Id_Producto);

            // Configuración adicional para Producto y Producto_Imagen
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Producto_Imagenes)
                .WithOne(pi => pi.Producto)
                .HasForeignKey(pi => pi.Id_Producto);

            // Configuración adicional para Carrito e Item_Carrito
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Items)
                .WithOne(i => i.ID_Carrito)
                .HasForeignKey(i => i.Id_Carrito);

            modelBuilder.Entity<Item_Carrito>()
                .HasOne(i => i.ID_Producto)
                .WithMany()
                .HasForeignKey(i => i.Id_Producto);
        }
        public DbSet<Tienda_Cece.Models.Incapacidad> Incapacidad { get; set; } = default!;

    }
}
