using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class ContactoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EnviarCorreo(contacto);
                    TempData["MensajeExito"] = "Tu mensaje ha sido enviado con éxito.";
                    return RedirectToAction("Gracias");
                }
                catch (Exception)
                {
                    ViewBag.Mensaje = "Error al enviar el mensaje.";
                }
            }
            return View(contacto);
        }

        private void EnviarCorreo(Contacto contacto)
        {
            var fromAddress = new MailAddress("contactotiendacece@gmail.com", "Tienda Cece");
            var toAddress = new MailAddress("contactotiendacece@gmail.com", "Destinatario");
            const string fromPassword = "kzvv serj jvbe jagw";
            const string subject = "Nuevo mensaje de contacto";

            string body = $"Nombre: {contacto.Nombre}\n" +
                          $"Correo electrónico: {contacto.CorreoElectronico}\n" +
                          $"Mensaje: {contacto.Mensaje}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        public ActionResult Gracias()
        {
            return View();
        }
    }
}