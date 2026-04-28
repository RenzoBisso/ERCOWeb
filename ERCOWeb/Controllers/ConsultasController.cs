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
        public async Task<IActionResult> Enviar(FormViewModel formulario)
        {
            if (!ModelState.IsValid) return View("Index", formulario);

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


                await _servicioEmail.EnviarEmail("renzo_bisso@outlook.com", "Nueva Consulta Web", cuerpoEmail);

                TempData["MensajeExito"] = "¡Gracias! Tu mensaje ha sido enviado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Hubo un problema al enviar el correo: " + ex.Message;
                return View("Index", formulario);
            }
        }
    }
}