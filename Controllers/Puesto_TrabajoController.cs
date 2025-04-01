using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda_Cece.Controllers
{
    public class PuestosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PuestosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Puestos
        public async Task<IActionResult> Index()
        {
            var puestos = await _context.Puestos_de_Trabajo.ToListAsync();
            return View(puestos);
        }

        // POST: Puestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombre_puesto,descripcion_puesto")] Puesto_Trabajo puesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        // POST: Puestos/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Puesto_Trabajo puesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Puestos_de_Trabajo.Update(puesto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoExists(puesto.id_puesto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(puesto);
        }

        private bool PuestoExists(int id)
        {
            return _context.Puestos_de_Trabajo.Any(e => e.id_puesto == id);
        }


        // POST: /Puestos/ToggleStatus
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id_puesto, bool estado)
        {
            var puesto = await _context.Puestos_de_Trabajo.FindAsync(id_puesto);
            if (puesto == null)
            {
                return NotFound();
            }

            puesto.Estado = !estado; // Cambiar el estado
            _context.Update(puesto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: /PuestoTrabajo/Get/{id}
        [HttpGet]
        public IActionResult Get(int id)
        {
            var puesto = _context.Puestos_de_Trabajo.Find(id);
            if (puesto != null)
            {
                return Json(puesto);
            }
            return NotFound();
        }

    }
}
