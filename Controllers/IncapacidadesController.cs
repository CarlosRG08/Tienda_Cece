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

    public class IncapacidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncapacidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Incapacidades
        public async Task<IActionResult> Index()
        {
            var incapacidades = await _context.Incapacidades
                .Include(i => i.Empleado)
                .ToListAsync();

            ViewBag.Empleados = await GetEmpleadosSelectList();
            return View(incapacidades);
        }

        // POST: Incapacidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_Empleado,FechaInicio,FechaFin,Motivo")] Incapacidad incapacidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incapacidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Empleados = await GetEmpleadosSelectList(incapacidad.Id_Empleado);
            var incapacidades = await _context.Incapacidades.Include(i => i.Empleado).ToListAsync();
            return View("Index", incapacidades);
        }

        private async Task<SelectList> GetEmpleadosSelectList(int? selectedEmpleadoId = null)
        {
            var empleados = await _context.Empleado
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

