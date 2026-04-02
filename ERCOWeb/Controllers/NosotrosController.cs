using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class NosotrosController : Controller
    {

        private readonly ErcoContext _context;

        public NosotrosController(ErcoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Sucursals.ToListAsync());
        }
    }
}
