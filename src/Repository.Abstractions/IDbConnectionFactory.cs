using System.Data.Common;

namespace Repository.Abstractions
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}