using ERCOWeb.Models.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Net.Security;

namespace ERCOWeb.Servicios
{
    public interface IServicioEmail
    {
        Task EnviarEmail(string emailReceptor, string tema, string cuerpo);
        Task EnviarEmailMasivo(List<string> receptores, string tema, string cuerpo);
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
            ServicePointManager.ServerCertificateValidationCallback =
    (sender, certificate, chain, sslPolicyErrors) => true;
            using (var smtpCliente = new SmtpClient(host, puerto))
            {
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Credentials = new NetworkCredential(emailEmisor, password);

                var mensaje = new MailMessage(emailEmisor, emailReceptor, tema, cuerpo);
                mensaje.IsBodyHtml = true;

                await smtpCliente.SendMailAsync(mensaje);
            }
        }

        public async Task EnviarEmailMasivo(List<string> receptores, string tema, string cuerpo)
        {
            var emailEmisor = _configuration["CONFIGURACIONES_EMAIL:EMAIL"];
            var password = _configuration["CONFIGURACIONES_EMAIL:PASSWORD"];
            var host = _configuration["CONFIGURACIONES_EMAIL:HOST"];
            var puerto = int.Parse(_configuration["CONFIGURACIONES_EMAIL:PUERTO"]);

            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
    (sender, certificate, chain, sslPolicyErrors) => true;
                using (var smtpCliente = new SmtpClient(host, puerto))
                {
                    smtpCliente.EnableSsl = true;
                    smtpCliente.UseDefaultCredentials = false;
                    smtpCliente.Credentials = new NetworkCredential(emailEmisor, password);

                    using (var mensaje = new MailMessage())
                    {
                        mensaje.From = new MailAddress(emailEmisor, "ERCO S.R.L");
                        mensaje.Subject = tema;
                        mensaje.Body = cuerpo;
                        mensaje.IsBodyHtml = true;

                        mensaje.To.Add(emailEmisor);

                        foreach (var email in receptores)
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                mensaje.Bcc.Add(email.Trim()); 
                            }
                        }

                        await smtpCliente.SendMailAsync(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n--- ERROR AL ENVIAR ---");
                Console.WriteLine($"Error general: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Causa real (InnerException): {ex.InnerException.Message}");
                }
                Console.WriteLine($"-----------------------\n");

                throw; 
            }
        }
    }
}
