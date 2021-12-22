using System.Data.SqlClient;
using SdiEmailLib.Models;

namespace SdiEmailLib.Dao
{
    public interface IDao
    {
        public bool CanTransmit(int appId);

        public bool Presist(Transmission transmission, SqlTransaction transaction = null);
    }
}