using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class RolesDAO
{
    private readonly string _connectionString;

    public RolesDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Roles>> GetAllRoles()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Roles";
        dbConnection.Open();
        return await dbConnection.QueryAsync<Roles>(sQuery);
    }

    public async Task<Roles> GetRoleById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Roles WHERE role_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<Roles>(sQuery, new { Id = id });
    }

    public async Task<int> InsertRole(Roles role)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO Roles (role_name, description) VALUES(@RoleName, @Description); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, role);
    }

    public async Task<bool> UpdateRole(Roles role)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "UPDATE Roles SET role_name = @RoleName, description = @Description WHERE role_id = @RoleId";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, role) > 0;
    }

    public async Task<bool> DeleteRole(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM Roles WHERE role_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}