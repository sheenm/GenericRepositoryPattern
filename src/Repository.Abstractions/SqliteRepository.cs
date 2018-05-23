using System.Data;
using System.Text;

namespace Repository.Abstractions
{
    public abstract class SqliteRepository<T> : AdoRepository<T>
    {
        public SqliteRepository(IDbConnectionProvider dbProvider, string tableName) : base(dbProvider, tableName)
        {
        }

        internal override string GetCommandTextForCreate()
        {
            var commandTextBuilder = new StringBuilder($"INSERT INTO {_tableName}(");
            foreach (var property in _objectPropertiesWithoutId)
            {
                commandTextBuilder.Append($"{property.Name},");
            }
            commandTextBuilder.Replace(',', ')', commandTextBuilder.Length - 1, 1);
            commandTextBuilder.Append(" VALUES(");

            foreach (var property in _objectPropertiesWithoutId)
            {
                commandTextBuilder.Append($"{GetParameterName(property)},");
            }
            commandTextBuilder.Replace(',', ')', commandTextBuilder.Length - 1, 1);
            commandTextBuilder.Append(";SELECT last_insert_rowid();");

            return commandTextBuilder.ToString();
        }
    }
}