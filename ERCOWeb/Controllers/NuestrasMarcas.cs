using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class NuestrasMarcasController : Controller 
    {
        private readonly ErcoContext _context;

        public NuestrasMarcasController(ErcoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var listaMarcasFiltradas = await _context.Marcas
                                            .Where(m => m.Estado == true)
                                            .ToListAsync();

            return View(listaMarcasFiltradas);
        }
    }
}