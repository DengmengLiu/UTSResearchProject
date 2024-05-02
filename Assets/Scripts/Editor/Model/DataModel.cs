using System.Threading.Tasks;

public class NPC
{
    public int npc_id { get; } // NPC ID
    public string npc_name { get; set; } // NPC Name
    public string personality { get; set; } // Personality
    public string person_summrise { get; set; } // Summary

    public NPC()
    {

    }
    public NPC(int id, string name, string personality, string person_sum)
    {
        npc_id = id;
        npc_name = name;
        this.personality = personality;
        person_summrise = person_sum;
    }
    public NPC(string name, string personality, string person_sum)
    {
        npc_name = name;
        this.personality = personality;
        person_summrise = person_sum;
    }
}

public class Knowledge
{
    public int knowledge_id { get; set; } // Knowledge ID
    public string knowledge_content { get; set; } // Knowledge Content
    public float[] knowledge_vector { get; } // Knowledge Vector

    public Knowledge(int id, string content)
    {
        knowledge_id = id;
        knowledge_content = content;
        knowledge_vector = new float[1536];
    }
}

public class NPC_Knowledge
{
    public int konwledge_id { get; set; } // Knowledge ID
    public int npc_id { get; set; } // NPC ID

    public NPC_Knowledge(int knowledgeId, int npcId)
    {
        konwledge_id = knowledgeId;
        npc_id = npcId;
    }
}