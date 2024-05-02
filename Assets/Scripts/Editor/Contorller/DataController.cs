using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DataController : EditorWindow
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
            Debug.LogError($"Error executing query: {ex.Message}");
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
            Debug.LogError($"Error executing non-query: {ex.Message}");
            return false;
        }
    }

}
