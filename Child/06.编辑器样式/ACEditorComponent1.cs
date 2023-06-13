using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public class ACEditorComponent1 : EditorWindow
{

	static List<GUIStyle> styles = null;
	
    private void Awake()
    {
        styles = new List<GUIStyle>();
        foreach (PropertyInfo fi in typeof(EditorStyles).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            object o = fi.GetValue(null, null);
            if (o.GetType() == typeof(GUIStyle))
            {
                styles.Add(o as GUIStyle);
            }
        }
    }

    public Vector2 scrollPosition = Vector2.zero;
	void OnGUI()
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		for (int i = 0; i < styles.Count; i++)
		{
			GUILayout.Label("EditorStyles." + styles[i].name, styles[i]);
		}
		GUILayout.EndScrollView();
	}
}