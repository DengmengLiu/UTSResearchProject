using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KnowledgeTableWindow : EditorWindow
{
    public static void CreateWindow()
    {
        GetWindow<KnowledgeTableWindow>("Knowledge Table");
    }

    private void OnGUI()
    {
        GUILayout.Label("This is Knowledge Table Window.");
    }
}
