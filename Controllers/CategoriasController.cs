using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Categorias
        public async Task<IActionResult> Index()
        {
            var compras = _context.Categorias.ToList();
            return View(await _context.Categorias.ToListAsync());
        }

        // POST: /Categorias/Create
        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Categoria/Edit
        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // POST: /Categoria/Delete
        [HttpPost]
        public IActionResult Delete(int Id_Categoria)
        {
            var categoria = _context.Categorias.Find(Id_Categoria);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id_Categoria == id);
        }



        // POST: /Compras/ToggleStatus
        [HttpPost]
        public IActionResult ToggleStatus(int Id_Categoria)
        {
            var categoria = _context.Categorias.Find(Id_Categoria);
            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = !categoria.Estado;
            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}