using System.Net.Mail;
using SdiEmailLib.Models;

namespace SdiEmailLib.Processes
{
    public interface ISend
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="mailPriority"></param>
        /// <param name="from"></param>
        /// <param name="IsBodyHtml"></param>
        /// <returns></returns>
        bool Send(SdiEmail email, MailPriority mailPriority, string from, bool IsBodyHtml);
    }
}