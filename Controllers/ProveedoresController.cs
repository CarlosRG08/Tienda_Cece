using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedores.ToListAsync());
        }

        // GET: Proveedores/Get/5
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(m => m.id_proveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return Json(proveedor);
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_proveedor,nom_proveedor,website")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor); // Cambia esto para mostrar el formulario con errores
        }


        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_proveedor,nom_proveedor,website")] Proveedor proveedor)
        {
            if (id != proveedor.id_proveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.id_proveedor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(proveedor); // Cambia esto para mostrar el formulario con errores
        }


        // POST: Proveedores/ToggleStatus/5
        [HttpPost]
        public IActionResult ToggleStatus(int Id_Proveedor)
        {
            var proveedor = _context.Proveedores.Find(Id_Proveedor);
            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.Estado = !proveedor.Estado; // Cambiar el estado
            _context.SaveChanges();

            return Json(new { success = true });
        }


        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.id_proveedor == id);
        }
    }
}