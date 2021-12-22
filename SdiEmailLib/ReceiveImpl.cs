using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace SdiEmailLib
{
    public class ReceiveImpl : IReceive
    {
        public List<EmailMessage> Receive(string uri, string foldername, WellKnownFolderName wellknownfoldername, FolderTraversal folderTraversal)
        {
            throw new System.NotImplementedException();
        }

        public List<EmailMessage> Receive(string uri, string foldername, WellKnownFolderName wellknownfoldername)
        {
            throw new System.NotImplementedException();
        }

        public List<EmailMessage> Receive(string uri, string foldername, string querystring = "")
        {
            throw new System.NotImplementedException();
        }

        public List<EmailMessage> Receive(string querystring = "")
        {
            throw new System.NotImplementedException();
        }

        public FolderId GetFolder(string foldername)
        {
            throw new System.NotImplementedException();
        }

        public Folder GetFolder(string foldername, FolderTraversal folderTraversal, WellKnownFolderName wellknownfoldername)
        {
            throw new System.NotImplementedException();
        }

        public FindItemsResults<Item> GetEmailItems(Folder folder, WellKnownFolderName wellknownfoldername, string querystring)
        {
            throw new System.NotImplementedException();
        }

        public List<EmailMessage> GetEmails(PropertySet propertyset, IEnumerable<Item> items)
        {
            throw new System.NotImplementedException();
        }
    }
}