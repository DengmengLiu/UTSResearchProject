using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssignKnowledgeToNPCWindow : EditorWindow
{
    private List<Knowledge> assignedKnowledge;
    private List<Knowledge> unassignedKnowledge;
    private NPC selectedNPC;
    private Vector2 scrollPositionAssigned;
    private Vector2 scrollPositionUnassigned;
    private Dictionary<Knowledge, bool> knowledgeSelection;

    public static async void CreateWindow(NPC npc)
    {
        AssignKnowledgeToNPCWindow window = GetWindow<AssignKnowledgeToNPCWindow>("Assign Knowledge");
        window.minSize = new Vector2(800, 450);
        window.maxSize = new Vector2(800, 450);

        window.selectedNPC = npc;

        // ³õÊ¼»¯ knowledgeSelection ×Öµä
        window.knowledgeSelection = new Dictionary<Knowledge, bool>();

        // ²éÑ¯ÒÑ·ÖÅä¸ø NPC µÄÖªÊ¶
        List<Knowledge> assignedKnowledge = await KnowledgeManager.GetAssignedKnowledge(npc.Npc_id);

        // ²éÑ¯Î´·ÖÅä¸ø NPC µÄÖªÊ¶
        List <Knowledge> unassignedKnowledge = await KnowledgeManager.GetUnassignedKnowledge(npc.Npc_id);

        window.assignedKnowledge = assignedKnowledge;
        window.unassignedKnowledge = unassignedKnowledge;

        // ³õÊ¼»¯ knowledgeSelection ×Öµä
        foreach (Knowledge knowledge in assignedKnowledge)
        {
            window.knowledgeSelection.Add(knowledge, false); // Ä¬ÈÏÑ¡ÖÐÒÑ·ÖÅäµÄÖªÊ¶
        }
        foreach (Knowledge knowledge in unassignedKnowledge)
        {
            window.knowledgeSelection.Add(knowledge, false); // Ä¬ÈÏÎ´Ñ¡ÖÐÎ´·ÖÅäµÄÖªÊ¶
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Assign Knowledge to " + selectedNPC.Npc_name, GUILayout.Height(25));

        GUILayout.Space(5);

        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        // ÒÑ¾­assignµÄknowledge¿ò
        DrawAssignedKnowledgeBox();
        GUILayout.Space(5);

        // °´Å¥ÇøÓò 
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("↑", GUILayout.Width(150)))
        {
            AssignSelectedKnowledge();
        }
        if (GUILayout.Button("↓", GUILayout.Width(150)))
        {
            UnassignSelectedKnowledge();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        // »¹Î´assignµÄknowledge¿ò
        DrawUnassignedKnowledgeBox();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Submit", GUILayout.Width(200), GUILayout.Height(30)))
        {
            SubmitAssignments();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawAssignedKnowledgeBox()
    {
        GUILayout.Label("Assigned Knowledge", EditorStyles.boldLabel);

        GUILayout.Space(5);
        scrollPositionAssigned = EditorGUILayout.BeginScrollView(scrollPositionAssigned, GUILayout.Height(150));

        if (assignedKnowledge != null && assignedKnowledge.Count > 0)
        {
            foreach (Knowledge knowledge in assignedKnowledge)
            {
                GUILayout.BeginHorizontal();
                bool selected = knowledgeSelection.ContainsKey(knowledge) && knowledgeSelection[knowledge];
                knowledgeSelection[knowledge] = EditorGUILayout.Toggle(selected, GUILayout.Width(20));
                GUILayout.Label(knowledge.Knowledge_content);
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("No assigned knowledge", EditorStyles.miniLabel);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawUnassignedKnowledgeBox()
    {
        GUILayout.Label("Unassigned Knowledge", EditorStyles.boldLabel);

        GUILayout.Space(5);
        scrollPositionUnassigned = EditorGUILayout.BeginScrollView(scrollPositionUnassigned, GUILayout.Height(150));

        if (unassignedKnowledge != null && unassignedKnowledge.Count > 0)
        {
            foreach (Knowledge knowledge in unassignedKnowledge)
            {
                GUILayout.BeginHorizontal();
                bool selected = knowledgeSelection.ContainsKey(knowledge) && knowledgeSelection[knowledge];
                knowledgeSelection[knowledge] = EditorGUILayout.Toggle(selected, GUILayout.Width(20));
                GUILayout.Label(knowledge.Knowledge_content);
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("No unassigned knowledge", EditorStyles.miniLabel);
        }

        EditorGUILayout.EndScrollView();
    }

    private void AssignSelectedKnowledge()
    {
        // »ñÈ¡Ñ¡ÖÐµÄÎ´assignµÄknowledge
        List<Knowledge> selectedKnowledge = new List<Knowledge>();
        foreach (KeyValuePair<Knowledge, bool> entry in knowledgeSelection)
        {
            if (entry.Value && unassignedKnowledge.Contains(entry.Key))
            {
                selectedKnowledge.Add(entry.Key);
            }
        }

        // ½«Ñ¡ÖÐµÄknowledge´ÓÎ´assignÁÐ±íÖÐÒÆ³ý£¬¼ÓÈëÒÑassignÁÐ±íÖÐ
        foreach (Knowledge knowledge in selectedKnowledge)
        {
            unassignedKnowledge.Remove(knowledge);
            assignedKnowledge.Add(knowledge);
            knowledgeSelection[knowledge] = false; // ÖØÖÃÎªÎ´Ñ¡ÖÐ×´Ì¬
        }
    }

    private void UnassignSelectedKnowledge()
    {
        // »ñÈ¡Ñ¡ÖÐµÄÒÑassignµÄknowledge
        List<Knowledge> selectedKnowledge = new List<Knowledge>();
        foreach (KeyValuePair<Knowledge, bool> entry in knowledgeSelection)
        {
            if (entry.Value && assignedKnowledge.Contains(entry.Key))
            {
                selectedKnowledge.Add(entry.Key);
            }
        }

        // ½«Ñ¡ÖÐµÄknowledge´ÓÒÑassignÁÐ±íÖÐÒÆ³ý£¬¼ÓÈëÎ´assignÁÐ±íÖÐ
        foreach (Knowledge knowledge in selectedKnowledge)
        {
            assignedKnowledge.Remove(knowledge);
            unassignedKnowledge.Add(knowledge);
            knowledgeSelection[knowledge] = false; // ÖØÖÃÎªÎ´Ñ¡ÖÐ×´Ì¬
        }
    }

    private async void SubmitAssignments()
    {
        // Ìá½»ÒÑassignµÄknowledgeµ½Êý¾Ý¿â
        bool success = await NPCManager.AssignKnowledgeToNPC(selectedNPC.Npc_id, assignedKnowledge);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "Knowledge assigned successfully!", "OK");
            Close();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to assign knowledge!", "OK");
        }
    }
}
