using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class VacacionesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Vacaciones
        public async Task<IActionResult> Index()
        {
            var vacaciones = await _context.Vacaciones
                .Include(e => e.Empleado)
                .ToListAsync();
            ViewBag.Empleados = await GetEmpleadosSelectList();
            return View(vacaciones);
        }

        // POST: Vacaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_Empleado,FechaInicio,FechaFin,Motivo")] Vacacion vacacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Empleados = await GetEmpleadosSelectList(vacacion.Id_Empleado);
            var vacaciones = await _context.Vacaciones.Include(v => v.Empleado).ToListAsync();
            return View("Index", vacaciones);
        }

        private async Task<SelectList> GetEmpleadosSelectList(int? selectedEmpleadoId = null)
        {
            var empleados = await _context.Empleado // Asegúrate de que el nombre de la propiedad sea correcto
                .Select(e => new
                {
                    Id = e.Id_Empleado,
                    NombreCompleto = $"{e.Nombre} {e.Primer_Apellido} {e.Segundo_Apellido}"
                })
                .ToListAsync();
            return new SelectList(empleados, "Id", "NombreCompleto", selectedEmpleadoId);
        }
    }
}

