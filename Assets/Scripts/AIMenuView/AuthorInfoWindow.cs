using UnityEditor;
using UnityEngine;

public class AuthorInfoWindow : EditorWindow
{
    private string websiteURL = "https://www.linkedin.com/in/dengmeng-liu/";

    public static void CreateWindow()
    {
        AuthorInfoWindow window = GetWindow<AuthorInfoWindow>("Author Info");
        window.minSize = new Vector2(320, 100);
        window.maxSize = new Vector2(320, 100);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Author Name: Dengmeng Liu", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Linkedin:", EditorStyles.boldLabel);
        GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
        linkStyle.normal.textColor = Color.blue;
        linkStyle.hover.textColor = Color.cyan;
        linkStyle.wordWrap = true;
        if (GUILayout.Button(websiteURL, linkStyle))
        {
            Application.OpenURL(websiteURL);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
}
