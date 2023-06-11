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
        private float vSbarValue;
        private Vector2 ttt;

        // The variable to control where the scrollview 'looks' into its child elements.
        Vector2 scrollPosition;

        // The string to display inside the scrollview. 2 buttons below add &amp; clear this string.
        string longString = "This is a long-ish string";

        [MenuItem("Assets/暗沉EditorTool/UI界面")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACUIInterface), false, "UI界面").Show();
        }

        private void OnGUI()
        {
            //scrollPosition = GUILayout.BeginScrollView(
            //scrollPosition, GUILayout.Width(100), GUILayout.Height(100));

            //// We just add a single label to go inside the scroll view. Note how the
            //// scrollbars will work correctly with wordwrap.
            //GUILayout.Label(longString);

            //// Add a button to clear the string. This is inside the scroll area, so it
            //// will be scrolled as well. Note how the button becomes narrower to make room
            //// for the vertical scrollbar
            //if (GUILayout.Button("Clear"))
            //    longString = "";
            // if (GUILayout.Button("Clear"))
            //    longString = "";
            // if (GUILayout.Button("Clear"))
            //    longString = "";
            // if (GUILayout.Button("Clear"))
            //    longString = "";
            // if (GUILayout.Button("Clear"))
            //    longString = "";
            // if (GUILayout.Button("Clear"))
            //    longString = "";

            //// End the scrollview we began above.
            //GUILayout.EndScrollView();

            //// Now we add a button outside the scrollview - this will be shown below
            //// the scrolling area.
            //if (GUILayout.Button("Add More Text"))
            //    longString += "\nHere is another line";

            GUILayout.BeginArea(new Rect(5, 5, position.width - 10, position.height - 10), new GUIStyle("LODBlackBox"));
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                {
                    //vSbarValue = GUILayout.VerticalScrollbar(vSbarValue, 700f, 700f, 100.0f);
                    GUILayout.Space(5);
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, "Wizard Box", GUILayout.Height(position.height - 20));
                    {
                        //gridId = GUILayout.SelectionGrid(gridId, new[] { "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "311", "2", "3", "1", "2", "312" }, 1, GUILayout.Height(position.height - 25));
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                        if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }
                         if (GUILayout.Button("测hi是", GUILayout.Height(50)))
                        {

                        }

                    }
                    EditorGUILayout.EndScrollView();

                    //EditorGUILayout.BeginVertical("Wizard Box", GUILayout.Height(position.height - 20), GUILayout.Width(200));
                    //{
                    //    gridId = GUILayout.SelectionGrid(gridId, new[] { "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "311" }, 1, GUILayout.Height(position.height-25));
                    //}
                    //EditorGUILayout.EndVertical(); 

                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal("Wizard Box", GUILayout.Width(position.width - 200), GUILayout.Height(position.height - 20));
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            m_color = EditorGUILayout.ColorField(colorTitle, m_color, true, true, true);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
