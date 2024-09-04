using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class RolePermissionsDAO
{
    private readonly string _connectionString;

    public RolePermissionsDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<RolePermissions>> GetAllRolePermissions()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM RolePermissions";
        dbConnection.Open();
        return await dbConnection.QueryAsync<RolePermissions>(sQuery);
    }

    public async Task<RolePermissions> GetRolePermissionById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM RolePermissions WHERE role_permission_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<RolePermissions>(sQuery, new { Id = id });
    }

    public async Task<int> InsertRolePermission(RolePermissions rolePermission)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO RolePermissions (role_id, permission_id) VALUES(@RoleId, @PermissionId); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, rolePermission);
    }

    public async Task<bool> DeleteRolePermission(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM RolePermissions WHERE role_permission_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}
