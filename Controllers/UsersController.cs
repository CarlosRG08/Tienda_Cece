using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: /Users/Get/{id}
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(new { id = user.Id, estadoCuenta = user.EstadoCuenta });
        }

        // POST: /Users/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(string id, bool estadoCuenta)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.EstadoCuenta = estadoCuenta;

            // Evitar la validación de ModelState para otros campos requeridos
            ModelState.Clear();

            // Actualizar el usuario en el contexto y guardar cambios
            try
            {
                _context.Update(existingUser);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(existingUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


    }
}
