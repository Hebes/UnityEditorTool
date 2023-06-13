using UnityEngine;
using UnityEditor;

public class ACGUIStyle4 : EditorWindow
{
    Vector2 scrollPosition = new Vector2(0, 0);
    string search = "";
    GUIStyle textStyle;

    void OnGUI()
    {
        if (textStyle == null)
        {
            textStyle = new GUIStyle("HeaderLabel");
            textStyle.fontSize = 25;
        }

        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("结果如下：", textStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Search:");
        search = EditorGUILayout.TextField(search);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
        GUILayout.Label("样式展示", textStyle, GUILayout.Width(300));
        GUILayout.Label("名字", textStyle, GUILayout.Width(300));
        GUILayout.EndHorizontal();


        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                GUILayout.Space(15);
                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                if (GUILayout.Button(style.name, style, GUILayout.Width(300)))
                {
                    EditorGUIUtility.systemCopyBuffer = style.name;
                    Debug.LogError(style.name);
                }
                EditorGUILayout.SelectableLabel(style.name, GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }
}