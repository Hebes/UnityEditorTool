using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACUIInterface : EditorWindow
    {
        private List<string> btnNameList { get; set; } = new List<string>()
        {
            "Hierarchy替换物体",
            "Hierarchy获取代码",
            "Hierarchy组件设置",
            "移除脚本",
            "Hierarchy重命名",
            //"Hierarchy后缀重命名",
            "显示",
        };
        private string number { get; set; }
        private string PlayerPrefsKey { get; set; } = "key";
        private Vector2 scrollPosition { get; set; }


        [MenuItem("Assets/暗沉工具面板Alt+Q &Q")]//#E
        public static void OnShow()
        {
            //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.GetWindow.html
            //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
            EditorWindow tempWindow = GetWindow(typeof(ACUIInterface), false, "UI界面");
            if (EditorWindow.HasOpenInstances<ACUIInterface>())
                tempWindow.Show();
        }

        [MenuItem("Assets/暗沉工具面板Alt+Q &Q")]
        public static void OnHide()
        {
            EditorWindow tempWindow = GetWindow(typeof(ACUIInterface), false, "UI界面");
            if (EditorWindow.HasOpenInstances<ACUIInterface>())
                tempWindow.Close();
        }

        //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
        private void Awake()
        {
            number = PlayerPrefs.GetString(PlayerPrefsKey);
        }

        //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
        /// <summary>
        /// 刷新界面
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }


        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(5, 5, position.width - 10, position.height - 10), new GUIStyle("LODBlackBox"));
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(5);
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, "Wizard Box", GUILayout.Height(position.height - 20), GUILayout.Width(150));
                    {
                        GUILayout.Space(5);
                        for (int i = 0; i < btnNameList.Count; i++)
                        {
                            if (GUILayout.Button(btnNameList[i], GUILayout.Height(40)))
                            {
                                number = btnNameList[i];
                                PlayerPrefs.SetString(PlayerPrefsKey, btnNameList[i]);
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();

                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal("Wizard Box", GUILayout.Width(position.width - 175), GUILayout.Height(position.height - 20));
                    {
                        GUILayout.Space(5);
                        EditorGUILayout.BeginVertical();
                        {
                            switch (number)
                            {
                                case "Hierarchy替换物体":
                                    ACHierarchyPrefabChange.OnShow();
                                    break;
                                case "Hierarchy获取代码":
                                    ACHierarchyGetCode.OnShow();
                                    break;
                                case "Hierarchy组件设置":
                                    ACHierarchyToolComponent.OnShow();
                                    break;
                                case "移除脚本":
                                    ACHierarchyToolCSharpCSharp.OnShow();
                                    break;
                                case "Hierarchy重命名":
                                    ACHierarchyToolReNameReName.OnShow();
                                    break;

                            }
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(5);
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
