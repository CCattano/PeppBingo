using System.Data.SqlClient;

namespace Pepp.Web.Apps.Bingo.Data
{
    public abstract class BaseDataService
    {
        private readonly string _connStr;
        public BaseDataService(string connStr) => _connStr = connStr;
        public SqlConnection GetConnection() => new(_connStr);
    }
}