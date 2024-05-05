using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Embedding;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AIManager
{
    private static readonly OpenAIAPI api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
 
    public static async Task<float[]> GetEmbeddings(string content)
    {
        var result = await api.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest(Model.TextEmbedding3Small, content));

        return result.Data[0].Embedding;
    }
    public static async Task<List<Knowledge>> GetCosineDistance(string content)
    {
        var vector = JsonConvert.SerializeObject(await AIManager.GetEmbeddings(content));
        string selectQuery = $"SELECT knowledge_content, 1 - (knowledge_vector <=> '{vector}') AS cosine_similarity FROM Knowledge ORDER BY cosine_similarity desc limit 5";
        return await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetDouble(0), reader.GetString(2)), selectQuery);
    }
}
