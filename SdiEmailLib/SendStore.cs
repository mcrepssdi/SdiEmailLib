using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Transactions;
using SdiEmailLib.Dao;
using SdiEmailLib.Models;

namespace SdiEmailLib
{
    public class SendStore
    {
        private readonly ISend _send;
        private readonly IDao _dao;

        public SendStore(ISend send, IDao dao, string connstr)
        {
            _send = send;
            _dao = dao;
        }
        
        public void Send(SdiEmail email, SqlConnection conn, Transaction transaction = null)
        {
            try
            {
                Transmission transmission = new Transmission()
                
                _dao.Presist()
            }
            catch (Exception e)
            {
                
            }
        }

        public void Send(SdiEmail email, string fromAddress, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false)
        {
            _send.Send(email, mailPriority, fromAddress, isBodyHtml);
        }
        
        
        
        
    }
}