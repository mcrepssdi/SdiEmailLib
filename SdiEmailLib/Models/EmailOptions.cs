using System.Net.Mail;

namespace SdiEmailLib.Models
{
    public class EmailOptions
    {
        public bool EnableSqlTransaction { get; set; } = false;
        public bool Persist { get; set; } = false;
        public MailPriority MailPriority { get; set; } = MailPriority.Normal;
        public bool IsBodyHtml { get; set; } = false;
    }
}