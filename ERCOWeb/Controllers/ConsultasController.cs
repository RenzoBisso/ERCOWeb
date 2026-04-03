using ERCOWeb.Models.ViewModels;
using ERCOWeb.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERCOWeb.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly IServicioEmail _servicioEmail;

        public ConsultasController(IServicioEmail servicioEmail)
        {
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
            if (!ModelState.IsValid)
            {
                return View("Index", formulario);
            }

            try
            {
                string cuerpoEmail = $"Nombre: {formulario.Nombre} {formulario.Apellido} \n" +

                        $"Teléfono: {formulario.Telefono} \n" +

                        $"Localidad: {formulario.Localidad} \n" +

                        $"Mensaje: {formulario.Mensaje}";

                await _servicioEmail.EnviarEmail(formulario.Email, "Nueva Consulta Web", cuerpoEmail);

                TempData["MensajeExito"] = "¡Gracias! Tu mensaje ha sido enviado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo enviar el correo: " + ex.Message;
                return View("Index", formulario);
            }
        }
    }
}