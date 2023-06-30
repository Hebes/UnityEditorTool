using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACUIInterface : EditorWindow
    {
        private string number { get; set; }
        private string PlayerPrefsKey { get; set; } = "key";
        private Vector2 scrollPosition { get; set; }
        private string info { get; set; } = string.Empty;


        [MenuItem("Assets/暗沉工具面板Alt+Q &Q")]//#E
        public static void OnShow()
        {
            //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.GetWindow.html
            //https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
            if (!EditorWindow.HasOpenInstances<ACUIInterface>())
                GetWindow(typeof(ACUIInterface), false, "UI界面").Show();
            else
                GetWindow(typeof(ACUIInterface)).Close();
        }

        /// <summary>
        /// 启动时调用 https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
        /// </summary>
        private void Awake()
        {
            number = PlayerPrefs.GetString(PlayerPrefsKey);
            if (number == null)
                number = btnNameList[0];
        }

        /// <summary>
        /// 刷新界面 https://docs.unity3d.com/cn/2020.3/ScriptReference/EditorWindow.html
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
                        GUILayout.Space(5);
                    }
                    EditorGUILayout.EndScrollView();

                    GUILayout.Space(5);
                    LetfV2ScrollView = EditorGUILayout.BeginScrollView(LetfV2ScrollView, GUILayout.Height(position.height - 20));
                    {
                        EditorGUILayout.BeginHorizontal("Wizard Box");
                        {
                            GUILayout.Space(5);
                            EditorGUILayout.BeginVertical();
                            {
                                switch (number)
                                {
                                    case "Hierarchy替换物体":
                                        ACHierarchyPrefabChange.OnShow();
                                        break;
                                    case "显示文件大小":
                                        ACFileAndFoledSize.OnShow();
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
                                    case "场景获取工具":
                                        ACProjectToolOther.OnShow();
                                        break;
                                    case "Project重命名":
                                        ACProjectToolReName.OnShow();
                                        break;
                                    case "创建目录":
                                        ACCreatBaseDirectory.OnShow();
                                        break;
                                    case "ET工具集合":
#if ACTool
                                        ETUITool.OnShow();
#endif
                                        break;
                                    case "编辑器样式":
                                        EditorGUILayout.BeginHorizontal();
                                        if (GUILayout.Button("样式1"))
                                            CreateInstance<ACGUIStyle1>().Show();
                                        if (GUILayout.Button("关闭"))
                                            GetWindow<ACGUIStyle1>().Close();
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        if (GUILayout.Button("样式5"))
                                            CreateInstance<ACGUIStyle5>().Show();
                                        if (GUILayout.Button("关闭"))
                                            GetWindow<ACGUIStyle5>().Close();
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        if (GUILayout.Button("样式4"))
                                            CreateInstance<ACGUIStyle4>().Show();
                                        if (GUILayout.Button("关闭"))
                                            GetWindow<ACGUIStyle4>().Close();
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        if (GUILayout.Button("组件1"))
                                            CreateInstance<ACEditorComponent1>().Show();
                                        if (GUILayout.Button("关闭"))
                                            GetWindow<ACEditorComponent1>().Close();
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        if (GUILayout.Button("组件2"))
                                            CreateInstance<ACEditorComponent2>().Show();
                                        if (GUILayout.Button("关闭"))
                                            GetWindow<ACEditorComponent2>().Close();
                                        EditorGUILayout.EndHorizontal();
                                        break;
                                    case "模型预览":
                                        ACPrefabPreview.OnShow();
                                        break;
                                }
                            }
                            EditorGUILayout.EndVertical();
                            GUILayout.Space(5);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                    GUILayout.Space(5);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
        private List<string> btnNameList { get; set; } = new List<string>()
        {
            "Hierarchy替换物体",
            "Hierarchy获取代码",
            "Hierarchy组件设置",
            "移除脚本",
            "Hierarchy重命名",
            "场景获取工具",
            "Project重命名",
            "创建目录",
            "ET工具集合",
            "编辑器样式",
            "模型预览",
            "显示文件大小",
        };
        public Vector2 LetfV2ScrollView { get; private set; }

        private static void OnShowOInfo()
        {
            GUILayout.BeginHorizontal();
        }
    }
}
