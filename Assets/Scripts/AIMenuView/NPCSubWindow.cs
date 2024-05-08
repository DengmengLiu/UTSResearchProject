using UnityEditor;
using UnityEngine;

public class NPCSubWindow : EditorWindow
{
    private string npcName = "";
    private string npcPersonality = "";
    private string npcSummary = "";
    private NPC npcToUpdate;
    private Vector2 scrollPosition;

    public delegate void NPCAddedEventHandler();
    public static event NPCAddedEventHandler OnNPCModifyed;

    public static void CreateNPCDetailsWindow(NPC npc = null)
    {
        NPCSubWindow window = GetWindow<NPCSubWindow>("NPC Info");
        window.minSize = new Vector2(400, 250);
        window.maxSize = new Vector2(400, 250);
        window.npcToUpdate = npc;

        if (npc != null)
        {
            window.npcName = npc.Npc_name;
            window.npcPersonality = npc.Personality;
            window.npcSummary = npc.Person_summrise;
        }
    }

    private void OnGUI()
    {
        if (npcToUpdate != null)
        {
            Detail("Update NPC", true);
        }
        else
        {
            Detail("Add NPC", false);
        }
    }

    private void Detail(string label, bool isUpdatingWindow)
    {
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(label, GUILayout.Height(25));

        GUILayout.Space(4);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        if (isUpdatingWindow)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID:", GUILayout.Width(75));
            GUILayout.Label(npcToUpdate.Npc_id.ToString());
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
        else
        {
            GUILayout.Space(16);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", GUILayout.Width(75));
        npcName = GUILayout.TextField(npcName);
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Personality:", GUILayout.Width(75));
        npcPersonality = GUILayout.TextField(npcPersonality);
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Summary:", GUILayout.Width(75));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));
        EditorStyles.textArea.wordWrap = true;
        npcSummary = EditorGUILayout.TextArea(npcSummary, GUILayout.ExpandHeight(true), GUILayout.MinWidth(200));
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (GUILayout.Button("Submit"))
        {
            if (isUpdatingWindow)
            {
                UpdateNPC();
            }
            else
            {
                AddNPC();
            }
        }
    }

    private async void UpdateNPC()
    {
        bool success = await NPCManager.UpdateNPC(npcToUpdate.Npc_id, npcName, npcPersonality, npcSummary);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "NPC updated successfully!", "OK");
            OnNPCModifyed?.Invoke();
            Close();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to update NPC!", "OK");
        }
    }

    private async void AddNPC()
    {
        bool success = await NPCManager.AddNPC(npcName, npcPersonality, npcSummary);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "NPC added successfully!", "OK");
            OnNPCModifyed?.Invoke();
            Close();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to add NPC!", "OK");
        }
    }
}