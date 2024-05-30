using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPCManager
{
    public static async Task<List<NPC>> GetNPC(int npc_id)
    {
        string selectQuery = $"SELECT * FROM NPC WHERE npc_id = {npc_id}";
        return await DatabaseController.ExecuteQueryAsync(reader => new NPC(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)), selectQuery);
    }
    public static async Task<List<NPC>> GetAllNPCs()
    {
        string selectQuery = "SELECT * FROM NPC";
        return await DatabaseController.ExecuteQueryAsync(reader => new NPC(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)), selectQuery);
    }

    public static async Task<bool> AddNPC(string npcName, string personality, string summary)
    {
        string insertQuery = $"INSERT INTO NPC (npc_name, personality, person_summrise) VALUES ('{npcName}', '{personality}', '{summary}')";
        return await DatabaseController.ExecuteNonQueryAsync(insertQuery);
    }

    public static async Task<bool> UpdateNPC(int id, string npcName, string personality, string summary)
    {
        string updateQuery = $"UPDATE NPC SET npc_name = '{npcName}', personality = '{personality}', person_summrise = '{summary}' WHERE npc_id = {id}";
        return await DatabaseController.ExecuteNonQueryAsync(updateQuery);
    }
    public static async Task<bool> AssignKnowledgeToNPC(int npc_id, List<Knowledge> knowledge)
    {
        try
        {
            using (var conn = new NpgsqlConnection(DBConnection.GetConnectionString()))
            {
                await conn.OpenAsync();

                // 首先删除NPC原有的知识
                string deleteQuery = $"DELETE FROM NPCKnowledge WHERE npc_id = {npc_id}";
                using (var deleteCmd = new NpgsqlCommand(deleteQuery, conn))
                {
                    await deleteCmd.ExecuteNonQueryAsync();
                }

                // 然后插入新的知识
                foreach (var k in knowledge)
                {
                    string insertQuery = $"INSERT INTO NPCKnowledge (npc_id, knowledge_id) VALUES ({npc_id}, {k.Knowledge_id})";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
        }
        catch (NpgsqlException ex)
        {
            Debug.LogError($"Error assigning knowledge to NPC: {ex.Message}");
            return false;
        }
    }

}
