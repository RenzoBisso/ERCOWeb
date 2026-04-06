using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class ZonaController : Controller
    {
        private readonly ErcoContext _context;

        public ZonaController(ErcoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Zonas.ToListAsync());
        }
        [HttpGet]
        public JsonResult GetZonas()
        {
            var data = _context.Zonas
                .Where(s=>s.Estado!=false)
                .Select(s => new {
                    nombre = s.Nombre,
                    estado = s.Estado,
                })
                .ToList();

            return Json(data);
        }
    }
}
