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

        private static Vector2 ACHierarchyTool_ScrollRoot { get; set; }
        

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
            ACHierarchyPrefabChange();
        }
       
        

        

        //***************************替换物体***************************
        private GameObject newPrefab { get; set; }//新的物体
        private static GameObject tonewPrefab { get; set; }//预制体
        private string replaceName { get; set; } = String.Empty;//重命名

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

        

        //***************************代码获取组件***************************
        private static Vector2 ACHierarchyPanelCode_scrollRoot { get; set; }
        private static string ACHierarchyPanelCode_KeyValue { get; set; } = "T_";
        private static string ACHierarchyPanelCode_ClassName { get; set; } = String.Empty;
        private static string[] ACHierarchyPanelCode_options1 { get; set; } //= new string[] { "None" };
        private static int ACHierarchyPanelCode_index1 { get; set; } = 0;
        private static bool ACHierarchyPanelCode_isShowComponent1 { get; set; } = true;
        private static string[] ACHierarchyPanelCode_options2 { get; set; } //= new string[] { "None" };
        private static int ACHierarchyPanelCode_index2 { get; set; } = 0;
        private static bool ACHierarchyPanelCode_isShowComponent2 { get; set; } = true;

        /// <summary>
        /// HierarchyPanel代码获取组件
        /// </summary>
        /// <param name="action"></param>
        public static void ACHierarchyPanelGetCode(ACHierarchyPanelGetCodeConfig aCHierarchyPanelGetCodeConfig = null)
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
                    ACShowComponentType2();
                    //ACShowComponentType1();
                    //******************************获取物体变量******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取物体变量或属性:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}变量", EditorStyles.miniButtonMid))
                        {
                            ACGetComponentCode(aCHierarchyPanelGetCodeConfig.actionGetCode);
                        }
                        if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}属性", EditorStyles.miniButtonMid))
                        {
                            ACGetComponentCode(aCHierarchyPanelGetCodeConfig.actionGetCodeGetSet);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************获取组件******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取组件:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}组件(自己)", EditorStyles.miniButtonMid))
                    {
                        Debug.Log("未写");
                    }
                    if (GUILayout.Button($"获取{ACHierarchyPanelCode_ClassName}组件", EditorStyles.miniButtonMid))
                    {
                        AcGetComponentFind(aCHierarchyPanelGetCodeConfig.actionGetComponent);
                    }
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
            List<GameObject> obj = ACToolCoreExpansionFind.ACGetObjs().ACGetGos();
            if (obj == null)
            {
                Debug.Log("未选中物体");
                return;
            }
            for (int i = 0; i < obj?.Count; i++)
                obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);

            //清理不匹配的开头
            List<GameObject> newGos = gameObjects.FindAll((go) => { return go.name.StartsWith(ACHierarchyPanelCode_KeyValue); });

            //获取类型
            Type type = null;
            type = ACHierarchyPanelCode_ClassName.ACReflectClass("UnityEngine.UI");
            if (type == null)
                type = ACHierarchyPanelCode_ClassName.ACReflectClass("UnityEngine");

            //拼接
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < newGos?.Count; i++)
            {
                GameObject gameObject = newGos[i];
                if (gameObject.transform.GetComponent(type) != null)
                    action?.Invoke(sb, gameObject, type);
                else
                    Debug.Log("没有组件类型,请检查!");
            }
            sb.ACCopyWord();
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 显示组件
        /// </summary>
        public static void ACShowComponentType1()
        {
            if (ACHierarchyPanelCode_isShowComponent1)
            {
                List<string> namespacelist = new List<string>();
                List<string> classlist = new List<string>() { "None" };
                //Assembly asm = Assembly.GetExecutingAssembly();
                Assembly assem = Assembly.Load("UnityEngine.UI");
                Array.ForEach(assem.GetTypes(), (type) =>
                {
                    if (type.Namespace == "UnityEngine.UI")
                        namespacelist.Add(type.Name);
                });

                foreach (var classname in namespacelist)
                {
                    if (classname.StartsWith("I") || classname.StartsWith("<")) continue;
                    classlist.Add(classname);
                }
                ACHierarchyPanelCode_options1 = classlist.ToArray();
                ACHierarchyPanelCode_isShowComponent1 = false;
            }
            ACHierarchyPanelCode_index1 = EditorGUILayout.Popup("选择常用组件1:", ACHierarchyPanelCode_index1, ACHierarchyPanelCode_options1);
        }

        /// <summary>
        /// 显示组件
        /// </summary>
        public static void ACShowComponentType2()
        {
            if (ACHierarchyPanelCode_isShowComponent2)
            {
                List<string> classlist = new List<string>()
                {
                   "None", "Text","Button","Toggle","Transform",
                   "Animator",
                };
                ACHierarchyPanelCode_options2 = classlist.ToArray();
                ACHierarchyPanelCode_isShowComponent2 = false;
            }
            ACHierarchyPanelCode_index2 = EditorGUILayout.Popup("选择常用组件2:", ACHierarchyPanelCode_index2, ACHierarchyPanelCode_options2);
        }

        /// <summary>
        /// 显示组件
        /// </summary>
        public static string ACShowComponentClassName()
        {
            ACHierarchyPanelCode_ClassName = String.Empty;
            return ACHierarchyPanelCode_index2 == 0 ? ACHierarchyPanelCode_ClassName : ACHierarchyPanelCode_options2[ACHierarchyPanelCode_index2];
        }

        /// <summary>
        /// 获取组件查找
        /// </summary>
        public static void AcGetComponentFind(Action<StringBuilder, GameObject, Type, string> action)
        {
            //获取所有的包含子物体和隐藏的
            GameObject tempGo = ACToolCoreExpansionFind.ACGetGo;
            List<GameObject> gos = tempGo.ACLoopGetKeywordGO(ACHierarchyPanelCode_KeyValue);
            //删选带有组件的
            Type type = null;//获取类型
            List<GameObject> tempGos = gos.FindAll((go) =>
            {
                type = ACHierarchyPanelCode_ClassName.ACReflectClass("UnityEngine.UI");
                if (type == null)
                    type = ACHierarchyPanelCode_ClassName.ACReflectClass("UnityEngine");
                return go.GetComponent(type) != null;
            });
            //添加到字典
            Dictionary<GameObject, string> keyValuePairs = new Dictionary<GameObject, string>();
            tempGos?.ForEach((go) =>
            {
                string path = go.transform.ACGetPathTransform(tempGo.name);
                keyValuePairs.Add(go, path);
            });
            //变成代码
            StringBuilder sb = new StringBuilder();

            foreach (GameObject item in keyValuePairs.Keys)
                action?.Invoke(sb, item, type, keyValuePairs[item]);
            sb.ACCopyWord();
            Debug.Log(sb.ToString());
        }
    }
}
