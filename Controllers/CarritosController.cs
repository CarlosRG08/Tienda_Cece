using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_Cece.Controllers
{
    public class CarritosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public CarritosController(ApplicationDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Muestra el carrito del usuario actual
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.ID_Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            // Si el carrito no existe, crea uno nuevo
            if (carrito == null)
            {
                carrito = new Carrito { UsuarioId = userId, Items = new List<Item_Carrito>() };
                _context.Carrito.Add(carrito);
                await _context.SaveChangesAsync();
            }

            return View(carrito);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateItemQuantity(int itemId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.ID_Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito != null)
            {
                var item = carrito.Items.FirstOrDefault(i => i.Id_Item == itemId);
                if (item != null)
                {
                    var previousQuantity = item.Cant_Producto;
                    item.Cant_Producto = quantity;
                    carrito.Total += (quantity - previousQuantity) * item.ID_Producto.Precio_Unitario;

                    _context.Carrito.Update(carrito);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        itemTotal = item.Cant_Producto * item.ID_Producto.Precio_Unitario,
                        cartTotal = carrito.Total
                    });
                }
            }

            return BadRequest("No se pudo actualizar el artículo.");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(Dictionary<string, int> quantities)
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.ID_Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito != null)
            {
                foreach (var kvp in quantities)
                {
                    // Verifica que la clave tenga el formato esperado
                    if (kvp.Key.StartsWith("quantity_"))
                    {
                        var itemIdStr = kvp.Key.Replace("quantity_", "");
                        if (int.TryParse(itemIdStr, out int itemId))
                        {
                            var newQuantity = kvp.Value;

                            var item = carrito.Items.FirstOrDefault(i => i.Id_Item == itemId);
                            if (item != null)
                            {
                                var previousQuantity = item.Cant_Producto;
                                item.Cant_Producto = newQuantity;
                                carrito.Total += (newQuantity - previousQuantity) * item.ID_Producto.Precio_Unitario;
                            }
                        }
                    }
                }

                _context.Carrito.Update(carrito);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Elimina un producto específico del carrito o todos los productos
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int itemId = 0)
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.ID_Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito != null)
            {
                if (itemId > 0)
                {
                    // Eliminar un producto específico del carrito
                    var item = carrito.Items.FirstOrDefault(i => i.Id_Item == itemId);
                    if (item != null)
                    {
                        carrito.Items.Remove(item);
                        carrito.Total -= item.Cant_Producto * item.ID_Producto.Precio_Unitario;

                        // Si el carrito queda vacío después de eliminar el producto, vaciar el carrito
                        if (!carrito.Items.Any())
                        {
                            carrito.Total = 0;
                        }
                    }
                }
                else
                {
                    // Eliminar todos los productos del carrito
                    carrito.Items.Clear();
                    carrito.Total = 0;
                }

                // Si el carrito está vacío, eliminar el carrito
                if (!carrito.Items.Any())
                {
                    _context.Carrito.Remove(carrito);
                }
                else
                {
                    _context.Carrito.Update(carrito); // Asegurarse de actualizar el carrito en la base de datos
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Método para obtener la imagen del producto
        [HttpGet]
        public IActionResult GetImage(int id)
        {
            var productoImagen = _context.Producto_Imagenes
                .FirstOrDefault(img => img.Id_ProductoImg == id);

            if (productoImagen != null && productoImagen.imagen != null)
            {
                return File(productoImagen.imagen, "image/jpeg"); // Cambia "image/jpeg" si es necesario para otros tipos de imagen
            }

            return File("~/img/icon-image-not-found.jpg", "image/jpeg");
        }
    }
}
