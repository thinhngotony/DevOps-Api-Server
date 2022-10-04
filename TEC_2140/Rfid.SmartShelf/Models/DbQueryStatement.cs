using Vjp.Rfid.SmartShelf.Enums;

namespace Vjp.Rfid.SmartShelf.Models
{
    public class DbQueryStatement
    {
        public DbQueryStatement()
        {
        }

        public string QueryStr { get; set; } = string.Empty;
        public DbQueryType DbQueryType { get; set; } = DbQueryType.SelectQuery;
    }
}
