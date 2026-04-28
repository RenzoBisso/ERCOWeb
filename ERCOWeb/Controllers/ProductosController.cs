using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ErcoContext _context;

        public ProductosController(ErcoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? idCategoria)
        {
            var query = _context.Productos
                .Include(p => p.IdMarcaNavigation)
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.Estado == true) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Nombre.Contains(search));

            if (idCategoria.HasValue)
                query = query.Where(p => p.IdCategoria == idCategoria);

            ViewBag.Categorias = await _context.Categoria.ToListAsync();
            ViewBag.Marcas = await _context.Marcas.ToListAsync();

            var productos = await query.ToListAsync();

            return View(productos); 
        }
    }
}