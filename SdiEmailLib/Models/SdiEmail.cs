using System.Collections.Generic;

namespace SdiEmailLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SdiEmail
    {
        /// <summary>
        /// 
        /// </summary>
        public SdiEmail()
        {
            Body = string.Empty;
            Subject = string.Empty;
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            Attachments = new List<string>();
        }

        /// <summary>
        /// List of Email Addresses.
        /// The emails will be validated to ensure the correct format is followed
        /// </summary>
        public List<string> To { get; set; }
        /// <summary>
        /// List of Email Addresses.
        /// The emails will be validated to ensure the correct format is followed
        /// </summary>
        public List<string> Cc { get; set; }
        /// <summary>
        /// List of Email Addresses.
        /// The emails will be validated to ensure the correct format is followed
        /// </summary>
        public List<string> Bcc { get; set; }
        /// <summary>
        /// Email Body.  Defaults to string.Empty
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Email Subject.  Defaults to string.Empty
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// true if the message body is in HTML; else false. The default is false.
        /// Defaults to false
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// List of fully qualified files paths on the local final system.
        /// Defaults to Empty List
        /// </summary>
        public List<string> Attachments { get; set; }

    }
}
