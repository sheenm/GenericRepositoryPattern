using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Repository.Abstractions
{
    public abstract class AdoRepository<T> : IRepository<T>
    {
        IDbConnectionProvider _dbProvider;
        private string _tableName;
        private PropertyInfo[] _objectProperties;

        public AdoRepository(IDbConnectionProvider dbProvider, string tableName)
        {
            _dbProvider = dbProvider;
            _tableName = tableName;
            _objectProperties = typeof(T).GetProperties();
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

                command.CommandText = $"SELECT * FROM {_tableName} WHERE Id=@ID";
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
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                foreach (var property in _objectProperties)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = GetParameterName(property);
                    parameter.Value = property.GetValue(item);
                    parameter.Direction = ParameterDirection.Input;
                    parameter.DbType = GetDbType(property.GetType());
                    command.Parameters.Add(parameter);
                }

                command.CommandText = GetCommandTextForSave(item);
                command.CommandType = CommandType.Text;

                return command.ExecuteNonQuery() == 1;
            }
        }

        private string GetCommandTextForSave(T item)
        {
            var commandTextBuilder = new StringBuilder($"UPDATE {_tableName} SET");
            foreach (var property in _objectProperties)
            {
                commandTextBuilder.Append($" {property.Name}={GetParameterName(property)}, ");
            }
            commandTextBuilder.Remove(commandTextBuilder.Length - 1, 1);
            commandTextBuilder.Append($" WHERE  Id={GetId(item)}");

            return commandTextBuilder.ToString();
        }

        private static string GetParameterName(PropertyInfo propertyInfo)
        {
            return $"@{propertyInfo.Name}";
        }

        private DbType GetDbType(Type type)
        {
            if (type == typeof(int))
            {
                return DbType.Int32;
            }
            if (type == typeof(string))
            {
                return DbType.String;
            }
            if (type == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            if (type == typeof(byte[]))
            {
                return DbType.Binary;
            }

            throw new NotImplementedException($"There's no converter for type {type.Name} in AdoRepository");
        }

        private int GetId(T item)
        {
            foreach (var property in _objectProperties)
            {
                if (property.Name == "Id")
                    return (int)property.GetValue(item);
            }

            throw new ArgumentException($"Id property was not found in {typeof(T).Name} ");
        }


        protected abstract T PopulateRecord(IDataReader reader);
    }
}