namespace SdiEmailLib.Models
{
    public class Host
    {
        public string HostName { get; set; }
        public int Port { get; set; } = 557;
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string Uri { get; set; }
    }
}