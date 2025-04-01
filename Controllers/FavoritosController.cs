using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tienda_Cece.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public FavoritosController(ApplicationDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                if (userId == null)
                {
                    return Json(new { success = false, message = "Usuario no autenticado." });
                }

                var listaFavorito = await _context.Lista_favoritos
                    .Include(l => l.ItemsFav)
                    .FirstOrDefaultAsync(l => l.UsuarioId == userId);

                if (listaFavorito == null)
                {
                    listaFavorito = new Lista_Favorito
                    {
                        UsuarioId = userId,
                        ItemsFav = new List<Item_Favorito>()
                    };
                    _context.Lista_favoritos.Add(listaFavorito);
                }

                var itemFavorito = listaFavorito.ItemsFav
                    .FirstOrDefault(i => i.Id_Producto == productId);

                bool isFavorite;

                if (itemFavorito == null)
                {
                    itemFavorito = new Item_Favorito
                    {
                        Id_Producto = productId,
                        ListaFavorito = listaFavorito
                    };
                    listaFavorito.ItemsFav.Add(itemFavorito);
                    isFavorite = true;
                }
                else
                {
                    listaFavorito.ItemsFav.Remove(itemFavorito);
                    _context.Items_Favoritos.Remove(itemFavorito);
                    isFavorite = false;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, isFavorite });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Ocurrió un error al procesar la solicitud." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Favoritos()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Obtener la lista de productos favoritos junto con sus imágenes
            var favoritos = await _context.Lista_favoritos
                .Where(l => l.UsuarioId == userId)
                .Include(l => l.ItemsFav)
                .ThenInclude(i => i.Producto)
                .ThenInclude(p => p.Producto_Imagenes)
                .SelectMany(l => l.ItemsFav)
                .Select(i => i.Producto)
                .ToListAsync();

            // Obtener los IDs de los productos favoritos
            var favoriteProductIds = favoritos.Select(p => p.Id_Producto).ToList();

            ViewBag.FavoriteProductIds = favoriteProductIds;

            return View(favoritos);
        }


        [HttpGet]
        public async Task<IActionResult> GetFavoriteProductIds()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Json(new List<int>());
            }

            var favoriteProductIds = await _context.Lista_favoritos
                .Where(l => l.UsuarioId == userId)
                .SelectMany(l => l.ItemsFav)
                .Select(i => i.Id_Producto)
                .ToListAsync();

            return Json(favoriteProductIds);
        }
    }
}
