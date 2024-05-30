using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class KnowledgeManager
{
    public static async Task<List<Knowledge>> GetAllKnowledge()
    {
        string selectQuery = "SELECT * FROM Knowledge";
        return await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), selectQuery);
    }

    public static async Task<bool> AddKnowledge(string content)
    {
        var vector = JsonConvert.SerializeObject(await AIManager.GetEmbeddings(content));
        string insertQuery = $"INSERT INTO Knowledge (knowledge_content, knowledge_vector) VALUES ('{content}', '{vector}')";
        return await DatabaseController.ExecuteNonQueryAsync(insertQuery);
    }

    public static async Task<bool> UpdateKnowledge(int id, string content)
    {
        var vector = JsonConvert.SerializeObject(await AIManager.GetEmbeddings(content));
        string updateQuery = $"UPDATE Knowledge SET knowledge_content = '{content}', knowledge_vector = '{vector}' WHERE knowledge_id = {id}";
        return await DatabaseController.ExecuteNonQueryAsync(updateQuery);
    }
    public static async Task<List<Knowledge>> GetAssignedKnowledge(int npcid)
    {
        string selectQuery = $"SELECT Knowledge. * FROM Knowledge INNER JOIN NPCKnowledge ON Knowledge.knowledge_id = NPCKnowledge.knowledge_id WHERE NPCKnowledge.npc_id = {npcid}";
        return await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), selectQuery);
    }
    public static async Task<List<Knowledge>> GetUnassignedKnowledge(int npcid)
    {
        string selectQuery = $"SELECT * FROM Knowledge WHERE knowledge_id NOT IN (SELECT knowledge_id FROM NPCKnowledge WHERE npc_id = {npcid})";
        return await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), selectQuery);
    }
}
