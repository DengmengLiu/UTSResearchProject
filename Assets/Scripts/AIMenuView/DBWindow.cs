using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class DBWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<NPC> npcData;
    private List<Knowledge> knowledgeData;
    private int selectedIndex = -1;
    private static NPC selectedNPC;
    private static Knowledge selectedKnowledge;
    private static string windowType;

    public static void CreateWindow(string name)
    {
        DBWindow window = GetWindow<DBWindow>("Database Table");
        windowType = name;
        window.minSize = new Vector2(800, 300);
    }

    private async void LoadNPCData()
    {
        npcData = await NPCManager.GetAllNPCs();
    }

    private async void LoadKnowledgeData()
    {
        knowledgeData = await KnowledgeManager.GetAllKnowledge();
    }

    private void OnEnable()
    {
        LoadNPCData();
        LoadKnowledgeData();

        NPCSubWindow.OnNPCModifyed += LoadNPCData;
        KnowledgeSubWindow.OnKnowledgeModified += LoadKnowledgeData;
    }

    private void OnDisable()
    {
        NPCSubWindow.OnNPCModifyed -= LoadNPCData;
        KnowledgeSubWindow.OnKnowledgeModified -= LoadKnowledgeData;
    }

    void OnGUI()
    {
        if (windowType == "NPCFromAIMenu")
        {
            NPCView();
            NPCAIMenu();
        }
        else if (windowType == "NPCFromINSPECTOR")
        {
            NPCView();
            NPCINSPECTOR();
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
        GUILayout.Label("Person Summary", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
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

                bool isSelected = selectedIndex == i;
                if (GUILayout.Button(isSelected ? "¡Ì" : "", GUILayout.Width(20)))
                {
                    selectedIndex = isSelected ? -1 : i;
                }

                GUILayout.Label(npc.Npc_id.ToString(), GUILayout.Width(50));
                GUILayout.Label(npc.Npc_name, GUILayout.Width(80));
                GUILayout.Label(npc.Personality, GUILayout.Width(120));
                GUILayout.Label(npc.Person_summrise, GUILayout.Width(450));
                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }
    private void NPCAIMenu()
    {
        GUILayout.BeginHorizontal(GUILayout.Height(25));

        if (GUILayout.Button("Add new NPC", GUILayout.Height(25)))
        {
            NPCSubWindow.CreateNPCDetailsWindow();
        }

        if (GUILayout.Button("Update selected NPC", GUILayout.Height(25)))
        {
            if (selectedIndex != -1 && npcData != null && selectedIndex < npcData.Count)
            {
                selectedNPC = npcData[selectedIndex];
                NPCSubWindow.CreateNPCDetailsWindow(selectedNPC);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a NPC", "OK");
            }
        }

        if (GUILayout.Button("Delete selected NPC", GUILayout.Height(25)))
        {
            if (selectedIndex != -1 && npcData != null && selectedIndex < npcData.Count)
            {
                NPC selectedNPC = npcData[selectedIndex];
                DeleteData("NPC", selectedNPC.Npc_id);           
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No NPC selected!", "OK");
            }
        }

        //if (GUILayout.Button("Assign Knowledge to selected NPC", GUILayout.Height(25)))
        //{
        //    if (selectedIndex != -1 && npcData != null && selectedIndex < npcData.Count)
        //    {
        //        selectedNPC = npcData[selectedIndex];
        //        AssignKnowledgeToNPCWindow.CreateWindow(selectedNPC);
        //    }
        //    else
        //    {
        //        EditorUtility.DisplayDialog("Error", "Please select a NPC", "OK");
        //    }
        //}

        GUILayout.EndHorizontal();
    }
    private  void NPCINSPECTOR()
    {
        GUILayout.BeginHorizontal(GUILayout.Height(25));
        if (GUILayout.Button("Select this NPC", GUILayout.Height(25)))
        {
            if (selectedIndex != -1 && npcData != null && selectedIndex < npcData.Count)
            {
                NPC selectedNPC = npcData[selectedIndex];
                NPCAttachment.GetNPC(selectedNPC);
                Close();
            }
        }

        GUILayout.EndHorizontal();
    }

    private async void DeleteData(string tableName, int id)
    {
        string deleteQuery = $"DELETE FROM {tableName} WHERE {tableName}_id = {id}";
        bool deleted = await DatabaseController.ExecuteNonQueryAsync(deleteQuery);

        if (deleted)
        {
            EditorUtility.DisplayDialog("Success", tableName + " deleted successfully!", "OK");
            LoadNPCData();
            LoadKnowledgeData();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to delete " + tableName + "!", "OK");
        }
    }

    private void KnowledgeView()
    {
        GUILayout.Space(4);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Knowledge Details", GUILayout.Height(32));

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

        GUI.skin.label.fontSize = 12;
        GUI.skin.label.wordWrap = true;

        if (knowledgeData != null)
        {
            for (int i = 0; i < knowledgeData.Count; i++)
            {
                Knowledge knowledge = knowledgeData[i];

                GUILayout.BeginHorizontal();

                bool isSelected = selectedIndex == i;
                if (GUILayout.Button(isSelected ? "¡Ì" : "", GUILayout.Width(20)))
                {
                    selectedIndex = isSelected ? -1 : i;
                }

                GUILayout.Label(knowledge.Knowledge_id.ToString(), GUILayout.Width(50));
                GUILayout.Label(knowledge.Knowledge_content);

                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new Knowledge", GUILayout.Height(25)))
        {
            KnowledgeSubWindow.CreateKnowledgeDetailsWindow();
        }

        if (GUILayout.Button("Update selected Knowledge", GUILayout.Height(25)))
        {
            if (selectedIndex != -1 && knowledgeData != null && selectedIndex < knowledgeData.Count)
            {
                selectedKnowledge = knowledgeData[selectedIndex];
                KnowledgeSubWindow.CreateKnowledgeDetailsWindow(selectedKnowledge);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a Knowledge", "OK");
            }
        }

        if (GUILayout.Button("Delete selected Knowledge", GUILayout.Height(25)))
        {
            if (selectedIndex != -1 && knowledgeData != null && selectedIndex < knowledgeData.Count)
            {
                Knowledge selectedKnowledge = knowledgeData[selectedIndex];
                DeleteData("Knowledge", selectedKnowledge.Knowledge_id);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No Knowledge selected!", "OK");
            }
        }

        GUILayout.EndHorizontal();
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

