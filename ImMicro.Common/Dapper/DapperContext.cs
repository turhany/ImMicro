using System;
using System.Data;
using Npgsql;

namespace ImMicro.Common.Dapper;

public class DapperContext
{
    private readonly string _connectionString;
    private readonly ConnectionType _connectionType;
    public DapperContext(string connectionString, ConnectionType connectionType)
    {
        _connectionString = connectionString;
        _connectionType = connectionType;
    }

    public IDbConnection CreateConnection()
    {
        switch (_connectionType)
        {
            case ConnectionType.PostgreSql:
                return new NpgsqlConnection(_connectionString);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum ConnectionType
    {
        PostgreSql = 1
    }
}