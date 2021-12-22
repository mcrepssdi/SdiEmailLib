using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using SdiEmailLib.Models;

namespace SdiEmailLib.Utilities.cs
{
    public static class EmailUtility
    {
        
        public static SmtpClient GetClient(Host host)
        {
            return new SmtpClient
            {
                Host = host.HostName,
                Port = host.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(host.Username, host.Password)
            };
        }
        
        public static MailMessage InitMailMessage(SdiEmail email, MailPriority mailPriority, string From, bool IsBodyHtml)
        {
            MailMessage mailmsg = new ()
            {
                From = new MailAddress(From)
            };
            mailmsg.To.Add(string.Join(",", email.To));
            if (email.Cc.Count > 0)
            {
                mailmsg.CC.Add(string.Join(",", email.Cc));
            }
            if (email.Bcc.Count > 0)
            {
                mailmsg.Bcc.Add(string.Join(",", email.Bcc));
            }
            mailmsg.Body = email.Body;
            mailmsg.Subject = email.Subject;
            mailmsg.IsBodyHtml = IsBodyHtml;
            mailmsg.Priority = mailPriority;
            email.Attachments.ForEach(s =>
            {
                Attachment attachment = Path.GetExtension(s).ToLower() switch
                {
                    "pdf" => new Attachment(s, MediaTypeNames.Application.Pdf),
                    "rtf" => new Attachment(s, MediaTypeNames.Application.Rtf),
                    "zip" => new Attachment(s, MediaTypeNames.Application.Zip),
                    _ => new Attachment(s, MediaTypeNames.Application.Octet)
                };
                mailmsg.Attachments.Add(attachment);
            });
            mailmsg.BodyEncoding = Encoding.UTF8;
            mailmsg.SubjectEncoding = Encoding.UTF8;
            return mailmsg;
        }

        public static Transmission InitTransmission(int appid, string hostName, string fromAddress, SdiEmail email)
        {
            SerializeObj obj = new ()
            {
                Cc = string.Join(",", email.Cc.ToArray()),
                Bcc = string.Join(",", email.Bcc.ToArray()),
                To = string.Join(",", email.To.ToArray()),
                HostName = hostName,
                Sender = fromAddress
            };
            email.Attachments.ForEach(s =>
            {
                if (File.Exists(s))
                {
                    string[] data = File.ReadAllLines(s);
                    obj.Attachments.Add(string.Join(Environment.NewLine, data));
                }
            });
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);

            return new Transmission
            {
                ApplicationId = appid,
                HostName = hostName,
                Sender = fromAddress,
                Recipients = string.Join(",", email.To.ToArray()),
                RecipientsCc = string.Join(",", email.Cc.ToArray()),
                RecipientsBcc = string.Join(",", email.Bcc.ToArray()),
                SerializedClass = nameof(SerializeObj),
                SerializedFormat = "JsonUtf8Bytes",
                SerializedData = Convert.ToBase64String(jsonUtf8Bytes)
            };
        }
    }
}