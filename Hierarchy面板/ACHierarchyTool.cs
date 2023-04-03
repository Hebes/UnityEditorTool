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
    public class ACHierarchyTool : EditorWindow
    {
        [MenuItem("Assets/暗沉EditorTool/Hierarchy面板/Hierarchy面板通用功能")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACHierarchyTool), false, "Hierarchy面板通用功能").Show();
        }

        /// <summary>
        /// 绘制面板
        /// </summary>
        private void OnGUI()
        {
            ACHierarchyPrefix();
            ACHierarchyRemoveScript();
            ACHierarchyPrefabChange();
        }

        //***************************面板前缀***************************
        private static Vector2 ACHierarchyTool_ScrollRoot { get; set; }
        public static string ACHierarchyTool_Prefix { get; set; } //Hierarchy面板的前缀
        public static string ACHierarchyTool_InputCustom { get; private set; }//输入自定义
        public static string[] ACHierarchyTool_options { get; private set; } = new string[] { "None", "T_" };
        public static int ACHierarchyTool_index { get; private set; } = 0;
        /// <summary>
        /// HierarchyPanel前缀
        /// </summary>
        public static void ACHierarchyPrefix()
        {
            ACHierarchyTool_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyTool_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Hierarchy前缀工具", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();
                    {
                        ACHierarchyTool_Prefix = EditorGUILayout.TextField("请输入组件查找前缀", ACPrefix());//ACHierarchyTool_Prefix
                        if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetSelection().ACSaveModification(); }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("清空前缀", EditorStyles.miniButtonMid))
                        {
                            ACHierarchyTool_Prefix = string.Empty;
                            ACHierarchyTool_index = 0;
                        }
                        if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                        {
                            ACHierarchyTool_Prefix = ACToolExpansionFind.ACGetSelectionOne().ACGetPrefix();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetSelection().ACAddPrefixLoop(ACHierarchyTool_Prefix); }
                        if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetSelection().ACRemovePrefix(ACHierarchyTool_Prefix); }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("去除空白和特殊字符", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetSelection().ClearTrim(); }
                    ACHierarchyTool_index = EditorGUILayout.Popup("选择常用前缀:", ACHierarchyTool_index, ACHierarchyTool_options);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        ///按钮的选择
        /// </summary>
        /// <returns></returns>
        public static string ACPrefix()
        {
            ACHierarchyTool_Prefix = String.Empty;
            return ACHierarchyTool_index == 0 ? ACHierarchyTool_Prefix : ACHierarchyTool_options[ACHierarchyTool_index];
        }

        //***************************替换物体***************************
        /// <summary>
        /// 新的物体
        /// </summary>
        public GameObject newPrefab { get; set; }

        /// <summary>
        /// 预制体
        /// </summary>
        public static GameObject tonewPrefab { get; set; }

        /// <summary>
        /// 重命名
        /// </summary>
        private string replaceName { get; set; } = String.Empty;

        /// <summary>
        /// 转换物体
        /// </summary>
        public void ACHierarchyPrefabChange()
        {
            ACHierarchyTool_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyTool_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Hierarchy替换物体", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.BeginVertical("box");
                    {
                        //******************************一键替换场景中的物体******************************
                        GUILayout.Space(5f); EditorGUILayout.LabelField("选择一个新的物体");
                        newPrefab = (GameObject)EditorGUILayout.ObjectField(newPrefab, typeof(GameObject), true, GUILayout.MinWidth(100f));
                        tonewPrefab = newPrefab;
                        if (GUILayout.Button("替换选中的物体")) { ReplaceObjects(); }
                        replaceName = EditorGUILayout.TextField("需要替换的物体名字", replaceName);
                        if (GUILayout.Button("替换相同名字的")) { ReplaceObjectsByName(replaceName, false); }
                        if (GUILayout.Button("替换包含名字的 慎用")) { ReplaceObjectsByName(replaceName, true); }
                        if (GUILayout.Button("保存修改")) { EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()); }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

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
                transform.ACLoopGetAllTransform(ref all);
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

        /// <summary>
        /// 替换的数据
        /// </summary>
        public class ReplaceData
        {
            public GameObject old;
            public GameObject replace;
            public int index = 0;
        }

        //***************************脚本相关***************************
        /// <summary>
        /// 移除脚本
        /// </summary>
        public static void ACHierarchyRemoveScript()
        {
            ACHierarchyTool_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyTool_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Hierarchy通用工具", EditorStyles.boldLabel);
                    //******************************移除丢失脚本******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("移除丢失脚本:", EditorStyles.largeLabel);
                    if (GUILayout.Button("移除丢失脚本", EditorStyles.miniButtonMid)) { RemoveMissScript(); }
                    ACHierarchyTool_InputCustom = EditorGUILayout.TextField("自定义脚本的名称", ACHierarchyTool_InputCustom);
                    if (GUILayout.Button("移除自定义脚本(单个物体和其子物体)", EditorStyles.miniButtonMid)) { ACRemoveCustomScript(); }
                    if (GUILayout.Button("移除自定义脚本(选中的物体不包括其子物体)", EditorStyles.miniButtonMid)) { ACRemoveSelectCustomScript(); }
                    if (GUILayout.Button("添加脚本(选中的物体不包括其子物体)", EditorStyles.miniButtonMid)) { ACAddSelectCustomScript(); }
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Animator", EditorStyles.miniButtonMid)) { ACHierarchyTool_InputCustom = "Animator"; }
                        if (GUILayout.Button("Button", EditorStyles.miniButtonMid)) { ACHierarchyTool_InputCustom = "Button"; }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// 移除丢失的脚本
        /// </summary>
        private static void RemoveMissScript()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < Selection.objects?.Length; i++)
                (Selection.objects[i] as GameObject).transform.ACLoopGetAllGameObject(ref gameObjects); //获取所有的物体
            //移除所有的物体的丢失脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObjects[i]);
            AssetDatabase.Refresh();
            Debug.Log("清理完成!");

        }

        /// <summary>
        /// 移除自定义脚本(单个物体和其子物体)
        /// </summary>
        public static void ACRemoveCustomScript()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            //获取所有的组件
            ACToolExpansionFind.ACLoopGetAllGameObject(ACToolExpansionFind.ACGetSelectionOneGo().transform, ref gameObjects);
            //移除自定义脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyTool_InputCustom);
        }

        /// <summary>
        /// 移除自定义脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACRemoveSelectCustomScript()
        {
            List<GameObject> gameObjects = ACToolExpansionFind.ACGetSelection().ACGetSelectionGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyTool_InputCustom);
        }

        /// <summary>
        /// 添加脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACAddSelectCustomScript()
        {
            List<GameObject> gameObjects = ACToolExpansionFind.ACGetSelection().ACGetSelectionGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACAddScript(ACHierarchyTool_InputCustom);
        }

        //***************************代码获取组件***************************
        private static Vector2 ACHierarchyPanelCode_scrollRoot { get; set; }
        private static string ACHierarchyPanelCode_KeyValue { get; set; } = "T_";
        private static string ACHierarchyPanelCode_ClassName { get; set; } = String.Empty;
        public static string[] ACHierarchyPanelCode_options { get; set; } //= new string[] { "None" };
        public static int ACHierarchyPanelCode_index { get; private set; } = 0;
        public static bool ACHierarchyPanelCode_isShowComponent = true;

        /// <summary>
        /// HierarchyPanel代码获取组件
        /// </summary>
        public static void ACHierarchyPanelCode(Action<StringBuilder, GameObject, Type> action = null)
        {
            GUI.skin.button.wordWrap = true;
            ACHierarchyPanelCode_scrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyPanelCode_scrollRoot);
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("HierarchyPanel代码获取组件", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    ACHierarchyPanelCode_ClassName = EditorGUILayout.TextField("请输入需要查找的组件", ACShowComponentClassName());
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        ACShowComponentType();
                        //if (GUILayout.Button("Text组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "Text"; }
                        //if (GUILayout.Button("InputField组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "InputField"; }
                        //if (GUILayout.Button("Button组件", GUILayout.Width(0))) { ACHierarchyPanelCode_ClassName = "Button"; }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}组件", EditorStyles.miniButtonMid)) { ACGetComponentCode((sb, go, type) => { action?.Invoke(sb, go, type); }); }
                    //******************************去除组件RayCastTarget*****************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("去除组件RayCastTarget:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"去除组件RayCastTarget", EditorStyles.miniButtonMid)) { Selection.objects.ClearRayCastTarget(); }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// 获取代码组件
        /// </summary>
        public static void ACGetComponentCode(Action<StringBuilder, GameObject, Type> action = null)
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
                    action?.Invoke(sb, gameObject, type);
            }
            sb.ToString().UnityCopyWord();
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 显示组件
        /// </summary>
        public static void ACShowComponentType()
        {
            if (ACHierarchyPanelCode_isShowComponent)
            {
                List<string> namespacelist = new List<string>();
                List<string> classlist = new List<string>();
                classlist.Add("None");
                //Assembly asm = Assembly.GetExecutingAssembly();

                Assembly assem = Assembly.Load("UnityEngine.UI");
                Type[] types = assem.GetTypes();
                foreach (Type type in types)
                {
                    if (type.Namespace == "UnityEngine.UI")
                        namespacelist.Add(type.Name);
                }
                foreach (string classname in namespacelist)
                {
                    //需要显示的过滤
                    if (classname.StartsWith("I") || classname.StartsWith("<"))
                        continue;
                    classlist.Add(classname);
                }
                ACHierarchyPanelCode_options = classlist.ToArray();
                ACHierarchyPanelCode_isShowComponent = false;
            }
            ACHierarchyPanelCode_index = EditorGUILayout.Popup("选择常用组件:", ACHierarchyPanelCode_index, ACHierarchyPanelCode_options);
        }

        /// <summary>
        /// 显示组件
        /// </summary>
        public static string ACShowComponentClassName()
        {
            ACHierarchyPanelCode_ClassName = String.Empty;
            return ACHierarchyPanelCode_index == 0 ? ACHierarchyPanelCode_ClassName : ACHierarchyPanelCode_options[ACHierarchyPanelCode_index];
        }
    }
}
