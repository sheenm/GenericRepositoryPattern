using System;
using System.Collections.Generic;
using System.Data;
using GenericRepositoryPattern.Database;
using System.Linq;

namespace GenericRepositoryPattern.Repositories
{
    public abstract class AdoRepository<T> : IRepository<T>
    {
        IDbConnectionProvider _dbProvider;
        private string _tableName;

        public AdoRepository(IDbConnectionProvider dbProvider, string tableName)
        {
            _dbProvider = dbProvider;
            _tableName = tableName;
        }

        public T GetById(int id)
        {
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@ID";
                parameter.Value = id;
                parameter.Direction = ParameterDirection.Input;
                parameter.DbType = DbType.Int32;

                command.CommandText = $"SELECT * FROM {_tableName} WHERE id=@ID";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return PopulateRecord(reader);
                    }

                    return default(T);
                }
            }
        }

        public IEnumerable<T> GetByQuery(Func<T, bool> selector)
        {
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM {_tableName}";
                command.CommandType = CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    var readerResults = new List<T>();

                    while (reader.Read())
                    {
                        readerResults.Add(PopulateRecord(reader));
                    }

                    return readerResults.Where(selector);
                }
            }
        }

        public bool Save(T item)
        {
            throw new NotImplementedException();
        }

        protected abstract T PopulateRecord(IDataReader reader);
    }
}