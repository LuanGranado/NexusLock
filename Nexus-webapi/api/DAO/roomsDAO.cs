using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Nexus_webapi.Models;

namespace Nexus_webapi.DAO;

public class RoomsDAO
{
    private readonly string _connectionString;

    public RoomsDAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Rooms>> GetAllRooms()
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Rooms";
        dbConnection.Open();
        return await dbConnection.QueryAsync<Rooms>(sQuery);
    }

    public async Task<Rooms> GetRoomById(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "SELECT * FROM Rooms WHERE room_id = @Id";
        dbConnection.Open();
        return await dbConnection.QueryFirstOrDefaultAsync<Rooms>(sQuery, new { Id = id });
    }

    public async Task<int> InsertRoom(Rooms room)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "INSERT INTO Rooms (room_name, room_description) VALUES(@RoomName, @RoomDescription); SELECT LAST_INSERT_ID();";
        dbConnection.Open();
        return await dbConnection.ExecuteScalarAsync<int>(sQuery, room);
    }

    public async Task<bool> UpdateRoom(Rooms room)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "UPDATE Rooms SET room_name = @RoomName, room_description = @RoomDescription WHERE room_id = @RoomId";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, room) > 0;
    }

    public async Task<bool> DeleteRoom(int id)
    {
        using IDbConnection dbConnection = new MySqlConnection(_connectionString);
        string sQuery = "DELETE FROM Rooms WHERE room_id = @Id";
        dbConnection.Open();
        return await dbConnection.ExecuteAsync(sQuery, new { Id = id }) > 0;
    }
}
