namespace Nexus_webapi.Repository;
using Nexus_webapi.Utils;

public class MySqlConnectionFactory
{
    public static MySqlConnection GetConnection()
    {
        List<string> constants = new GetConstants().getConstants();
        string server = constants[0];
        string port = constants[1];
        string database = constants[2];
        string uid = constants[3];
        string pwd = constants[4];

        string connectionString = "server=" + server + ";" +
                                  "port=" + port + ";" +
                                  "database=" + database + ";" +
                                  "uid=" + uid + ";" +
                                  "pwd=" + pwd + ";";

        return new MySqlConnection(connectionString);
    }
}