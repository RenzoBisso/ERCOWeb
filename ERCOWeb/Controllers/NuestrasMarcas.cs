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
            try
            {
                var listaMarcasFiltradas = await _context.Marcas
                                                .Where(m => m.Estado == true && m.Prioridad != null)
                                                .OrderBy(m => m.Prioridad)
                                                .ToListAsync();

                return View(listaMarcasFiltradas);
            }catch(Exception ex)
            {
                return View(ex);
            }

        }
    }
}