using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class EmployeeRoomAccessDAO
{
    private readonly string _connectionString;

    public EmployeeRoomAccessDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<EmployeeRoomAccess>> GetAllEmployeeRoomAccess()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM EmployeeRoomAccess";
        dbConnection.Open();
        return await dbConnection.QueryAsync<EmployeeRoomAccess>(sQuery);
    }

    public async Task<EmployeeRoomAccess> GetEmployeeRoomAccessById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM EmployeeRoomAccess WHERE access_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<EmployeeRoomAccess>(sQuery, new { Id = id });
    }

    public async Task<int> InsertEmployeeRoomAccess(EmployeeRoomAccess access)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO EmployeeRoomAccess (employee_id, room_id) VALUES(@EmployeeId, @RoomId); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, access);
    }

    public async Task<bool> DeleteEmployeeRoomAccess(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM EmployeeRoomAccess WHERE access_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}