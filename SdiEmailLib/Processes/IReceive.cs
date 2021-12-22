using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;
using SdiEmailLib.Enums;

namespace SdiEmailLib.Processes
{
    public interface IReceive
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="wellknownfoldername"></param>
        /// <param name="folderTraversal"></param>
        /// <param name="receivingType"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string foldername, WellKnownFolderName wellknownfoldername, FolderTraversal folderTraversal, SmtpReceivingTypes receivingType);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="wellknownfoldername"></param>
        /// <param name="receivingType"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string foldername, WellKnownFolderName wellknownfoldername, SmtpReceivingTypes receivingType);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="receivingType"></param>
        /// <param name="querystring"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string foldername, SmtpReceivingTypes receivingType, string querystring = "");
        
        /// <summary>
        /// Sends the Email from the specified host
        /// </summary>
        /// <param name="receivingType"></param>
        /// <param name="querystring">Used to Filter Emails Returned</param>
        /// <returns></returns>
        List<EmailMessage> Receive(SmtpReceivingTypes receivingType, string querystring = "");
        
        /// <summary>
        /// Fetches the Microfosft Folder ID from the Exchange Server
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        FolderId GetFolder(string foldername);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foldername"></param>
        /// <param name="folderTraversal"></param>
        /// <param name="wellknownfoldername"></param>
        /// <returns></returns>
        Folder GetFolder(string foldername, FolderTraversal folderTraversal, WellKnownFolderName wellknownfoldername);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="wellknownfoldername"></param>
        /// <param name="querystring"></param>
        /// <returns></returns>
        FindItemsResults<Item> GetEmailItems(Folder folder, WellKnownFolderName wellknownfoldername, string querystring);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyset"></param>
        /// <param name="receivingType"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        List<EmailMessage> GetEmails(PropertySet propertyset, SmtpReceivingTypes receivingType, IEnumerable<Item> items);
    }
}