using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DBWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<NPC> npcData;
    private List<Knowledge> KnowledgeData;
    private int selectedNPCIndex = -1;
    private static NPC selectedNPC;
    private static string windowType;

    public static void CreateWindow(string name)
    {
        GetWindow<DBWindow>("Database Table");
        windowType = name;
    }
    private async void LoadNPCData()
    {
        string selectQuery = "SELECT * FROM NPC";
        npcData = await DataController.ExecuteQueryAsync<NPC>(reader => new NPC(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)), selectQuery);
    }
    private async void LoadKnowledgeData()
    {
        string selectQuery = "SELECT * FROM Knowledge";
        KnowledgeData = await DataController.ExecuteQueryAsync<Knowledge>(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), selectQuery);
    }
    private void OnEnable()
    {
        LoadNPCData();
        LoadKnowledgeData();

        NPCSubWindow.OnNPCModifyed += LoadNPCData;
    }

    private void OnDisable()
    {
        NPCSubWindow.OnNPCModifyed -= LoadNPCData;
    }

    void OnGUI()
    {
        if (windowType == "NPC")
        {
            NPCView();
        }
        else if (windowType == "Knowledge")
        {
            KnowledgeView();
        }
        else 
        {
            ErrorView();
        }
        
    }
    private void NPCView()
    {
        GUILayout.Space(4);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("NPC Details", GUILayout.Height(25));

        GUILayout.Space(4);
        GUI.skin.label.fontSize = 14;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginHorizontal();
        GUILayout.Label(" ", EditorStyles.boldLabel, GUILayout.Width(20));
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(50));
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.Label("Personality", EditorStyles.boldLabel, GUILayout.Width(120));
        GUILayout.Label("Person Summry", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        GUI.skin.label.fontSize = 12;
        GUI.skin.label.wordWrap = true;

        if (npcData != null)
        {

            for (int i = 0; i < npcData.Count; i++)
            {
                NPC npc = npcData[i];

                GUILayout.BeginHorizontal();

                bool isSelected = selectedNPCIndex == i;
                if (GUILayout.Button(isSelected ? "√" : "", GUILayout.Width(20)))
                {
                    selectedNPCIndex = isSelected ? -1 : i;
                }

                GUILayout.Label(npc.npc_id.ToString(), GUILayout.Width(50));
                GUILayout.Label(npc.npc_name, GUILayout.Width(80));
                GUILayout.Label(npc.personality, GUILayout.Width(120));            
                GUILayout.Label(npc.person_summrise, GUILayout.Width(450));
                GUILayout.EndHorizontal();
            }
        
        }

        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new NPC"))
        {
            NPCSubWindow.CreateNPCDetailsWindow();
        }

        if (GUILayout.Button("Update selected NPC"))
        {
            if (selectedNPCIndex != -1 && npcData != null && selectedNPCIndex < npcData.Count)
            {
                selectedNPC = npcData[selectedNPCIndex];
                NPCSubWindow.CreateNPCDetailsWindow(selectedNPC);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a NPC", "OK");
            }
        }

        if (GUILayout.Button("Delete selected NPC"))
        {
            if (selectedNPCIndex != -1 && npcData != null && selectedNPCIndex < npcData.Count)
            {
                NPC selectedNPC = npcData[selectedNPCIndex];
                DeleteNPC(selectedNPC.npc_id);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No NPC selected!", "OK");
            }
        }
        if (GUILayout.Button("Assign Knowledge to selected NPC"))
        {
            Debug.Log("Button 4 clicked");
        }

        GUILayout.EndHorizontal();
    }

    private async void DeleteNPC(int npcId)
    {
        string deleteQuery = $"DELETE FROM NPC WHERE npc_id = {npcId}";
        bool deleted = await DataController.ExecuteNonQueryAsync(deleteQuery);

        if (deleted)
        {
            EditorUtility.DisplayDialog("Success", "NPC deleted successfully!", "OK");
            LoadNPCData(); 
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to delete NPC!", "OK");
        }
    }
    private void KnowledgeView()
    {
        GUILayout.Space(4);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Knowledge Details");

        GUILayout.Space(4);
        GUI.skin.label.fontSize = 14;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.BeginHorizontal();
        GUILayout.Label(" ", EditorStyles.boldLabel, GUILayout.Width(20));
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(50));
        GUILayout.Label("Content", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        EditorGUILayout.EndScrollView();
    }
    
    private void ErrorView()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Error loading. Please close this window", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
}
