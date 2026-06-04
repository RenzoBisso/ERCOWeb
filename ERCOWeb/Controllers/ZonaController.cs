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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var sucursales = await _context.Zonas.ToListAsync();

                var listaZonas = await _context.Zonas.ToListAsync();

                foreach (var zona in listaZonas)
                {
                    if (zona.Nombre != null)
                    {
                        zona.Nombre = zona.Nombre.Trim();
                    }
                }

                var zonasOrdenadas = listaZonas
                    .OrderBy(z => z.Nombre)
                    .ToList();
                return View((sucursales, zonasOrdenadas));
            }
            catch (Exception)
            {
                return View((new List<Sucursal>(), new List<Zona>()));
            }
        }
    }
}