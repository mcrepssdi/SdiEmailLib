using System;
using System.Data.SqlClient;
using System.Net.Mail;
using NLog;
using SdiEmailLib.Dao;
using SdiEmailLib.Logging;
using SdiEmailLib.Models;
using SdiEmailLib.Utilities.cs;

namespace SdiEmailLib.Processes
{
    public class EmailSender :ISend
    {
        private readonly int _applicationId;
        private readonly Host _host;
        private readonly IDao _dao;
        private readonly Logger _logger;

        public EmailSender(Host host, IDao dao, int appid, Logger logger = null)
        {
            _logger = logger ?? LogManager.CreateNullLogger();
            _applicationId = appid;
            _dao = dao;
            _host = host;
            if (appid <= 0)
            {
                throw new ArgumentException("ApplicationId is required and must be >= zero.");
            }
            bool res = _dao.CanTransmit(_applicationId);
            if (!res)
            {
                string msg = $"ApplicationId: {_applicationId} is not enable for communications. Please refer to the Config Manager Application";
                _logger?.Error(msg);
                throw new Exception(msg);
            }
        }
        
        /// <inheritdoc cref="ISend.Send(SdiEmail, string, MailPriority, bool)"/>
        public void Send(SdiEmail email, string fromAddress, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            SendMail(email, mailPriority, fromAddress, isBodyHtml);
        }
        
        /// <inheritdoc cref="ISend.Send(SdiEmail, Host, int, MailPriority, bool)"/>
        public void Send(SdiEmail email, Host host, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            SendMail(email, mailPriority, host.FromAddress, isBodyHtml);
            Transmission transmission = EmailUtil.InitTransmission(applicationId, host.HostName, host.FromAddress, email);
            _dao.Presist(transmission);
        }
        
        /// <inheritdoc cref="ISend.Send(SdiEmail, Host, string, int, MailPriority, bool)"/>
        public void Send(SdiEmail email, Host host, string connStr, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _logger?.Trace("Entering...");
            SqlTransaction transaction = null;
            try
            {
                using SqlConnection conn = new(connStr);
                transaction = conn.BeginTransaction();
                
                Transmission transmission = EmailUtil.InitTransmission(applicationId, host.HostName, host.FromAddress, email);
                _dao.Presist(transmission, conn, transaction);
                SendMail(email, mailPriority, host.FromAddress, isBodyHtml);
                
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

        
        #region Private Method/Helpers
        private bool SendMail(SdiEmail email, MailPriority mailPriority, string fromAddress, bool IsBodyHtml)
        {
            _logger?.Trace("Sending...");
            MailMessage mailMsg = EmailUtil.InitMailMessage(email, mailPriority, fromAddress, IsBodyHtml);
            
            using SmtpClient client = EmailUtil.GetClient(_host);
            client.Send(mailMsg);

            EventLogger.Info(_applicationId, $"Email successfully to {string.Join(",", email.To)}", _logger);
            return true;
        }
        
        #endregion
        
    }
}