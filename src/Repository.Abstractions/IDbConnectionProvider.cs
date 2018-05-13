using System.Data.Common;

namespace Repository.Abstractions
{
    public interface IDbConnectionProvider
    {
        DbConnection GetDatabaseConnection();
    }
}