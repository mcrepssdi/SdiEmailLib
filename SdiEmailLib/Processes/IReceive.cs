using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace SdiEmailLib.Processes
{
    public interface IReceive
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="foldername"></param>
        /// <param name="wellknownfoldername"></param>
        /// <param name="folderTraversal"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string uri, string foldername, WellKnownFolderName wellknownfoldername, FolderTraversal folderTraversal);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="foldername"></param>
        /// <param name="wellknownfoldername"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string uri, string foldername, WellKnownFolderName wellknownfoldername);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="foldername"></param>
        /// <param name="querystring"></param>
        /// <returns></returns>
        List<EmailMessage> Receive(string uri, string foldername, string querystring = "");
        
        /// <summary>
        /// Sends the Email from the specified host
        /// </summary>
        /// <param name="querystring">Used to Filter Emails Returned</param>
        /// <returns></returns>
        List<EmailMessage> Receive(string querystring = "");
        
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
        /// <param name="items"></param>
        /// <returns></returns>
        List<EmailMessage> GetEmails(PropertySet propertyset, IEnumerable<Item> items);
    }
}