using UnityEditor;
using UnityEngine;

public class DBConnWindow : EditorWindow
{
    private string[] labels = { "Database Host", "Username", "Password", "Database Name" };
    private string[] textFields = new string[4];
    private static bool isConnected = false;
    public static void CreateWindow()
    {
        DBConnWindow window = GetWindow<DBConnWindow>("Connection setup");
        window.minSize = new Vector2(375, 200);
    }

    private async void OnGUI()
    {
        GUILayout.Label("Custom Editor Window", EditorStyles.boldLabel);
        GUILayout.Space(10);
        for (int i = 0; i < labels.Length; i++)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(labels[i], GUILayout.Width(150));
            if (i == 2)
                textFields[i] = EditorGUILayout.PasswordField("", textFields[i]);
            else
                textFields[i] = GUILayout.TextField(textFields[i]);

            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Submit"))
        {
            DBConnection.BuildConnectionStringFromArray(textFields);
            await DBConnection.ConnectToDatabase(DBConnection.GetConnectionString());
            isConnected = true;
            Close();
        }
    }
    public static bool IsConnected
    {
        get { return isConnected; }
    }
}