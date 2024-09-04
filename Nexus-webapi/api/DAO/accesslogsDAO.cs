using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class AccessLogsDAO
{
    private readonly string _connectionString;

    public AccessLogsDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<AccessLogs>> GetAllAccessLogs()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM AccessLogs";
        dbConnection.Open();
        return await dbConnection.QueryAsync<AccessLogs>(sQuery);
    }

    public async Task<AccessLogs> GetAccessLogById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM AccessLogs WHERE log_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<AccessLogs>(sQuery, new { Id = id });
    }

    public async Task<int> InsertAccessLog(AccessLogs log)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO AccessLogs (employee_id, room_id, access_time, access_granted) VALUES(@IdEmployee, @IdRoom, @AccessTime, @AccessGranted); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, log);
    }
}