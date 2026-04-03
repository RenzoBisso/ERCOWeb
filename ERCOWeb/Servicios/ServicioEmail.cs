using ERCOWeb.Models.ViewModels;
using System.Net;
using System.Net.Mail;

namespace ERCOWeb.Servicios
{
    public interface IServicioEmail
    {
        Task EnviarEmail(string emailReceptor, string tema, string cuerpo);
    }

    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration _configuration;

        public ServicioEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmail(string emailReceptor, string tema, string cuerpo)
        {
            var emailEmisor = _configuration["CONFIGURACIONES_EMAIL:EMAIL"];
            var password = _configuration["CONFIGURACIONES_EMAIL:PASSWORD"];
            var host = _configuration["CONFIGURACIONES_EMAIL:HOST"];
            var puerto = int.Parse(_configuration["CONFIGURACIONES_EMAIL:PUERTO"]);

            using (var smtpCliente = new SmtpClient(host, puerto))
            {
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Credentials = new NetworkCredential(emailEmisor, password);

                var mensaje = new MailMessage(emailEmisor, emailReceptor, tema, cuerpo);


                await smtpCliente.SendMailAsync(mensaje);
            }
        }
    }
}
