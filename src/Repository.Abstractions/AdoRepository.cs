using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstractions
{
    public abstract class AdoRepository<T> : IRepository<T>
    {
        IDbConnectionProvider _dbProvider;
        protected string _tableName;
        protected IEnumerable<PropertyInfo> _objectPropertiesWithoutId;
        protected PropertyInfo _idProperty;

        public AdoRepository(IDbConnectionProvider dbProvider, string tableName)
        {
            _dbProvider = dbProvider;
            _tableName = tableName;
            var properties = typeof(T).GetProperties();
            _idProperty = properties.FirstOrDefault(x => x.Name == "Id");
            _objectPropertiesWithoutId = properties.Where(prop => prop.Name != "Id");
        }

        ///<inheritdoc/>
        public async Task<T> GetByIdAsync(int id)
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

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return PopulateRecord(reader);
                    }

                    return default(T);
                }
            }
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM {_tableName}";
                command.CommandType = CommandType.Text;

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var readerResults = new List<T>();

                    while (await reader.ReadAsync())
                    {
                        readerResults.Add(PopulateRecord(reader));
                    }

                    return readerResults;
                }
            }
        }

        ///<inheritdoc/>
        public async Task<bool> SaveAsync(T item)
        {
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                foreach (var property in _objectPropertiesWithoutId)
                {
                    command.Parameters.Add(CreateDbParameterFromProperty(command, property, item));
                }

                command.CommandText = GetCommandTextForSave(item);
                command.CommandType = CommandType.Text;

                await connection.OpenAsync();
                return (await command.ExecuteNonQueryAsync()) == 1;
            }
        }

        ///<inheritdoc/>
        public async Task<int> CreateAsync(T item)
        {
            using (var connection = _dbProvider.GetDatabaseConnection())
            using (var command = connection.CreateCommand())
            {
                foreach (var property in _objectPropertiesWithoutId)
                {
                    command.Parameters.Add(CreateDbParameterFromProperty(command, property, item));
                }

                command.CommandText = GetCommandTextForCreate();
                command.CommandType = CommandType.Text;

                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        internal IDbDataParameter CreateDbParameterFromProperty(IDbCommand command, PropertyInfo property, T item)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = GetParameterName(property);
            parameter.Value = property.GetValue(item);
            parameter.Direction = ParameterDirection.Input;
            parameter.DbType = GetDbType(property.PropertyType);
            return parameter;
        }

        internal static string GetParameterName(PropertyInfo propertyInfo)
        {
            return $"@{propertyInfo.Name}";
        }

        internal static DbType GetDbType(Type type)
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


        internal string GetCommandTextForSave(T item)
        {
            var commandTextBuilder = new StringBuilder($"UPDATE {_tableName} SET ");
            foreach (var property in _objectPropertiesWithoutId)
            {
                commandTextBuilder.Append($"{property.Name}={GetParameterName(property)}, ");
            }
            commandTextBuilder.Remove(commandTextBuilder.Length - 2, 1);
            commandTextBuilder.Append($"WHERE Id={GetId(item)}");

            return commandTextBuilder.ToString();
        }

        internal virtual string GetCommandTextForCreate()
        {
            var commandTextBuilder = new StringBuilder($"INSERT INTO {_tableName}(");
            foreach (var property in _objectPropertiesWithoutId)
            {
                commandTextBuilder.Append($"{property.Name},");
            }
            commandTextBuilder.Replace(',', ')', commandTextBuilder.Length - 1, 1);
            commandTextBuilder.Append(" output INSERTED.ID VALUES(");

            foreach (var property in _objectPropertiesWithoutId)
            {
                commandTextBuilder.Append($"{GetParameterName(property)},");
            }
            commandTextBuilder.Replace(',', ')', commandTextBuilder.Length - 1, 1);

            return commandTextBuilder.ToString();
        }

        internal int GetId(T item)
        {
            return _idProperty != null
            ? (int)_idProperty.GetValue(item)
            : throw new ArgumentException($"Id property was not found in {typeof(T).Name} ");
        }

        protected abstract T PopulateRecord(IDataReader reader);
    }
}