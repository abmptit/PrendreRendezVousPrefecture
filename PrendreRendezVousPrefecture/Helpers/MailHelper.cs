using System.Collections.Generic;
using System.Net.Mail;

namespace PrendreRendezVousPrefecture.Helpers
{
    public static class MailHelper
    {
        public static void SendMail(string subject, 
            string body, 
            IEnumerable<string> attachements = null, 
            IEnumerable<string> destinataires = null)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("aben-miled@talentsoft.com");

            foreach (var destinataire in destinataires)
            {
                mail.To.Add(destinataire);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.Priority = MailPriority.Normal;
            foreach (var attachement in attachements)
            {
                mail.Attachments.Add(new Attachment(attachement));
            }

            SmtpClient mailSender = new SmtpClient(SmtpConfig.Address);
            mailSender.UseDefaultCredentials = false;
            mailSender.Port = SmtpConfig.Port;
            mailSender.EnableSsl = false;
            mailSender.Timeout = 10000;
            mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailSender.Credentials = new System.Net.NetworkCredential(SmtpConfig.Login, SmtpConfig.Password);

            mailSender.Send(mail);
            System.Console.WriteLine($"Mail Envoyé {subject} / {body}");
        }
    }
}
