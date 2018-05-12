using System.Data;

namespace GenericRepositoryPattern.Database
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetDatabaseConnection();
    }
}