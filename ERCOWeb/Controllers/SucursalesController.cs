using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class SucursalesController : Controller
    {

        private readonly ErcoContext _context;


        public SucursalesController(ErcoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var listaSucursales = await _context.Sucursals.ToListAsync();
                var listaZonas = await _context.Zonas.ToListAsync();

                var modeloTupla = (sucursales: listaSucursales, zonas: listaZonas);

                return View(modeloTupla);
            }
            catch (Exception ex) { 
                return View(ex);
            }

        }
        [HttpGet]
        public JsonResult GetSucursales()
        {
            try
            {
                            var data = _context.Sucursals
                .Where(s => s.Latitud != null && s.Longitud != null)
                .Select(s => new {
                    nombre = s.Nombre,
                    lat = s.Latitud,
                    lng = s.Longitud
                })
                .ToList();

            return Json(data);
            }
            catch
            {
                return Json(null);
            }

        }
    }
}
