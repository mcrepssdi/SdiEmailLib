using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using NLog;
using SdiEmailLib.Enums;
using SdiEmailLib.Models;
using SdiEmailLib.Utilities.cs;

namespace SdiEmailLib.Processes
{
    public class ReceiveImpl : IReceive
    {
        private readonly Logger _logger;
        private readonly ExchangeConnector _connector;
        private const int _pageSize = 100;

        public ReceiveImpl(Logger logger, ExchangeConnector exchangeConnector)
        {
            _logger = logger ?? LogManager.CreateNullLogger();
            _connector = exchangeConnector;
        }
        
        /// <inheritdoc cref="IReceive.Receive(string, WellKnownFolderName, FolderTraversal, SmtpReceivingTypes)"/>
        public List<EmailMessage> Receive(string foldername, WellKnownFolderName wellknownfoldername, FolderTraversal folderTraversal, SmtpReceivingTypes receivingType)
        {
            _logger?.Trace("Fetching Emails...");
            return ReadEmail( foldername, folderTraversal, wellknownfoldername, receivingType, string.Empty);
        }

        /// <inheritdoc cref="IReceive.Receive(string, WellKnownFolderName, SmtpReceivingTypes) "/>
        public List<EmailMessage> Receive(string foldername, WellKnownFolderName wellknownfoldername, SmtpReceivingTypes receivingType)
        {
            _logger?.Trace("Fetching Emails...");
            return ReadEmail(foldername, FolderTraversal.Shallow, wellknownfoldername, receivingType, string.Empty);
        }

        /// <inheritdoc cref="IReceive.IReceive(string, SmtpReceivingTypes, string)"/>
        public List<EmailMessage> Receive(string foldername, SmtpReceivingTypes receivingType, string querystring = "")
        {
            _logger?.Trace("Fetching Emails...");
            return ReadEmail(foldername, FolderTraversal.Shallow, WellKnownFolderName.Inbox, receivingType, querystring);
        }

        /// <inheritdoc cref="IReceive.Receive(SmtpReceivingTypes, string)"/>
        public List<EmailMessage> Receive(SmtpReceivingTypes receivingType, string querystring = "")
        {
            _logger?.Trace("Fetching Emails...");
            return ReadEmail(string.Empty, FolderTraversal.Shallow, WellKnownFolderName.Inbox, receivingType, querystring);
        }

        /// <inheritdoc cref="IReceive.GetFolder(string)"/>
        public FolderId GetFolder(string foldername)
        {
            _logger?.Trace("Retreiving Exchange FolerId...");
            ExchangeService exchangeservice = EmailUtil.GetExchangeService(_connector);
            FolderView view = new (_pageSize)
            {
                PropertySet = new PropertySet(BasePropertySet.IdOnly) { FolderSchema.DisplayName },
                Traversal = FolderTraversal.Deep
            };
            SearchFilter searchFilter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, foldername);
            Task<FindFoldersResults> findFolderResults = exchangeservice.FindFolders(WellKnownFolderName.Root, searchFilter, view);
            FindFoldersResults results = findFolderResults.Result;
            
            Folder folder = results.Folders.FirstOrDefault(result => result.DisplayName.Equals(foldername));
            return folder?.Id;
        }
        
        /// <inheritdoc cref="IReceive.GetFolder(string,FolderTraversal,WellKnownFolderName)"/>
        public Folder GetFolder(string foldername, FolderTraversal folderTraversal, WellKnownFolderName wellknownfoldername)
        {
            _logger?.Trace("Entering...");
            if (foldername.IsEmpty()) return null;

            ExchangeService exchangeservice = EmailUtil.GetExchangeService(_connector);
            FolderView folderView = new (_pageSize)
            {
                PropertySet = new PropertySet(BasePropertySet.IdOnly) {FolderSchema.DisplayName},
                Traversal = folderTraversal
            };

            List<SearchFilter> FolderSearchFilterCollection = new ()
            {
                new SearchFilter.IsEqualTo(FolderSchema.DisplayName, foldername)
            };
            SearchFilter FolderSearchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, FolderSearchFilterCollection.ToArray());
            
            Task<FindFoldersResults> findFolderResults = exchangeservice.FindFolders(wellknownfoldername, FolderSearchFilter, folderView);
            FindFoldersResults results = findFolderResults.Result;
            Folder folder = (from s in results where s.DisplayName.Equals(foldername) select s).FirstOrDefault();
            return folder;
        }

        /// <inheritdoc cref="IReceive.GetEmailItems"/>
        public FindItemsResults<Item> GetEmailItems(Folder folder, WellKnownFolderName wellknownfoldername, string querystring)
        {
            _logger?.Trace("Entering...");
            ExchangeService exchangeservice = EmailUtil.GetExchangeService(_connector);
            List<SearchFilter> SearchFilterCollection = new ();
            ItemView View = new(_pageSize);

            SearchFilterCollection.Add(new SearchFilter.IsEqualTo(ItemSchema.HasAttachments, true));
            //SearchFilterCollection.Add(new SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Now.AddMinutes(MinutesToSearch)));
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, SearchFilterCollection.ToArray());

            View.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.DateTimeReceived);
            View.OrderBy.Add(ItemSchema.DateTimeReceived, SortDirection.Descending);
            View.Traversal = ItemTraversal.Shallow;

            Task<FindItemsResults<Item>> task;
            if (querystring.IsEmpty())
            {
                task = folder switch
                {
                    null => exchangeservice.FindItems(wellknownfoldername, querystring, View),
                    _ => exchangeservice.FindItems(folder.Id, querystring, View)
                };
            }
            else
            {
                task = folder.Id switch
                {
                    null => exchangeservice.FindItems(wellknownfoldername, searchFilter, View),
                    _ => exchangeservice.FindItems(folder.Id, searchFilter, View)
                };
            }
            return task.Result;
        }

        /// <inheritdoc cref="IReceive.GetEmails"/>
        public List<EmailMessage> GetEmails(PropertySet propertyset, SmtpReceivingTypes receivingType, IEnumerable<Item> items)
        {
            _logger?.Trace("Entering...");
            ExchangeService exchangeservice = EmailUtil.GetExchangeService(_connector);
            List<EmailMessage> results = new();

            IEnumerable<Task<EmailMessage>> msgTasks;
            List<Task<EmailMessage>> task = items.Select(item => EmailMessage.Bind(exchangeservice, item.Id, propertyset)).ToList();
            switch (receivingType)
            {
                case SmtpReceivingTypes.NotRead:
                    msgTasks = (from s in task where !s.Result.IsRead select s);
                    results.AddRange(msgTasks.Select(msg => EmailMessage.Bind(exchangeservice, msg.Result.Id, propertyset))
                        .Select(emailMsg => emailMsg.Result));
                    break;
                case SmtpReceivingTypes.Read:
                    msgTasks = (from s in task where s.Result.IsRead select s);
                    results.AddRange(msgTasks.Select(msg => EmailMessage.Bind(exchangeservice, msg.Result.Id, propertyset))
                        .Select(emailMsg => emailMsg.Result));
                    break;
                case SmtpReceivingTypes.All:
                    msgTasks = (from s in task select s);
                    results.AddRange(msgTasks.Select(msg => EmailMessage.Bind(exchangeservice, msg.Result.Id, propertyset))
                        .Select(emailMsg => emailMsg.Result));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(receivingType), receivingType, null);
            }
            
            return results;
        }

        #region Private Methods/Helpers

        private List<EmailMessage> ReadEmail(string foldername, FolderTraversal folderTraversal,
            WellKnownFolderName wellknownfoldername, SmtpReceivingTypes receivingType, string querystring = "")
        {
            _logger?.Trace("Fetching Emails...");

            Folder folder = GetFolder(foldername, folderTraversal, wellknownfoldername);
            FindItemsResults<Item> items = GetEmailItems(folder, wellknownfoldername, querystring);
            PropertySet propertyset = new ()
            {
                ItemSchema.Attachments,
                ItemSchema.HasAttachments,
                ItemSchema.MimeContent,
                ItemSchema.DisplayTo,
                ItemSchema.DisplayCc,
                ItemSchema.Id,
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                EmailMessageSchema.Sender,
                EmailMessageSchema.IsRead
            };
            return GetEmails(propertyset, receivingType, items);
        }
        
        #endregion
    }
}