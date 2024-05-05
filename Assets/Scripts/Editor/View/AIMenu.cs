using UnityEditor;
using UnityEngine;

public class AIMenu : EditorWindow
{
    [MenuItem("AI Menu/Database connection setup", false, 0)]
    public static void DBConnectionSetup()
    {
        DBConnWindow.CreateWindow();
    }

    [MenuItem("AI Menu/Database View", false, 10)]
    public static void DatabaseView()
    {
        Debug.Log(DBConnWindow.IsConnected);
    }

    [MenuItem("AI Menu/Database View/NPC", false, 2)]
    public static void NPCTable()
    {
        DBWindow.CreateWindow("NPC");
    }

    [MenuItem("AI Menu/Database View/NPC", true)]
    public static bool ValidateNPCTable()
    {
        return DBConnWindow.IsConnected;
    }

    [MenuItem("AI Menu/Database View/Knowledge", false, 3)]
    public static void KnowledgeTable()
    {
        DBWindow.CreateWindow("Knowledge");
    }

    [MenuItem("AI Menu/Database View/Knowledge", true)]
    public static bool ValidateKnowledgeTable()
    {
        return DBConnWindow.IsConnected;
    }

    [MenuItem("AI Menu/Author Info", false, 100)]
    public static void AuthorInfo()
    {
        AuthorInfoWindow.CreateWindow();
    }
}
