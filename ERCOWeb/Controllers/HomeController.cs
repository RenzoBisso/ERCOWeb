using System.Diagnostics;
using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly  ErcoContext _context;
        public HomeController(ILogger<HomeController> logger,ErcoContext ercoContext)
        {
            _logger = logger;
            _context = ercoContext;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var listaMarcasFiltradas = await _context.Marcas
                    .Where(m => m.Estado == true)
                    .ToListAsync();

                return View(listaMarcasFiltradas);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
