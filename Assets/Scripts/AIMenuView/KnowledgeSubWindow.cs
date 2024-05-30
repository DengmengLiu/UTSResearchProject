using UnityEditor;
using UnityEngine;

public class KnowledgeSubWindow : EditorWindow
{
    private string knowledgeContent = "";
    private Knowledge knowledgeToUpdate;

    public delegate void KnowledgeModifiedEventHandler();
    public static event KnowledgeModifiedEventHandler OnKnowledgeModified;

    public static void CreateKnowledgeDetailsWindow(Knowledge k = null)
    {
        KnowledgeSubWindow window = GetWindow<KnowledgeSubWindow>("Knowledge Info");
        window.minSize = new Vector2(400, 250);
        window.maxSize = new Vector2(400, 250);
        window.knowledgeToUpdate = k;

        if (k != null)
        {
            window.knowledgeContent = k.Knowledge_content;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Knowledge Details", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Content:");
        knowledgeContent = EditorGUILayout.TextArea(knowledgeContent, GUILayout.Height(100));

        GUILayout.Space(10);

        if (knowledgeToUpdate != null)
        {
            if (GUILayout.Button("Update Knowledge"))
            {
                UpdateKnowledge();
            }
        }
        else
        {
            if (GUILayout.Button("Add Knowledge"))
            {
                AddKnowledge();
            }
        }
    }

    private async void AddKnowledge()
    {
        bool success = await KnowledgeManager.AddKnowledge(knowledgeContent);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "Knowledge added successfully!", "OK");
            OnKnowledgeModified?.Invoke();
            Close();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to add Knowledge!", "OK");
        }
    }

    private async void UpdateKnowledge()
    {
        bool success = await KnowledgeManager.UpdateKnowledge(knowledgeToUpdate.Knowledge_id, knowledgeContent);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "Knowledge updated successfully!", "OK");
            OnKnowledgeModified?.Invoke();
            Close();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to update Knowledge!", "OK");
        }
    }
}