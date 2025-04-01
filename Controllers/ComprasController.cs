using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tienda_Cece.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Compras
        public IActionResult Index()
        {
            var compras = _context.Compras
                .Include(c => c.Proveedor)
                .ToList();

            LoadProveedores(); // Inicializa ViewBag.Proveedores
            return View(compras);
        }

        // GET: /Compras/Create
        public IActionResult Create()
        {
            LoadProveedores(); // Inicializa ViewBag.Proveedores
            return View();
        }

        // POST: /Compras/Create
        [HttpPost]
        public IActionResult Create(Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Compras.Add(compra);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadProveedores(); // Inicializa ViewBag.Proveedores en caso de error en el modelo
            return View(compra);
        }

        // GET: /Compras/Edit/{id}
        public IActionResult Edit(int id)
        {
            var compra = _context.Compras.Find(id);
            if (compra == null)
            {
                return NotFound();
            }

            LoadProveedores(); // Inicializa ViewBag.Proveedores
            return View(compra);
        }

        // POST: /Compras/Edit
        [HttpPost]
        public IActionResult Edit(Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Compras.Update(compra);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadProveedores(); // Inicializa ViewBag.Proveedores en caso de error en el modelo
            return View(compra);
        }

        // GET: /Compras/Get/{id}
        [HttpGet]
        public IActionResult Get(int id)
        {
            var compra = _context.Compras
                .Where(c => c.Id_Compra == id)
                .Select(c => new
                {
                    c.Id_Compra,
                    c.Id_Proveedor,
                    c.Nombre_Producto,
                    c.Fecha,
                    c.Cant_Producto,
                    c.link_Producto,
                    c.Estado
                })
                .FirstOrDefault();

            if (compra == null)
            {
                return NotFound();
            }

            return Json(compra);
        }

        // POST: /Compras/ToggleStatus
        [HttpPost]
        public IActionResult ToggleStatus(int id_Compra)
        {
            var compra = _context.Compras.Find(id_Compra);
            if (compra == null)
            {
                return NotFound();
            }

            compra.Estado = !compra.Estado;
            _context.Compras.Update(compra);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void LoadProveedores()
        {
            var proveedores = _context.Proveedores
                .Where(p => p.Estado) // Filtrar proveedores activos
                .Select(p => new { p.id_proveedor, p.nom_proveedor })
                .ToList();

            ViewBag.Proveedores = new SelectList(proveedores, "id_proveedor", "nom_proveedor");
        }
    }
}
