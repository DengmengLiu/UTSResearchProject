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

        // 初始化 knowledgeSelection 字典
        window.knowledgeSelection = new Dictionary<Knowledge, bool>();

        // 查询已分配给 NPC 的知识
        string assignedQuery = $"SELECT Knowledge.* FROM Knowledge INNER JOIN NPCKnowledge ON Knowledge.knowledge_id = NPCKnowledge.knowledge_id WHERE NPCKnowledge.npc_id = {npc.Npc_id}";
        List<Knowledge> assignedKnowledge = await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), assignedQuery);

        // 查询未分配给 NPC 的知识
        string unassignedQuery = $"SELECT * FROM Knowledge WHERE knowledge_id NOT IN (SELECT knowledge_id FROM NPCKnowledge WHERE npc_id = {npc.Npc_id})";
        List<Knowledge> unassignedKnowledge = await DatabaseController.ExecuteQueryAsync(reader => new Knowledge(reader.GetInt32(0), reader.GetString(1)), unassignedQuery);

        window.assignedKnowledge = assignedKnowledge;
        window.unassignedKnowledge = unassignedKnowledge;

        // 初始化 knowledgeSelection 字典
        foreach (Knowledge knowledge in assignedKnowledge)
        {
            window.knowledgeSelection.Add(knowledge, false); // 默认选中已分配的知识
        }
        foreach (Knowledge knowledge in unassignedKnowledge)
        {
            window.knowledgeSelection.Add(knowledge, false); // 默认未选中未分配的知识
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

        // 已经assign的knowledge框
        DrawAssignedKnowledgeBox();
        GUILayout.Space(5);

        // 按钮区域
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

        // 还未assign的knowledge框
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
        // 获取选中的未assign的knowledge
        List<Knowledge> selectedKnowledge = new List<Knowledge>();
        foreach (KeyValuePair<Knowledge, bool> entry in knowledgeSelection)
        {
            if (entry.Value && unassignedKnowledge.Contains(entry.Key))
            {
                selectedKnowledge.Add(entry.Key);
            }
        }

        // 将选中的knowledge从未assign列表中移除，加入已assign列表中
        foreach (Knowledge knowledge in selectedKnowledge)
        {
            unassignedKnowledge.Remove(knowledge);
            assignedKnowledge.Add(knowledge);
            knowledgeSelection[knowledge] = false; // 重置为未选中状态
        }
    }

    private void UnassignSelectedKnowledge()
    {
        // 获取选中的已assign的knowledge
        List<Knowledge> selectedKnowledge = new List<Knowledge>();
        foreach (KeyValuePair<Knowledge, bool> entry in knowledgeSelection)
        {
            if (entry.Value && assignedKnowledge.Contains(entry.Key))
            {
                selectedKnowledge.Add(entry.Key);
            }
        }

        // 将选中的knowledge从已assign列表中移除，加入未assign列表中
        foreach (Knowledge knowledge in selectedKnowledge)
        {
            assignedKnowledge.Remove(knowledge);
            unassignedKnowledge.Add(knowledge);
            knowledgeSelection[knowledge] = false; // 重置为未选中状态
        }
    }

    private async void SubmitAssignments()
    {
        // 提交已assign的knowledge到数据库
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
