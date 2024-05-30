using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

public class DatabaseController
{
    private static string connectionString = DBConnection.GetConnectionString();

    public static async Task<List<T>> ExecuteQueryAsync<T>(Func<NpgsqlDataReader, T> createFromReader, string sqlQuery)
    {
        List<T> data = new List<T>();

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (var cmd = new NpgsqlCommand(sqlQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        T item = createFromReader(reader);
                        data.Add(item);
                    }
                }
            }
        }
        catch (NpgsqlException ex)
        {
            EditorUtility.DisplayDialog("Error", $"Error executing query: {ex.Message}", "OK");
        }

        return data;
    }

    public static async Task<bool> ExecuteNonQueryAsync(string sqlQuery)
    {
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (var cmd = new NpgsqlCommand(sqlQuery, conn))
                {
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (NpgsqlException ex)
        {
            EditorUtility.DisplayDialog("Error", $"Error executing non-query: {ex.Message}", "OK");
            return false;
        }
    }
    public static async Task<NPC> GetNPCByIDAsync(int npcID)
    {
        string sqlQuery = $"SELECT * FROM NPC WHERE npc_id = {npcID}";
        List<NPC> result = await ExecuteQueryAsync(NPCFromReader, sqlQuery);
        return result.Count > 0 ? result[0] : null;
    }
    private static NPC NPCFromReader(NpgsqlDataReader reader)
    {
        return new NPC
        {
            Npc_id = reader.GetInt32(0),
            Npc_name = reader.GetString(1),
            Personality = reader.GetString(2),
            Person_summrise = reader.GetString(3)
        };
    }
}
