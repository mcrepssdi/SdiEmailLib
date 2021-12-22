using System;
using System.Data.SqlClient;
using System.Net.Mail;
using NLog;
using SdiEmailLib.Dao;
using SdiEmailLib.Models;
using SdiEmailLib.Processes;
using SdiEmailLib.Utilities.cs;

namespace SdiEmailLib
{
    public class SendStore
    {
        private readonly ISend _send;
        private readonly IDao _dao;
        private readonly Logger _logger;

        public SendStore(ISend send, IDao dao, Logger logger = null)
        {
            _send = send;
            _dao = dao;
            _logger = logger ?? LogManager.CreateNullLogger();
        }
        
        public void Send(SdiEmail email, string connStr, Host host, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            SqlTransaction transaction = null;
            try
            {
                using SqlConnection conn = new(connStr);
                transaction = conn.BeginTransaction();
                
                Transmission transmission = EmailUtility.InitTransmission(applicationId, host.HostName, host.FromAddress, email);
                _dao.Presist(transmission);
                _send.Send(email, mailPriority, host.FromAddress, isBodyHtml);
                
                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger?.Error($"Error Saving or Sending the Email. {e.Message}");
                transaction?.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
            }
        }
        
        public void Send(SdiEmail email, Host host, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            _send.Send(email, mailPriority, host.FromAddress, isBodyHtml);
            Transmission transmission = EmailUtility.InitTransmission(applicationId, host.HostName, host.FromAddress, email);
            _dao.Presist(transmission);
        }

        public void Send(SdiEmail email, string fromAddress, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            _send.Send(email, mailPriority, fromAddress, isBodyHtml);
        }
    }
}