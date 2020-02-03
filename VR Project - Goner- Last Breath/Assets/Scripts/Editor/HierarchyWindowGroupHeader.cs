using UnityEngine;
using UnityEditor;

/// <summary>
/// Headers creator in the hierarchy window.
/// </summary>
[InitializeOnLoad]
public static class HierarchyWindowGroupHeader
{
    /*
      If the object isn't null, starts with "---" and using an ordinal (binary) sort rules:
      Draws a rectangle around it with a gray color and drops a shadow label with the new name.
    */

    private static Color _lightGray = new Color(0.625f, 0.625f, 0.625f, 1);
    private static string _inactive = " (INACTIVE)";

    static HierarchyWindowGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("---", System.StringComparison.Ordinal))
        {
            if (!gameObject.activeInHierarchy)
            {
                EditorGUI.DrawRect(selectionRect, _lightGray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "") + _inactive.ToString());
            }
            else
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "").ToString());
            }
        }
    }
}