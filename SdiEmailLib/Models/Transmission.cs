using System;

namespace SdiEmailLib.Models
{
    public class Transmission
    {
        public int ApplicationId { get; set; }
        public DateTime TransmissionDateTime { get; set; }
        public string SerializedData { get; set; }
        public string SerializedFormat { get; set; }
        public string SerializedClass { get; set; }
        public string HostName { get; set; }
        public string Recipients { get; set; }
        public string RecipientsCc { get; set; }
        public string RecipientsBcc { get; set; }
        public string Sender { get; set; }
    }
}
