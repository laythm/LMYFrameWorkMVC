using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public class EmailHelper
    {
        public async Task SendEmail(LMYFrameWorkMVC.Common.Entities.EmailInfo emailInfo)
        {
            Thread.Sleep(2000);
            // Make the mail message.
            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailInfo.SMTPInfo.From.Email, emailInfo.SMTPInfo.From.Name);

            foreach (Common.Entities.MailAddress mailAddress in emailInfo.To)
            {
                message.To.Add(new MailAddress(mailAddress.Email, mailAddress.Name));
            }

            foreach (Common.Entities.MailAddress mailAddress in emailInfo.CC)
            {
                message.CC.Add(new MailAddress(mailAddress.Email, mailAddress.Name));
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(emailInfo.Body);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
            {
                if (!string.IsNullOrEmpty(link.Attributes["src"].Value))
                {
                    string contentID = Guid.NewGuid().ToString();
                    Attachment inlineImage = new Attachment(FileHelper.GetFullFilePathFromShortPath(link.Attributes["src"].Value));
                    inlineImage.ContentId = contentID;
                    inlineImage.ContentDisposition.Inline = true;
                    inlineImage.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                    message.Attachments.Add(inlineImage);

                    link.Attributes["src"].Value = "cid:" + contentID;
                }
            }

            SmtpClient client = new SmtpClient()
            {
                Host = emailInfo.SMTPInfo.Host,
                Port = emailInfo.SMTPInfo.Port,
                EnableSsl = emailInfo.SMTPInfo.EnableSsl,
                UseDefaultCredentials = emailInfo.SMTPInfo.UseDefaultCredentials,
                Credentials = new NetworkCredential(
                    emailInfo.SMTPInfo.From.Email, emailInfo.SMTPInfo.From.Password),
            };

            message.Subject = emailInfo.Subject;
            message.Body = doc.ToString();
            message.IsBodyHtml = true;

            // Send the message.
            await client.SendMailAsync(message);
        }

    }
}
