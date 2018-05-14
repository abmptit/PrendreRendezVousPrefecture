using System.Net.Mail;

namespace PrendreRendezVousPrefecture.Helpers
{
    public static class MailHelper
    {
        public static void SendMail(string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("aben-miled@talentsoft.com");
            mail.To.Add("benmiledaymen@gmail.com");
            mail.Subject = subject;
            mail.Body = body;
            mail.Priority = MailPriority.Normal;

            SmtpClient mailSender = new SmtpClient(SmtpConfig.Address);
            mailSender.UseDefaultCredentials = false;
            mailSender.Port = SmtpConfig.Port;
            mailSender.EnableSsl = false;
            mailSender.Timeout = 10000;
            mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailSender.Credentials = new System.Net.NetworkCredential(SmtpConfig.Login, SmtpConfig.Password);

            mailSender.Send(mail);
        }
    }
}
