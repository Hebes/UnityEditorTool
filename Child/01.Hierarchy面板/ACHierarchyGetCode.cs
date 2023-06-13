using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    获取代码脚本

-----------------------*/

namespace ACTool
{
    public class ACHierarchyGetCode : EditorWindow
    {
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
        public static void OnShow(ACHierarchyPanelGetCodeConfig aCHierarchyPanelGetCodeConfig = null)
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
            List<GameObject> obj = ACCoreExpansion_Find.ACGetObjs().ACGetGos();
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
            GameObject tempGo = ACCoreExpansion_Find.ACGetGo;
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
