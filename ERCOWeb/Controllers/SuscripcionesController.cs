using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERCOWeb.Controllers
{
    public class SuscripcionesController : Controller
    {

        private readonly ErcoContext _context;

        public SuscripcionesController(ErcoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public bool buscarEmail(string email)
        {
            return _context.Usuarios.Any(x => x.Email == email);
        }

        [HttpPost]
        public async Task<IActionResult> Suscribir(string nombre, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Index", "Home");
                }
                bool existe = buscarEmail(email);
                if (!existe)
                {
                    Usuario user = new Usuario(nombre, email, true);
                    await _context.Usuarios.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
