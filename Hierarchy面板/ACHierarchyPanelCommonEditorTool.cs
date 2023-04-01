using ACTtool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ACTool
{
    /// <summary>
    /// Hierarchy面板通用功能
    /// </summary>
    public class ACHierarchyPanelCommonEditorTool : EditorWindow
    {
        public string InputCustom { get; private set; }//输入自定义
        private Vector2 scrollRoot { get; set; }

        [MenuItem("Assets/暗沉EditorTool/Hierarchy面板/Hierarchy面板通用功能")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACHierarchyPanelCommonEditorTool), false, "Hierarchy面板通用功能").Show();
        }

        private void OnGUI()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    //******************************移除丢失脚本******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("移除丢失脚本:", EditorStyles.largeLabel);
                    if (GUILayout.Button("移除丢失脚本", EditorStyles.miniButtonMid)) { Selection.objects.ACRemoveMissScriptAll(); }
                    InputCustom = EditorGUILayout.TextField("自定义脚本的名称", InputCustom);
                    if (GUILayout.Button("移除自定义脚本", EditorStyles.miniButtonMid))
                    {
                        GameObject obj = Selection.objects.First() as GameObject;
                        List<GameObject> gameObjects = new List<GameObject>();
                        //获取所有的组件
                        obj.transform.ACLoopGetAllGameObject(ref gameObjects);
                        //移除自定义脚本
                        for (int i = 0; i < gameObjects?.Count; i++)
                            gameObjects[i].ACRemoveScript(InputCustom);
                    }

                    //******************************一键替换场景中的物体******************************
                    EditorGUILayout.LabelField("====================================================");
                    GUILayout.Space(5f); EditorGUILayout.LabelField("选择一个新的物体");
                    newPrefab = (GameObject)EditorGUILayout.ObjectField(newPrefab, typeof(GameObject), true, GUILayout.MinWidth(100f));
                    tonewPrefab = newPrefab;
                    if (GUILayout.Button("替换选中的物体"))
                    {
                        ReplaceObjects();
                    }
                    replaceName = EditorGUILayout.TextField("需要替换的物体名字", replaceName);
                    if (GUILayout.Button("替换相同名字的"))
                    {
                        ReplaceObjectsByName(replaceName, false);
                    }
                    if (GUILayout.Button("替换包含名字的 慎用"))
                    {
                        ReplaceObjectsByName(replaceName, true);
                    }
                    if (GUILayout.Button("保存修改"))
                    {
                        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// HierarchyPanel前缀
        /// </summary>
        public static void ACHierarchyPanelPrefix()
        {
            ACEditorConfig.HierarchyPanel_scrollRoot = EditorGUILayout.BeginScrollView(ACEditorConfig.HierarchyPanel_scrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("HierarchyPanel通用工具", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    ACEditorConfig.HierarchyPanel_Inputprefix = EditorGUILayout.TextField("请输入组件查找前缀", ACEditorConfig.HierarchyPanel_Inputprefix);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("获取名称", EditorStyles.miniButtonMid))
                        {
                            GameObject obj = Selection.objects.First() as GameObject;
                            ACEditorConfig.HierarchyPanel_Inputprefix = $"{obj.name}_";
                        }
                        if (GUILayout.Button("清空前缀", EditorStyles.miniButtonMid))
                        {
                            ACEditorConfig.HierarchyPanel_Inputprefix = string.Empty;
                        }
                        if (GUILayout.Button("常用前缀:T_", EditorStyles.miniButtonMid))
                        {
                            ACEditorConfig.HierarchyPanel_Inputprefix = "T_";
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //GUILayout.FlexibleSpace();// 自动填充间隔
                    //******************************获取组件专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField($"前缀添加{ACEditorConfig.HierarchyPanel_Inputprefix}:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button($"前缀添加{ACEditorConfig.HierarchyPanel_Inputprefix}", EditorStyles.miniButtonMid)) { Selection.objects.ACAddPrefixLoop(ACEditorConfig.HierarchyPanel_Inputprefix); }
                        if (GUILayout.Button($"去除前缀{ACEditorConfig.HierarchyPanel_Inputprefix}", EditorStyles.miniButtonMid)) { Selection.objects.ACRemovePrefix(ACEditorConfig.HierarchyPanel_Inputprefix); }
                        if (GUILayout.Button($"保存修改", EditorStyles.miniButtonMid)) { Selection.objects.ACSaveModification(); }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndVertical(); GUILayout.Space(5f);
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }


        private static Vector2 ACHierarchyPanelCode_scrollRoot { get; set; }
        private static string ACHierarchyPanelCode_KeyValue { get; set; } = "T_";
        private static string ACHierarchyPanelCode_ClassName = String.Empty;

        /// <summary>
        /// HierarchyPanel代码获取组件
        /// </summary>
        public static void ACHierarchyPanelCode()
        {
            GUI.skin.button.wordWrap = true;
            ACHierarchyPanelCode_scrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyPanelCode_scrollRoot);
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("HierarchyPanel代码获取组件", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    ACHierarchyPanelCode_ClassName = EditorGUILayout.TextField("请输入需要查找的组件", ACHierarchyPanelCode_ClassName);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("Text组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "Text"; }
                        if (GUILayout.Button("InputField组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "InputField"; }
                        if (GUILayout.Button("Button组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "Button"; }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}组件", EditorStyles.miniButtonMid))
                    {
                        //查找自定义的需要的组件
                        List<GameObject> gameObjects = new List<GameObject>();
                        List<GameObject> obj = ACToolExpansionFind.ACGetSelection().ACGetSelectionGos();
                        for (int i = 0; i < obj?.Count; i++)
                            obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);
                        //拼接
                        Type type = ACHierarchyPanelCode_ClassName.ACReflectClass("UnityEngine.UI");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < gameObjects?.Count; i++)
                        {
                            GameObject gameObject = gameObjects[i];
                            if (gameObject.transform.GetComponent(type) != null && gameObject.name.StartsWith(ACHierarchyPanelCode_KeyValue))
                            {
                                switch (type.Name)
                                {
                                    case "Text":
                                        sb.AppendLine($"self.{gameObject.name}.GetComponent<Text>().text = String.Empty;");
                                        break;
                                    case "InputField":
                                        sb.AppendLine($"self.{gameObject.name}.GetComponent<InputField>().text = String.Empty;");
                                        break;
                                    case "Button":
                                        sb.AppendLine("//");
                                        sb.AppendLine($"self.{gameObject.name}.GetComponent<Button>().onClick.AddListener(self.On{gameObject.name});");
                                        break;
                                }
                            }
                        }
                        sb.ToString().UnityCopyWord();
                        Debug.Log(sb.ToString());
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        #region Unity 一键替换场景中的物体
        /// <summary>
        /// 新的物体
        /// </summary>
        public GameObject newPrefab;
        /// <summary>
        /// 预制体
        /// </summary>
        public static GameObject tonewPrefab;
        private string replaceName = String.Empty;

        /// <summary>
        /// 替换物体
        /// </summary>
        public static void ReplaceObjects()
        {
            if (tonewPrefab == null) return;
            UnityEngine.Object[] objects = Selection.objects;
            List<ReplaceData> replaceDatas = new List<ReplaceData>();
            foreach (UnityEngine.Object item in objects)
            {
                GameObject temp = (GameObject)item;
                ReplaceData replaceData = new ReplaceData();
                replaceData.old = temp;
                ReplaceOne(replaceData);
                replaceDatas.Add(replaceData);
            }
            HandleReplaceData(replaceDatas);
        }

        /// <summary>
        /// 根据名称替换
        /// </summary>
        /// <param name="name">物体名称</param>
        /// <param name="isContain">是否包含</param>
        void ReplaceObjectsByName(string name, bool isContain)
        {
            if (string.IsNullOrEmpty(name)) return;//名称不为空
            List<ReplaceData> replaceDatas = new List<ReplaceData>();//新的列表

            //选中的物体
            UnityEngine.Object[] objects = Selection.objects;
            List<Transform> all = new List<Transform>();

            foreach (var item in objects)
            {
                GameObject go = (GameObject)item;
                Transform transform = go.GetComponent<Transform>();
                transform.LoopGetAllTransform(ref all);
            }

            foreach (var item in all)
            {
                //Debug.LogError(item.name);
                ReplaceData replaceData = new ReplaceData();
                replaceData.old = item.gameObject;
                if (!isContain && item.gameObject.name == name)
                {
                    ReplaceOne(replaceData);
                    replaceDatas.Add(replaceData);
                }
                else if (isContain && item.gameObject.name.Contains(name))
                {
                    ReplaceOne(replaceData);
                    replaceDatas.Add(replaceData);
                }
            }
            HandleReplaceData(replaceDatas);
        }

        /// <summary>
        /// 替换一个
        /// </summary>
        /// <param name="replaceData"></param>
        public static void ReplaceOne(ReplaceData replaceData)
        {
            GameObject replace = (GameObject)PrefabUtility.InstantiatePrefab(tonewPrefab);
            replace.transform.SetParent(replaceData.old.transform.parent);
            replace.transform.localPosition = replaceData.old.transform.localPosition;
            replace.transform.localRotation = replaceData.old.transform.localRotation;
            replace.transform.localScale = replaceData.old.transform.localScale;
            replaceData.replace = replace;
            replaceData.index = replaceData.old.transform.GetSiblingIndex();
        }

        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="replaceDatas"></param>
        public static void HandleReplaceData(List<ReplaceData> replaceDatas)
        {
            foreach (var replaceData in replaceDatas)
            {
                replaceData.replace.transform.SetSiblingIndex(replaceData.index);
                if (null != replaceData.old && null != replaceData.old.gameObject)
                    DestroyImmediate(replaceData.old.gameObject);
            }
        }

        public class ReplaceData
        {
            public GameObject old;
            public GameObject replace;
            public int index = 0;
        }
        #endregion
    }
}
