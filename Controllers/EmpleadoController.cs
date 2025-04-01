using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tienda_Cece.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Empleados
        public IActionResult Index()
        {
            var empleados = _context.Empleado
                .Include(e => e.Puesto)
                .ToList();

            LoadPuestos(); // Inicializa ViewBag.Puestos
            return View(empleados);
        }

        // GET: /Empleados/Create
        public IActionResult Create()
        {
            LoadPuestos(); // Inicializa ViewBag.Puestos
            return View();
        }

        // POST: /Empleados/Create
        [HttpPost]
        public IActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleado.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadPuestos(); // Inicializa ViewBag.Puestos en caso de error en el modelo
            return View(empleado);
        }

        // GET: /Empleados/Edit/{id}
        public IActionResult Edit(int id)
        {
            var empleado = _context.Empleado.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }

            LoadPuestos(); // Inicializa ViewBag.Puestos
            return View(empleado);
        }

        // POST: /Empleados/Edit
        [HttpPost]
        public IActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleado.Update(empleado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadPuestos(); // Inicializa ViewBag.Puestos en caso de error en el modelo
            return View(empleado);
        }

        // GET: /Empleados/Get/{id}
        [HttpGet]
        public IActionResult Get(int id)
        {
            var empleado = _context.Empleado
                .Where(e => e.Id_Empleado == id)
                .Select(e => new
                {
                    e.Id_Empleado,
                    e.Nombre,
                    e.Primer_Apellido,
                    e.Segundo_Apellido,
                    e.Numero_Telefono,
                    e.Id_Puesto
                })
                .FirstOrDefault();

            if (empleado == null)
            {
                return NotFound();
            }

            return Json(empleado);
        }

        // POST: /Empleados/Delete
        [HttpPost]
        public IActionResult Delete(int id_Empleado)
        {
            var empleado = _context.Empleado.Find(id_Empleado);
            if (empleado == null)
            {
                return NotFound();
            }

            _context.Empleado.Remove(empleado);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void LoadPuestos()
        {
            var puestos = _context.Puestos_de_Trabajo
                .Where(p => p.Estado) // Filtrar solo los puestos activos
                .Select(p => new { p.id_puesto, p.nombre_puesto })
                .ToList();

            ViewBag.Puestos = new SelectList(puestos, "id_puesto", "nombre_puesto");
        }
    }
}
