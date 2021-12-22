using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using NLog;
using SdiEmailLib.Models;

namespace SdiEmailLib.Dao
{
    public class DaoImpl : IDao
    {
        private readonly Logger _logger;
        private readonly string _connStr;
        public DaoImpl(string connStr, Logger logger)
        {
            _logger = logger ?? LogManager.CreateNullLogger();
            _connStr = connStr;
        }
        
        /// <inheritdoc cref="IDao.CanTransmit"/>
        public bool CanTransmit(int appId)
        {
            _logger?.Trace($"Verifying Transmission Flag for AppId: {appId}");
            const string sql = "SELECT CanTransmit FROM Application WHERE ApplicationId = @ApplicationId";
            try
            {
                using SqlConnection conn = new(_connStr);
                DynamicParameters dp = new();
                dp.Add("@ApplicationId", appId);
                dynamic transmissionFlag = conn.Query(sql, dp).FirstOrDefault();
                _logger?.Trace($"TransmissionFlag: {transmissionFlag.CanTransmit}");
                if (transmissionFlag == null) return false;
                return Convert.ToBoolean(transmissionFlag.CanTransmit);
            }
            catch (Exception e)
            {
                _logger?.Error($"Error reading Transmission Flag for for AppId: {appId}.  Msg: {e.Message}");
            }
            return false;
        }

        /// <inheritdoc cref="IDao.Presist(Transmission)"/>
        public bool Presist(Transmission transmission)
        {
            _logger?.Trace("Entering...");
            StringBuilder sb = new();
            sb.Append("INSERT INTO ElectronicTransmission ");
            sb.Append(
                " (ApplicationId, TransmissionDataTime, SerializedData, SerializedFormat, SerializedClass, HostName, Recipients, Sender) ");
            sb.Append(" VALUES ");
            sb.Append(
                " (@ApplicationId, @TransmissionDataTime, @SerializedData, @SerializedFormat, @SerializedClass, @HostName, @Recipients, @Sender) ");

            DynamicParameters dp = new();
            dp.Add("@ApplicationId", transmission.ApplicationId);
            dp.Add("@TransmissionDataTime", DateTime.Now);
            dp.Add("@SerializedData", transmission.SerializedData);
            dp.Add("@SerializedFormat", transmission.SerializedFormat);
            dp.Add("@SerializedClass", transmission.SerializedClass);
            dp.Add("@HostName", transmission.HostName);
            dp.Add("@Recipients", transmission.Recipients);
            dp.Add("@RecipientsCc", transmission.RecipientsCc);
            dp.Add("@RecipientsBcc", transmission.RecipientsBcc);
            dp.Add("@Sender", transmission.Sender);

            using SqlConnection conn = new(_connStr);
            conn.Open();
            conn.Execute(sb.ToString(), dp);
            return true;
        }
        
        /// <inheritdoc cref="IDao.Presist(Transmission, SqlConnection, SqlTransaction)"/>
        public bool Presist(Transmission transmission, SqlConnection conn, SqlTransaction transaction)
        {
            _logger?.Trace("Entering...");
            StringBuilder sb = new();
            sb.Append("INSERT INTO ElectronicTransmission ");
            sb.Append(
                " (ApplicationId, TransmissionDataTime, SerializedData, SerializedFormat, SerializedClass, HostName, Recipients, Sender) ");
            sb.Append(" VALUES ");
            sb.Append(
                " (@ApplicationId, @TransmissionDataTime, @SerializedData, @SerializedFormat, @SerializedClass, @HostName, @Recipients, @Sender) ");

            DynamicParameters dp = new();
            dp.Add("@ApplicationId", transmission.ApplicationId);
            dp.Add("@TransmissionDataTime", DateTime.Now);
            dp.Add("@SerializedData", transmission.SerializedData);
            dp.Add("@SerializedFormat", transmission.SerializedFormat);
            dp.Add("@SerializedClass", transmission.SerializedClass);
            dp.Add("@HostName", transmission.HostName);
            dp.Add("@Recipients", transmission.Recipients);
            dp.Add("@RecipientsCc", transmission.RecipientsCc);
            dp.Add("@RecipientsBcc", transmission.RecipientsBcc);
            dp.Add("@Sender", transmission.Sender);
            
            conn.Execute(sb.ToString(), dp, transaction: transaction);
            return true;
        }
    }
}