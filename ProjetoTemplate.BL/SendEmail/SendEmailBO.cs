using System.Net.Mail;
using System.Text;

namespace ProjetoTemplate.BL.SendEmail
{
    public class SendEmailBO : ISendEmailBO
    {
        public async Task<bool> SendEmail(string to, string subject, string message, List<string> listCCEmail = null)
        {
            return await SendEmail(new List<string> { to }, subject, message);
        }

        public async Task<bool> SendEmail(List<string> to, string subject, string message, List<string> listCCEmail = null, List<string> listAttachments = null)
        {
            try
            {
                var _emailSettings = new
                {
                    DisplaName = "ProjetoTemplate APP",
                    Domain = "smtp-gw.valecloud.com.br",
                    Port = 26,
                    UsernameEmail = "notify@triasoftware.com.br"
                };

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, _emailSettings.DisplaName)
                };

                foreach (var email in to)
                    mail.To.Add(email);

                if (listCCEmail != null)
                {
                    foreach (var cc in listCCEmail)
                        mail.CC.Add(new MailAddress(cc));
                }

                if (listAttachments != null)
                {
                    foreach (var anexo in listAttachments)
                        mail.Attachments.Add(new Attachment(anexo));
                }

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                mail.HeadersEncoding = Encoding.UTF8;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
                {
                    smtp.UseDefaultCredentials = true;
                    //smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
