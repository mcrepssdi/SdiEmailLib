using System.Net.Mail;
using SdiEmailLib.Models;

namespace SdiEmailLib.Processes
{
    public interface ISend
    {
        /// <summary>
        ///  Sends an Email communication without persisting the email message.
        /// </summary>
        /// <param name="email">Email Message</param>
        /// <param name="fromAddress">Email Address of the Sender</param>
        /// <param name="mailPriority">MailPriority used when sending the email. HIGH, NORMAL and LOW are the valid mail priority options</param>
        /// <param name="isBodyHtml">True/Fals used to indicate when the Email Body is formatted as HTML</param>
        public void Send(SdiEmail email, string fromAddress, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false);

        /// <summary>
        /// Sends an Email communication while attempting to persist the email message.
        /// The Email will always be sent, regardless of the persisting.
        /// </summary>
        /// <param name="email">Email Message</param>
        /// <param name="host">Hosting Information</param>
        /// <param name="applicationId">Calling Application ID in Config Manager</param>
        /// <param name="mailPriority">MailPriority used when sending the email. HIGH, NORMAL and LOW are the valid mail priority options</param>
        /// <param name="isBodyHtml">True/Fals used to indicate when the Email Body is formatted as HTML</param>
        public void Send(SdiEmail email, Host host, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false);

        /// <summary>
        /// Sends an Email communication while persisting the email message.
        /// An failure will result in a rollback and the email message will not be sent.
        /// </summary>
        /// <param name="email">Email Message</param>
        /// <param name="host">Hosting Information</param>
        /// <param name="connStr">Connection String using to Store the Serialized Email Message</param>
        /// <param name="applicationId">Calling Application ID in Config Manager</param>
        /// <param name="mailPriority">MailPriority used when sending the email. HIGH, NORMAL and LOW are the valid mail priority options</param>
        /// <param name="isBodyHtml">True/Fals used to indicate when the Email Body is formatted as HTML</param>
        public void Send(SdiEmail email, Host host, string connStr, int applicationId, MailPriority mailPriority = MailPriority.Normal, bool isBodyHtml = false);
    }
}