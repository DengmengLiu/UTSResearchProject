using Npgsql;
using System.Threading.Tasks;
using UnityEditor;

public class DBConnection
{
    static string connString = "Host=localhost;Username=postgres;Password=Aki825389654;Database=UTSResearchProject";
    public static async Task ConnectToDatabase(string connStr)
    {
        try
        {
            using (var conn = new NpgsqlConnection(connStr))
            {
                await conn.OpenAsync();

                EditorUtility.DisplayDialog("Success", "Database connected successfully!", "OK");
            }
        }
        catch (NpgsqlException ex)
        {
            EditorUtility.DisplayDialog("Error", $"Database connection failed: {ex.Message}", "OK");
        }
    }

    public static string GetConnectionString() => connString;
    public static void BuildConnectionStringFromArray(string[] strings)
    {
        connString = "Host=" + strings[0] + ";Username=" + strings[1] + ";Password=" + strings[2] + ";Database=" + strings[3];
    }
}
