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
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (!buscarEmail(email))
                {
                    Usuario user=new Usuario(nombre,email,true);
                    await _context.Usuarios.AddAsync(user);
                    await _context.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
