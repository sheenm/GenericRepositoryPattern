using System.Data;

namespace Repository.Abstractions
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDatabaseConnection();
    }
}