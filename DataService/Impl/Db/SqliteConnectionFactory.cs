using System.Data;
using DataService.Interfaces;
using Microsoft.Data.Sqlite;

namespace DataService.Impl.Db;

internal sealed class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection Create()
    {
        return new SqliteConnection(_connectionString);
    }
}