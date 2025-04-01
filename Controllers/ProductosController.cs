using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_Cece.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public ProductosController(ApplicationDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /*Funciones admin*/
        // GET: /Productos
        public IActionResult Index()
        {
            var productos = _context.Productos.Include(p => p.Categoria).ToList();
            LoadCategorias();
            return View(productos);
        }

        // GET: /Productos/Create
        public IActionResult Create()
        {
            LoadCategorias();
            return View();
        }

        // POST: /Productos/Create
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            LoadCategorias();
            return View(producto);
        }

        // GET: /Productos/Edit/{id}
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            LoadCategorias();
            return View(producto);
        }

        // POST: /Productos/Edit
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Update(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            LoadCategorias();
            return View(producto);
        }

        // POST: /Productos/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Productos/Get/{id}
        [HttpGet]
        public IActionResult Get(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                return Json(producto);
            }
            return NotFound();
        }


        // POST: /Productos/ToggleStatus
        [HttpPost]
        public IActionResult ToggleStatus(int Id_Producto)
        {
            var producto = _context.Productos.Find(Id_Producto);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Estado = !producto.Estado;
            _context.Productos.Update(producto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Productos/alertas
        [HttpGet]
        public IActionResult GetHighStockProducts()
        {
            var highStockProducts = _context.Productos
                .Where(p => p.Cantidad_Stock > 15)
                .Select(p => new
                {
                    nombre_Producto = p.Nombre_Producto,
                    cantidad_Stock = p.Cantidad_Stock
                })
                .ToList();

            return Json(highStockProducts);
        }

        // GET: /Productos/alertas
        [HttpGet]
        public IActionResult GetLowStockProducts()
        {
            var lowStockProducts = _context.Productos
                .Where(p => p.Cantidad_Stock < 5)
                .Select(p => new
                {
                    nombre_Producto = p.Nombre_Producto,
                    cantidad_Stock = p.Cantidad_Stock
                })
                .ToList();

            return Json(lowStockProducts);
        }

        /*Funciones usuario normal*/
        // GET: /Productos
        public async Task<IActionResult> Productos(List<string> categorias, string sortOrder)
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Producto_Imagenes)
                .AsQueryable();

            // Filtro por categorías
            if (categorias != null && categorias.Any())
            {
                productos = productos.Where(p => categorias.Contains(p.Categoria.Nombre_Categoria));
            }

            // Ordenar por precio
            switch (sortOrder)
            {
                case "price_asc":
                    productos = productos.OrderBy(p => p.Precio_Unitario);
                    break;
                case "price_desc":
                    productos = productos.OrderByDescending(p => p.Precio_Unitario);
                    break;
                default:
                    productos = productos.OrderBy(p => p.Nombre_Producto); // Orden por defecto
                    break;
            }

            var productosList = await productos.ToListAsync();

            var userId = _userManager.GetUserId(User);
            var favoriteProductIds = new List<int>();

            if (userId != null)
            {
                favoriteProductIds = await _context.Lista_favoritos
                    .Where(l => l.UsuarioId == userId)
                    .SelectMany(l => l.ItemsFav)
                    .Select(i => i.Id_Producto)
                    .ToListAsync();
            }

            ViewBag.FavoriteProductIds = favoriteProductIds;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(productosList);
        }

        // GET: /Productos/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Producto_Imagenes)
                .FirstOrDefaultAsync(m => m.Id_Producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            // Obtener los IDs de los productos favoritos del usuario actual
            List<int> favoriteProductIds = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                favoriteProductIds = await _context.Lista_favoritos
                    .Where(l => l.UsuarioId == userId)
                    .SelectMany(l => l.ItemsFav)
                    .Select(i => i.Id_Producto)
                    .ToListAsync();
            }

            // Pasar los IDs de los productos favoritos al ViewBag
            ViewBag.FavoriteProductIds = favoriteProductIds;

            return View(producto);
        }

        // GET: /Productos/Search
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>());
            }

            var products = await _context.Productos
                .Include(p => p.Producto_Imagenes)
                .Where(p => p.Nombre_Producto.Contains(query) || p.Descripcion_Producto.Contains(query))
                .Select(p => new
                {
                    id = p.Id_Producto,
                    name = p.Nombre_Producto,
                    description = p.Descripcion_Producto,
                    price = p.Precio_Unitario,
                    imageUrl = p.Producto_Imagenes.Any() ? Url.Action("GetImage", "Producto_Imagen", new { id = p.Producto_Imagenes.First().Id_ProductoImg }) : Url.Content("~/img/icon-image-not-found.jpg")
                })
                .ToListAsync();

            return Json(products);
        }

        // GET: /Productos/promociones// PROMOCIONES
        public async Task<IActionResult> Promociones()
        {
            var productosEnPromocion = await _context.Productos
                .Include(p => p.Producto_Imagenes) // Incluir las imágenes de los productos
                .Where(p => p.EnPromocion)
                .Select(p => new
                {
                    id = p.Id_Producto,
                    name = p.Nombre_Producto,
                    description = p.Descripcion_Producto,
                    price = p.Precio_Unitario,
                    imageUrl = p.Producto_Imagenes.Any()
                        ? Url.Action("GetImage", "Producto_Imagen", new { id = p.Producto_Imagenes.First().Id_ProductoImg })
                        : Url.Content("~/img/icon-image-not-found.jpg")
                })
                .ToListAsync();

            var userId = _userManager.GetUserId(User);
            var favoriteProductIds = new List<int>();

            if (userId != null)
            {
                favoriteProductIds = await _context.Lista_favoritos
                    .Where(l => l.UsuarioId == userId)
                    .SelectMany(l => l.ItemsFav)
                    .Select(i => i.Id_Producto)
                    .ToListAsync();
            }

            ViewBag.FavoriteProductIds = favoriteProductIds;

            return View(productosEnPromocion);
        }

        // GET: /Productos/TENDENCIAS
        public async Task<IActionResult> Tendencias()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var favoriteProductIds = await _context.Lista_favoritos
                    .Where(l => l.UsuarioId == userId)
                    .SelectMany(l => l.ItemsFav)
                    .Select(i => i.Id_Producto)
                    .ToListAsync();

                ViewBag.FavoriteProductIds = favoriteProductIds;
            }
            else
            {
                ViewBag.FavoriteProductIds = new List<int>();
            }

            var productosEnTendencia = _context.Productos
                .Include(p => p.Producto_Imagenes) // Incluir las imágenes de los productos
                .Where(p => p.EnTendencia)
                .Select(p => new
                {
                    id = p.Id_Producto,
                    name = p.Nombre_Producto,
                    description = p.Descripcion_Producto,
                    price = p.Precio_Unitario,
                    imageUrl = p.Producto_Imagenes.Any()
                        ? Url.Action("GetImage", "Producto_Imagen", new { id = p.Producto_Imagenes.First().Id_ProductoImg })
                        : Url.Content("~/img/icon-image-not-found.jpg")
                })
                .ToList();

            return View(productosEnTendencia);
        }

        private void LoadCategorias()
        {
            var categorias = _context.Categorias
                .Where(c => c.Estado) // Filtrar categorías activas
                .Select(c => new { c.Id_Categoria, c.Nombre_Categoria })
                .ToList();

            ViewBag.Categorias = new SelectList(categorias, "Id_Categoria", "Nombre_Categoria");
        }

        //AGREGAR PRODUCTOS AL CARRITO
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            if (quantity > producto.Cantidad_Stock)
            {
                return Json(new { success = false, message = $"Sólo hay {producto.Cantidad_Stock} unidades disponibles en stock." });
            }

            var userId = _userManager.GetUserId(User);
            var carrito = await _context.Carrito
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito == null)
            {
                carrito = new Carrito { UsuarioId = userId, Items = new List<Item_Carrito>() };
                _context.Carrito.Add(carrito);
            }

            var item = carrito.Items.FirstOrDefault(i => i.Id_Producto == id);
            if (item == null)
            {
                item = new Item_Carrito { Id_Producto = id, Cant_Producto = quantity };
                carrito.Items.Add(item);
            }
            else
            {
                item.Cant_Producto += quantity;
            }

            carrito.Total += producto.Precio_Unitario * quantity; // Asegúrate de que todo aquí es double
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
