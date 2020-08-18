using WebApp.Common;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core;

namespace WebApp.Business.Services
{
    public class SystemService
    {
        private DT_WebGISEntities db = new DT_WebGISEntities();

        public void SendMail(string email, string fromMail, string fromName, string passMail, string subject, string strBody, List<string> lstAttachFile)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                            new System.Net.Mail.MailAddress(fromMail, fromName),
                            new System.Net.Mail.MailAddress(email));
            m.Subject = subject;
            m.Body = strBody;
            m.IsBodyHtml = true;

            if (lstAttachFile != null)
            {
                System.Net.Mail.Attachment attachment;

                foreach (var attachFile in lstAttachFile)
                {
                    attachment = new System.Net.Mail.Attachment(attachFile);
                    m.Attachments.Add(attachment);
                }
            }

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Credentials = new System.Net.NetworkCredential(fromMail, passMail);
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
