using ERCOWeb.Models;
using ERCOWeb.Models.ViewModels;
using ERCOWeb.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERCOWeb.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly ErcoContext _context;
        private readonly IServicioEmail _servicioEmail;
        public ConsultasController(ErcoContext context, IServicioEmail servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Enviar(FormViewModel formulario)
        {
            Console.WriteLine("ENTRO AL METODO ENVIAR");
            Console.WriteLine($"ModelState válido: {ModelState.IsValid}");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                Console.WriteLine($"VALIDACION: {error.ErrorMessage}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine($"VALIDACION: {error.ErrorMessage}");
                return View("Index", formulario);
            }

            try
            {
                string cuerpoEmail = $@"
            <div style='font-family: sans-serif; border: 1px solid #eee; padding: 20px; border-radius: 10px;'>
                <h2 style='color: #c02873;'>Nueva Consulta desde la Web</h2>
                <p><strong>Nombre:</strong> {formulario.Nombre} {formulario.Apellido}</p>
                <p><strong>Email:</strong> {formulario.Email}</p>
                <p><strong>Teléfono:</strong> {formulario.Telefono}</p>
                <p><strong>Localidad:</strong> {formulario.Localidad}</p>
                <hr>
                <p><strong>Mensaje:</strong></p>
                <p style='background: #f9f9f9; padding: 15px;'>{formulario.Mensaje}</p>
            </div>";


                await _servicioEmail.EnviarEmail("erconotificaciones@gmail.com", "Nueva Consulta Web", cuerpoEmail);
                Console.WriteLine("EMAIL ENVIADO OK");

                TempData["MensajeExito"] = "¡Gracias! Tu mensaje ha sido enviado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"INNER: {ex.InnerException?.Message}");
                ViewBag.Error = "Hubo un problema al enviar el correo: " + ex.Message;
                return View("Index", formulario);
            }
        }
    }
}