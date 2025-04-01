using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;
using System.IO;
using System.Threading.Tasks;

namespace Tienda_Cece.Controllers
{
    public class Producto_ImagenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Producto_ImagenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Producto_Imagen
        public IActionResult Index()
        {
            var productoImagenes = _context.Producto_Imagenes.Include(pi => pi.Producto).ToList();
            LoadProductos();
            return View(productoImagenes);
        }

        // GET: /Producto_Imagen/Create
        public IActionResult Create()
        {
            LoadProductos();
            return PartialView("_Create");
        }

        // POST: /Producto_Imagen/Create
        [HttpPost]
        public async Task<IActionResult> Create(Producto_Imagen productoImagen, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        productoImagen.imagen = memoryStream.ToArray();
                    }
                }

                _context.Producto_Imagenes.Add(productoImagen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadProductos();
            return PartialView("_Create", productoImagen);
        }

        // GET: /Producto_Imagen/Edit/{id}
        public IActionResult Edit(int id)
        {
            var productoImagen = _context.Producto_Imagenes.Find(id);
            if (productoImagen == null)
            {
                return NotFound();
            }

            LoadProductos();
            return PartialView("_Edit", productoImagen);
        }

        // POST: /Producto_Imagen/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Producto_Imagen productoImagen, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        productoImagen.imagen = memoryStream.ToArray();
                    }
                }

                _context.Producto_Imagenes.Update(productoImagen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadProductos();
            return PartialView("_Edit", productoImagen);
        }

        // POST: /Producto_Imagen/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoImagen = await _context.Producto_Imagenes.FindAsync(id);
            if (productoImagen != null)
            {
                _context.Producto_Imagenes.Remove(productoImagen);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // POST: /ProductoImagen/ToggleStatus
        [HttpPost]
        public IActionResult ToggleStatus(int Id_ProductoImg)
        {
            var productoImagen = _context.Producto_Imagenes.Find(Id_ProductoImg);
            if (productoImagen == null)
            {
                return NotFound();
            }

            productoImagen.Estado = !productoImagen.Estado;
            _context.Producto_Imagenes.Update(productoImagen);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Acción para obtener la imagen
        public async Task<IActionResult> GetImage(int id)
        {
            var productoImagen = await _context.Producto_Imagenes.FirstOrDefaultAsync(pi => pi.Id_ProductoImg == id);

            if (productoImagen == null || productoImagen.imagen == null)
            {
                return NotFound();
            }

            // Ajusta el tipo MIME según sea necesario
            var mimeType = "image/jpeg"; // o "image/png" según corresponda

            return File(productoImagen.imagen, mimeType);
        }


        private void LoadProductos()
        {
            var productos = _context.Productos
                .Where(c => c.Estado) // Filtrar categorías activas
                .Select(c => new { c.Id_Producto, c.Nombre_Producto })
                .ToList();

            ViewBag.Productos = new SelectList(productos, "Id_Producto", "Nombre_Producto");
        }
    }
}