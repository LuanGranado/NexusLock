using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class PermissionsDAO
{
    private readonly string _connectionString;

    public PermissionsDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Permissions>> GetAllPermissions()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Permissions";
        dbConnection.Open();
        return await dbConnection.QueryAsync<Permissions>(sQuery);
    }

    public async Task<Permissions> GetPermissionById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Permissions WHERE permission_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<Permissions>(sQuery, new { Id = id });
    }

    public async Task<int> InsertPermission(Permissions permission)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO Permissions (permission_name, description) VALUES(@PermissionName, @Description); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, permission);
    }

    public async Task<bool> UpdatePermission(Permissions permission)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "UPDATE Permissions SET permission_name = @PermissionName, description = @Description WHERE permission_id = @PermissionId";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, permission) > 0;
    }

    public async Task<bool> DeletePermission(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM Permissions WHERE permission_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}
