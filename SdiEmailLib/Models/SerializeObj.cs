using System;
using System.Collections.Generic;

namespace SdiEmailLib.Models
{
    [Serializable]
    internal class SerializeObj
    {

        public string To { get; set; } = string.Empty;
        public string Cc { get; set; } = string.Empty;
        public string Bcc { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public List<string> Attachments { get; set; } = new List<string>();
        public string HostName { get; set; } = string.Empty;
        public bool IsHtmlBody { get; set; } = true;

    }
}
