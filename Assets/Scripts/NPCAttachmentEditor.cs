using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(NPCAttachment))]
public class NPCAttachmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NPCAttachment npcAttachment = (NPCAttachment)target;
        GUILayout.Space(5);
        if (GUILayout.Button(NPCAttachment.ButtonText, GUILayout.Height(30))) 
        {
            npcAttachment.SelectNPC();
            
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Assign Knowledge to NPC", GUILayout.Height(30)))
        {
            npcAttachment.AssignKnowledge();
        }
        GUILayout.Space(5);
        //if (GUILayout.Button("Assign Quest to NPC", GUILayout.Height(30)))
        //{
        //    npcAttachment.AssignKnowledge();
        //}
    }
}