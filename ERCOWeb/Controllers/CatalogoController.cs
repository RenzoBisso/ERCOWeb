using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ErcoContext _context;

        public CatalogoController(ErcoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var catalogos = await _context.Catalogos
                                    .OrderBy(c => c.Tipo)
                                    .ToListAsync();

                ViewBag.General = catalogos.FirstOrDefault(c => c.Tipo == "General");
                ViewBag.Unilever = catalogos.FirstOrDefault(c => c.Tipo == "Unilever");

                return View();
            }
            catch (Exception ex) { 
                return View(ex);
            }

        }
    }
}