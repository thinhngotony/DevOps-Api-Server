using System.Data;

namespace Vjp.Rfid.SmartShelf.Db
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
