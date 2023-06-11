using System.Drawing;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACUIInterface : EditorWindow
    {
        private Vector2 scrollRoot;
        private int gridId;
        private UnityEngine.Color m_color;
        private GUIContent colorTitle = new GUIContent("颜色选择");

        [MenuItem("Assets/暗沉EditorTool/UI界面")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACUIInterface), false, "UI界面").Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, position.width - 20, position.height - 20));
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(200));
                {
                    EditorGUILayout.BeginVertical(new GUIStyle("LODBlackBox"), GUILayout.Width(200));
                    {
                        gridId = GUILayout.SelectionGrid(gridId, new[] { "1", "2", "3" }, 1, "ButtonMid", GUILayout.Width(180),GUILayout.Height(100));
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(300, 20, position.width - 20, position.height - 20));
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(200));
                {
                    EditorGUILayout.BeginVertical(new GUIStyle("LODBlackBox"));
                    {
                        m_color = EditorGUILayout.ColorField(colorTitle, m_color, true, true, true);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

            }
            GUILayout.EndArea();
        }
    }
}
