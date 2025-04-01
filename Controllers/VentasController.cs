using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_Cece.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public VentasController(ApplicationDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Ventas
        public IActionResult Index()
        {
            var ventas = _context.Ventas
                .Include(v => v.Producto)
                .Include(v => v.Usuario)
                .ToList();

            LoadViewData();
            return View(ventas);
        }

        // GET: /Ventas/Create
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // POST: /Ventas/Create
        [HttpPost]
        public IActionResult Create(Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Ventas.Add(venta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadViewData();
            return View(venta);
        }

        // GET: /Ventas/Edit/{id}
        public IActionResult Edit(int id)
        {
            var venta = _context.Ventas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }

            LoadViewData();
            return View(venta);
        }

        // POST: /Ventas/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Venta venta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Buscamos la venta existente para actualizar sólo los campos modificables
                    var existingVenta = await _context.Ventas.FindAsync(venta.Id_venta);
                    if (existingVenta == null)
                    {
                        return NotFound();
                    }

                    // Actualizamos los campos permitidos
                    existingVenta.UsuarioId = venta.UsuarioId;
                    existingVenta.Id_Producto = venta.Id_Producto;
                    existingVenta.Precio_Unitario = venta.Precio_Unitario;
                    existingVenta.Cantidad_Producto = venta.Cantidad_Producto;

                    // Guardamos los cambios
                    _context.Update(existingVenta);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id_venta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            LoadViewData();
            return View(venta);
        }

        // GET: /Ventas/Get/{id}
        [HttpGet]
        public IActionResult Get(int id)
        {
            var venta = _context.Ventas
                .Where(v => v.Id_venta == id)
                .Select(v => new
                {
                    v.Id_venta,
                    v.UsuarioId,
                    v.Id_Producto,
                    v.Precio_Unitario,
                    v.Cantidad_Producto,
                    v.Total
                })
                .FirstOrDefault();

            if (venta == null)
            {
                return NotFound();
            }

            return Json(venta);
        }


        // POST: /Compras/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var venta = _context.Ventas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }

            _context.Ventas.Remove(venta);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id_venta == id);
        }

        private void LoadViewData()
        {
            var productos = _context.Productos
               .Where(c => c.Estado) // Filtrar categorías activas
               .Select(c => new { c.Id_Producto, c.Nombre_Producto })
               .ToList();

            ViewBag.Productos = new SelectList(productos, "Id_Producto", "Nombre_Producto");

            var categorias = _context.Users
               .Where(c => c.EstadoCuenta) // Filtrar categorías activas
               .Select(c => new { c.Id, c.UserName })
               .ToList();

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "UserName");
        }


        // POST: /Ventas/ToggleStatus
        [HttpPost]
        public IActionResult ToggleStatus(int Id_venta)
        {
            var ventas = _context.Ventas.Find(Id_venta);
            if (ventas == null)
            {
                return NotFound();
            }

            ventas.Estado = !ventas.Estado;
            _context.Ventas.Update(ventas);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
