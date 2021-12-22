using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

        public static Transmission InitTransmission(int applicationId, List<string> attachments)
        {
            string servername = Environment.GetEnvironmentVariable("COMPUTERNAME");
            SerializeObj obj = new()
            {
                To = "", //ConnectionInfo.Host
                HostName = servername,
                Sender = servername,
                Cc = string.Empty,
                Bcc = string.Empty
            };
            attachments.ForEach(s =>
            {
                string[] data = File.ReadAllLines(s);
                obj.Attachments.Add(string.Join(Environment.NewLine, data));
            });
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            return new Transmission
            {
                ApplicationId = applicationId,
                Recipients = "", //ConnectionInfo.Host,
                RecipientsCc = string.Empty,
                RecipientsBcc = string.Empty,
                SerializedClass = nameof(SerializeObj),
                SerializedFormat = "JsonUtf8Bytes",
                SerializedData = Convert.ToBase64String(jsonUtf8Bytes),
                TransmissionDateTime = DateTime.Now,
                Sender = servername,
                HostName = servername
            };
        }
    }
}