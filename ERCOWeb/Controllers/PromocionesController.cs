using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class PromocionesController : Controller
    {

        private readonly ErcoContext _context;

        public PromocionesController(ErcoContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var promosValidas = await _context.Promos
                .Where(p => p.FechaInicio <= hoy && p.FechaFin >= hoy)
                .ToListAsync();

            return View(promosValidas);
        }


    }
}
