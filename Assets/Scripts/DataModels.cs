[System.Serializable]
public class NPC
{
    public int Npc_id { get; set; } // NPC ID
    public string Npc_name { get; set; } // NPC Name
    public string Personality { get; set; } // Personality
    public string Person_summrise { get; set; } // Summary

    public NPC()
    {

    }
    public NPC(int id, string name, string personality, string person_sum)
    {
        Npc_id = id;
        Npc_name = name;
        Personality = personality;
        Person_summrise = person_sum;
    }
    public NPC(string name, string personality, string person_sum)
    {
        Npc_name = name;
        Personality = personality;
        Person_summrise = person_sum;
    }
}

public class Knowledge
{
    public int Knowledge_id { get; set; } // Knowledge ID
    public string Knowledge_content { get; set; } // Knowledge Content
    public double Knowledge_CosineDis { get; set; } // Knowledge Vector

    public Knowledge(int id, string content)
    {
        Knowledge_id = id;
        Knowledge_content = content;
    }
    public Knowledge(double dis, string content)
    {
        Knowledge_CosineDis = dis;
        Knowledge_content = content;
    }
}

public class NPCKnowledge
{
    public int Knowledge_Id { get; set; } // Knowledge ID
    public int NPC_Id { get; set; } // NPC ID

    public NPCKnowledge(int knowledgeId, int npcId)
    {
        Knowledge_Id = knowledgeId;
        NPC_Id = npcId;
    }
}

public class Quest
{
    public int Quest_id { get; set; }
    public string Quest_goal { get; set; }
    public int Quest_urgency { get; set; }
    public string Quest_description {  get; set; }
    public double Quest_description_CosineDis { get; set; }

    public Quest(int id, string content) { }
}

