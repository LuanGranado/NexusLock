using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class EmployeesDAO
{
    private readonly string _connectionString;

    public EmployeesDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Employees>> GetAllEmployees()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Employees";
        dbConnection.Open();
        return await dbConnection.QueryAsync<Employees>(sQuery);
    }

    public async Task<Employees> GetEmployeeById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Employees WHERE employee_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<Employees>(sQuery, new { Id = id });
    }

    public async Task<int> InsertEmployee(Employees employee)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO Employees (name, pin_code, fingerprint_data) VALUES(@Name, @PinCode, @FingerprintData); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, employee);
    }

    public async Task<bool> UpdateEmployee(Employees employee)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "UPDATE Employees SET name = @Name, pin_code = @PinCode, fingerprint_data = @FingerprintData WHERE employee_id = @EmployeeId";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, employee) > 0;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM Employees WHERE employee_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}
