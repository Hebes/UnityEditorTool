using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace ACTool
{
    /// <summary>
    /// UI组件全路径查找
    /// </summary>
    public class ACUIDemoAllPath : EditorWindow
    {
        public static string ACUIDemoAllPath_Prefix { get; set; } = "T_";
        public static string ACUIDemoAllPath_ClassName { get; set; }
        private static Vector2 ACUIDemoAllPath_scrollRoot { get; set; }

        private static List<string> ACUIDemoAllPath_options { get; set; } = new List<string>()
        {
             "Text","Button","Toggle","Transform","Animator",
        };

        [MenuItem("Assets/UI组件获取工具/Transform组件查找-Transform全路径(Shift+A) ")]//#A
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACUIDemoAllPath), false, "Transform组件查找(全路径)").Show();
        }
        private void OnGUI()
        {
            ACHierarchyTool.ACHierarchyPrefix();
            ACUIDemoAllPath.ACUIDemoFIndAllPath();
        }

        public static void ACUIDemoFIndAllPath()
        {
            GUI.skin.button.wordWrap = true;
            ACUIDemoAllPath_scrollRoot = EditorGUILayout.BeginScrollView(ACUIDemoAllPath_scrollRoot);
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("ACUIDemoAllPath代码获取组件", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5f);
                    //******************************前缀******************************
                    EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    ACUIDemoAllPath_ClassName = EditorGUILayout.TextField("请输入需要查找的组件", ACUIDemoAllPath_ClassName);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        foreach (string item in ACUIDemoAllPath_options)
                        {
                            if (GUILayout.Button($"{item}", EditorStyles.miniButtonMid))
                            {
                                ACUIDemoAllPath_ClassName = item;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(5f);
                    //******************************获取物体变量******************************
                    EditorGUILayout.LabelField("获取物体变量或属性:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button($"获取{ACUIDemoAllPath_ClassName}变量(包含子物体)", EditorStyles.miniButtonMid))
                        {
                            ACGetComponentCode(ACUIToolShowCode);
                        }
                        if (GUILayout.Button($"获取{ACUIDemoAllPath_ClassName}属性(包含子物体)", EditorStyles.miniButtonMid))
                        {
                            ACGetComponentCode(ACUIToolShowCodeGetSet);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(5f);
                    //******************************获取组件******************************
                    EditorGUILayout.LabelField("获取组件:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"获取{ACUIDemoAllPath_ClassName}组件(自己)", EditorStyles.miniButtonMid))
                    {
                        Debug.Log("未写");
                    }
                    if (GUILayout.Button($"获取{ACUIDemoAllPath_ClassName}组件", EditorStyles.miniButtonMid))
                    {
                        AcGetComponentFind(ACUIToolShowComponent);
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
        /// 获取物体的代码
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="go"></param>
        /// <param name="type"></param>
        private static void ACUIToolShowCode(StringBuilder sb, GameObject go, System.Type type)
        {
            sb.AppendLine($"public {type.Name} {go.name}{type.Name};");
        }

        /// <summary>
        /// 获取物体的代码
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="go"></param>
        /// <param name="type"></param>
        private static void ACUIToolShowCodeGetSet(StringBuilder sb, GameObject go, System.Type type)
        {
            sb.AppendLine($"public {type.Name} {go.name}{type.Name} {{get;set;}}");
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="go"></param>
        /// <param name="type"></param>
        /// <param name="str"></param>
        private static void ACUIToolShowComponent(StringBuilder sb, GameObject go, System.Type type, string str)
        {
            sb.AppendLine($"{go.name}{type.Name} = transform.Find(\"{str}\").GetComponent<{type.Name}>();");
        }

        /// <summary>
        /// 获取变量或属性
        /// </summary>
        public static void ACGetComponentCode(Action<StringBuilder, GameObject, Type> action = null)
        {
            //查找自定义的需要的组件
            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> obj = ACToolExpansionFind.ACGetObjs().ACGetGos();
            for (int i = 0; i < obj?.Count; i++)
                obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);

            //清理不匹配的开头
            List<GameObject> newGos = gameObjects.FindAll((go) => { return go.name.StartsWith(ACUIDemoAllPath_Prefix); });

            //获取类型
            Type type = null;
            type = ACUIDemoAllPath_ClassName.ACReflectClass("UnityEngine.UI");
            if (type == null)
                type = ACUIDemoAllPath_ClassName.ACReflectClass("UnityEngine");

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
        /// 获取组件查找
        /// </summary>
        public static void AcGetComponentFind(Action<StringBuilder, GameObject, Type, string> action)
        {
            //获取所有的包含子物体和隐藏的
            GameObject tempGo = ACToolExpansionFind.ACGetGo();
            List<GameObject> gos = tempGo.ACLoopGetKeywordGO(ACUIDemoAllPath_Prefix);
            //删选带有组件的
            Type type = null;//获取类型
            List<GameObject> tempGos = gos.FindAll((go) =>
            {
                type = ACUIDemoAllPath_ClassName.ACReflectClass("UnityEngine.UI");
                if (type == null)
                    type = ACUIDemoAllPath_ClassName.ACReflectClass("UnityEngine");
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
