using ERCOWeb.Models;
using ERCOWeb.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERCOWeb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly ErcoContext _context;
        private readonly IServicioEmail _servicioEmail;
        public AdminController(ErcoContext context, IServicioEmail servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Promos(DateOnly? startDate, DateOnly? endDate, string search)
        {
            try
            {
                var query = _context.Promos.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Titulo.Contains(search));
                }

                if (startDate.HasValue)
                {
                    query = query.Where(p => p.FechaInicio >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(p => p.FechaFin <= endDate.Value);
                }

                var promos = await query.ToListAsync();

                var entityType = _context.Model.FindEntityType(typeof(Promo));
                var propiedades = entityType.GetProperties().ToList();
                var primaryKey = entityType.FindPrimaryKey().Properties.First();
                var keyName = primaryKey.Name;

                return View((promos, propiedades, keyName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModificarPromo(int id)
        {
            try
            {
                var promo = await _context.Promos.FindAsync(id);

                if (promo == null)
                {
                    return NotFound();
                }

                return View(promo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarPromo(Promo Model, IFormFile archivoPdf, IFormFile image)
        {
            try
            {
                var promoBD = await _context.Promos.FindAsync(Model.Id);
                if (promoBD == null)
                {
                    return NotFound();
                }

                promoBD.Titulo = Model.Titulo;
                promoBD.Descripcion = Model.Descripcion;
                promoBD.FechaInicio = Model.FechaInicio;
                promoBD.FechaFin = Model.FechaFin;

                if (archivoPdf != null && archivoPdf.Length > 0)
                {
                    string nombreUnico = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(archivoPdf.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "promopdf");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);

                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await archivoPdf.CopyToAsync(stream);
                    }

                    promoBD.Pdf = "/promopdf/" + nombreUnico;
                }

                if (image != null && image.Length > 0)
                {
                    string nombreUnicoImg = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(image.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "promos");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreUnicoImg);

                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    promoBD.Imagen = "/images/promos/" + nombreUnicoImg;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Promos", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CargarPromo()
        {
            try
            {
                return View(new Promo());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarPromo(Promo Model, IFormFile archivoPdf, IFormFile image)
        {
            try
            {
                if (Model == null) throw new ArgumentNullException(nameof(Model));

                if (archivoPdf != null && archivoPdf.Length > 0)
                {
                    string nombreUnico = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(archivoPdf.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "promopdf");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);
                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);
                    using (var stream = new FileStream(rutaFisica, FileMode.Create)) { await archivoPdf.CopyToAsync(stream); }
                    Model.Pdf = "/promopdf/" + nombreUnico;
                }

                if (image != null && image.Length > 0)
                {
                    string nombreArchivo = Path.GetFileName(image.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "promos");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);
                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);
                    using (var stream = new FileStream(rutaFisica, FileMode.Create)) { await image.CopyToAsync(stream); }
                    Model.Imagen = "/images/promos/" + nombreArchivo;
                }

                await _context.AddAsync(Model);
                await _context.SaveChangesAsync();

                try
                {
                    var destinatarios = await _context.Usuarios
                        .Where(u => !string.IsNullOrEmpty(u.Email) && u.Estado != false)
                        .Select(u => u.Email)
                        .ToListAsync();

                    if (destinatarios.Any())
                    {
                        string urlWeb = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                        string cuerpoEmail = $@"
                            <div style='font-family: sans-serif; max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;'>
                                <div style='background-color: #3e3f3f; padding: 20px; text-align: center;'>
                                    <h1 style='color: #dfa12a; margin: 0;'>¡Nueva Promo en ERCO!</h1>
                                </div>
                                <div style='padding: 20px; text-align: center;'>
                                    <h2 style='color: #c02873;'>{Model.Titulo}</h2>
                                    <p style='color: #555; font-size: 16px;'>No te pierdas nuestra última oportunidad de ahorro.</p>
                                    <img src='{urlWeb}{@Url.Content(@Model.Imagen)}' style='width: 100%; max-width: 400px; border-radius: 5px; margin: 20px 0;' />
                                    <br>
                                    <a href='{urlWeb}' style='background-color: #dfa12a; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>VER MÁS DETALLES</a>
                                </div>
                                <div style='background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 12px; color: #888;'>
                                    ERCO S.R.L - Distribuidora Mayorista
                                </div>
                            </div>";

                        await _servicioEmail.EnviarEmailMasivo(destinatarios, "🔥 Nueva Promoción Imperdible", cuerpoEmail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR EMAIL: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"INNER: {ex.InnerException.Message}");
                }

                return RedirectToAction("Promos", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Marcas(string search)
        {
            try
            {
                var query = _context.Marcas.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Nombre.Contains(search));
                }

                var marcas = await query.ToListAsync();

                var entityType = _context.Model.FindEntityType(typeof(Marca));
                var propiedades = entityType.GetProperties().ToList();
                var primaryKey = entityType.FindPrimaryKey().Properties.First();
                var keyName = primaryKey.Name;

                return View((marcas, propiedades, keyName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Categorias(string search)
        {
            try
            {
                var query = _context.Categoria.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Nombre.Contains(search));
                }

                var categorias = await query.ToListAsync();

                var entityType = _context.Model.FindEntityType(typeof(Categorium));
                var propiedades = entityType.GetProperties().ToList();
                var primaryKey = entityType.FindPrimaryKey().Properties.First();
                var keyName = primaryKey.Name;

                return View((categorias, propiedades, keyName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Sucursales(string search)
        {
            try
            {
                var query = _context.Sucursals.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Nombre.Contains(search));
                }

                var sucursales = await query.ToListAsync();

                var entityType = _context.Model.FindEntityType(typeof(Sucursal));
                var propiedades = entityType.GetProperties().ToList();
                var primaryKey = entityType.FindPrimaryKey().Properties.First();
                var keyName = primaryKey.Name;

                return View((sucursales, propiedades, keyName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CargarMarca()
        {
            try
            {
                return View(new Marca { Estado = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarMarca(Marca model, IFormFile imgMarca)
        {
            try
            {
                if (model == null) return BadRequest();

                bool existe = await _context.Marcas.AnyAsync(p => p.Nombre == model.Nombre);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una marca con ese nombre.");
                    return View(model);
                }

                if (imgMarca != null && imgMarca.Length > 0)
                {
                    string nombreUnico = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(imgMarca.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "marcas");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);

                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await imgMarca.CopyToAsync(stream);
                    }

                    model.Imagen = "/images/marcas/" + nombreUnico;
                }

                _context.Marcas.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Marcas", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModificarMarca(int id)
        {
            try
            {
                var marca = await _context.Marcas.FindAsync(id);

                if (marca == null)
                {
                    return NotFound();
                }

                return View(marca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarMarca(Marca model, IFormFile imgMarca)
        {
            try
            {
                if (model == null) return BadRequest();

                var marcaDB = await _context.Marcas.FindAsync(model.IdMarca);
                if (marcaDB == null) return NotFound();

                bool existe = await _context.Marcas.AnyAsync(p => p.Nombre == model.Nombre && p.IdMarca != model.IdMarca);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otra marca con ese nombre.");
                    return View(model);
                }

                marcaDB.Nombre = model.Nombre;
                marcaDB.Estado = model.Estado;
                marcaDB.Prioridad = model.Prioridad;

                if (imgMarca != null && imgMarca.Length > 0)
                {
                    string nombreUnico = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(imgMarca.FileName);
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "marcas");
                    string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);

                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await imgMarca.CopyToAsync(stream);
                    }

                    marcaDB.Imagen = "/images/marcas/" + nombreUnico;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Marcas", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CargarCategoria()
        {
            try
            {
                return View(new Categorium { Estado = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarCategoria(Categorium model)
        {
            try
            {
                if (model == null) return BadRequest();

                bool existe = await _context.Categoria.AnyAsync(p => p.Nombre == model.Nombre);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoria con ese nombre.");
                    return View(model);
                }

                _context.Categoria.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Categorias", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModificarCategoria(int id)
        {
            try
            {
                var categoria = await _context.Categoria.FindAsync(id);

                if (categoria == null)
                {
                    return NotFound();
                }

                return View(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarCategoria(Categorium model)
        {
            try
            {
                if (model == null) return BadRequest();

                var categoriaDB = await _context.Categoria.FindAsync(model.IdCategoria);
                if (categoriaDB == null) return NotFound();

                bool existe = await _context.Categoria.AnyAsync(p => p.Nombre == model.Nombre && p.IdCategoria != model.IdCategoria);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otra marca con ese nombre.");
                    return View(model);
                }

                categoriaDB.Nombre = model.Nombre;
                categoriaDB.Estado = model.Estado;

                await _context.SaveChangesAsync();
                return RedirectToAction("Categorias", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CargarSucursal()
        {
            try
            {
                return View(new Sucursal { Estado = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarSucursal(Sucursal model)
        {
            try
            {
                if (model == null) return BadRequest();

                bool existe = await _context.Sucursals.AnyAsync(p => p.Nombre == model.Nombre);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe una sucursal con ese nombre.");
                    return View(model);
                }

                _context.Sucursals.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Sucursales", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ModificarSucursal(int id)
        {
            try
            {
                var sucursal = await _context.Sucursals.FindAsync(id);

                if (sucursal == null)
                {
                    return NotFound();
                }

                return View(sucursal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarSucursal(Sucursal model)
        {
            try
            {
                if (model == null) return BadRequest();

                var sucursalDB = await _context.Sucursals.FindAsync(model.IdSucursal);
                if (sucursalDB == null) return NotFound();

                bool existe = await _context.Sucursals.AnyAsync(p => p.Nombre == model.Nombre && p.IdSucursal != model.IdSucursal);

                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otra marca con ese nombre.");
                    return View(model);
                }

                sucursalDB.Nombre = model.Nombre;
                sucursalDB.Estado = model.Estado;
                sucursalDB.Principal = model.Principal;
                sucursalDB.Ubicacion = model.Ubicacion;
                sucursalDB.Latitud = model.Latitud;
                sucursalDB.Longitud = model.Longitud;

                await _context.SaveChangesAsync();
                return RedirectToAction("Sucursales", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task CargarCombos()
        {
            try
            {
                ViewBag.Categorias = await _context.Categoria.ToListAsync();
                ViewBag.Marcas = await _context.Marcas.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Usuarios(string search)
        {
            try
            {
                var query = _context.Usuarios.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Nombre.Contains(search));
                }

                var usuarios = await query.ToListAsync();

                var entityType = _context.Model.FindEntityType(typeof(Usuario));
                var propiedades = entityType.GetProperties().ToList();
                var primaryKey = entityType.FindPrimaryKey().Properties.First();
                var keyName = primaryKey.Name;

                return View((usuarios, propiedades, keyName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ── PANEL CATÁLOGOS (GET) ───────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Catalogos()
        {
            try
            {
                var catalogos = await _context.Catalogos
                    .OrderBy(c => c.Tipo)
                    .ToListAsync();
                return View(catalogos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ── ACTUALIZAR CATÁLOGO (GET) ───────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ActualizarCatalogo(int id)
        {
            try
            {
                var catalogo = await _context.Catalogos.FindAsync(id);
                if (catalogo == null) return NotFound();
                return View(catalogo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ── ACTUALIZAR CATÁLOGO (POST) ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarCatalogo(
            int IdCatalogo,
            string? Descripcion,
            IFormFile? imagen,
            IFormFile? archivoPdf)
        {
            try
            {
                var catalogo = await _context.Catalogos.FindAsync(IdCatalogo);
                if (catalogo == null) return NotFound();

                catalogo.Descripcion = Descripcion?.Trim();
                catalogo.FechaActualizacion = DateTime.Now;

                if (imagen != null && imagen.Length > 0)
                {
                    string nombreLimpio = Path.GetFileName(imagen.FileName)
                        .Replace(" ", "_").Replace("..", ".");
                    string nombre = Guid.NewGuid().ToString()[..8] + "_" + nombreLimpio;
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "catalogos");
                    if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                    using var stream = new FileStream(Path.Combine(carpeta, nombre), FileMode.Create);
                    await imagen.CopyToAsync(stream);
                    catalogo.Imagen = "/images/catalogos/" + nombre;
                }

                if (archivoPdf != null && archivoPdf.Length > 0)
                {
                    string nombreLimpio = Path.GetFileName(archivoPdf.FileName)
                        .Replace(" ", "_").Replace("..", ".");
                    string nombre = Guid.NewGuid().ToString()[..8] + "_" + nombreLimpio;
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "catalogopdf");
                    if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                    using var stream = new FileStream(Path.Combine(carpeta, nombre), FileMode.Create);
                    await archivoPdf.CopyToAsync(stream);
                    catalogo.Pdf = "/catalogopdf/" + nombre;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Catalogos", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
