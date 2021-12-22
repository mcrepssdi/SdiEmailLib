using System;
using System.Net.Mail;
using NLog;
using SdiEmailLib.Dao;
using SdiEmailLib.Logging;
using SdiEmailLib.Models;
using SdiEmailLib.Utilities.cs;

namespace SdiEmailLib.Processes
{
    public class SendImpl :ISend
    {
        private readonly int _applicationId;
        private readonly Host _host;
        private readonly IDao _dao;
        private readonly Logger _logger;

        public SendImpl(Host host, IDao dao, int appid, Logger logger = null)
        {
            if (appid <= 0)
            {
                throw new ArgumentException("ApplicationId is required and must be >= zero.");
            }
            _logger = logger ?? LogManager.CreateNullLogger();
            _applicationId = appid;
            _dao = dao;
            _host = host;
        }
        
        public bool Send(SdiEmail email, MailPriority mailPriority, string from, bool IsBodyHtml)
        {
            _logger?.Trace("Sending...");
            bool res = _dao.CanTransmit(_applicationId);
            if (!res)
            {
                string msg = $"ApplicationId: {_applicationId} is not enable for communications. Please refer to the Config Manager Application";
                _logger?.Error(msg);
                throw new Exception(msg);
            }
            
            MailMessage mailMsg = EmailUtility.InitMailMessage(email, mailPriority, from, IsBodyHtml);
            
            using SmtpClient client = EmailUtility.GetClient(_host);
            client.Send(mailMsg);

            EventLogger.Info(_applicationId, $"Email successfully to {string.Join(",", email.To)}", _logger);
            return true;
        }
    }
}