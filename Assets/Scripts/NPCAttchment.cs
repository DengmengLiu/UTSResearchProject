using UnityEditor;
using UnityEngine;

public class NPCAttachment : MonoBehaviour
{
    [SerializeField]
    public string quest;
    public static string ButtonText = "Select a NPC";

    public static NPC selectedNPC;

    private async void OnEnable()
    {
        int npcID = EditorPrefs.GetInt("SelectedNPC_ID", -1);
        selectedNPC = await DatabaseController.GetNPCByIDAsync(npcID);
        if (npcID != -1)
        {
            if (selectedNPC != null)
            {
                ButtonText = selectedNPC.Npc_name;
            }
        }
    }
    public void SelectNPC()
    {
        DBWindow.CreateWindow("NPCFromINSPECTOR");
    }
    public void AssignKnowledge()
    {
        if (selectedNPC != null) 
        {
            AssignKnowledgeToNPCWindow.CreateWindow(selectedNPC);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Please select a NPC", "OK");
        }       
    }
    public static void GetNPC(NPC npc)
    {
        selectedNPC = npc;
        ButtonText = npc.Npc_name;

        EditorPrefs.SetInt("SelectedNPC_ID", npc.Npc_id);        
    }
 
}


