using System.Data;
using GenericRepositoryPattern.Database;
using GenericRepositoryPattern.Entities;

namespace GenericRepositoryPattern.Repositories
{
    public class UserRepository : AdoRepository<User>
    {
        public UserRepository(IDbConnectionProvider dbProvider) : base(dbProvider, "Users")
        {
        }

        protected override User PopulateRecord(IDataReader reader)
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = (string)reader["Username"],
                LastName = (string)reader["LastName"],
                FirstName = (string)reader["FirstName"]
            };
        }
    }
}