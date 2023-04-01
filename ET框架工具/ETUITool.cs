using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ETUITool : EditorWindow
    {
        private static string ETUITool_Prefix { get; set; } = String.Empty;//关键的前缀
        public static string ETUITool_InputPrefix { get; set; }//输入物体的Transform，就是前缀
        private static Vector2 ETUITool_ScrollRoot { get; set; }

        [MenuItem("Assets/UI组件获取工具/ET专用工具-暗沉(Shift+E) ")]//#E
        public static void GeneratorFindComponentTool() => GetWindow(typeof(ETUITool), false, "ET工具-暗沉").Show();
        private void OnGUI()
        {
            ACHierarchyPanelCommonEditorTool.ACHierarchyPanelPrefix();
            ACHierarchyPanelCommonEditorTool.ACHierarchyPanelCode();
            OnETUITool();
        }

        public static void OnETUITool()
        {
            ETUITool_ScrollRoot = EditorGUILayout.BeginScrollView(ETUITool_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("ET工具", EditorStyles.boldLabel);
                    //******************************编译代码******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("编译代码:", EditorStyles.largeLabel);
                    if (GUILayout.Button("编译代码", EditorStyles.miniButtonMid)) { BuildCode(); }
                    //******************************ReferenceCollector自动化组件专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("ReferenceCollector自动设置:", EditorStyles.largeLabel);
                    if (GUILayout.Button("ReferenceCollector自动化组件专用", EditorStyles.miniButtonMid)) { ETReferenceCollectorTool(); }
                    EditorGUILayout.EndHorizontal();
                    //******************************生成GameObject专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("ReferenceCollector获取组件代码:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("RC获取代码", EditorStyles.miniButtonMid)) { GetUIALlGoName(new ACFindConfig() { KeyValue = ETUITool_Prefix, isGetSet = false, }); }
                        if (GUILayout.Button("RC获取代码Get.Set", EditorStyles.miniButtonMid)) { GetUIALlGoName(new ACFindConfig() { KeyValue = ETUITool_Prefix, isGetSet = true, }); }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************获取组件专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取组件专用:", EditorStyles.largeLabel);
                    if (GUILayout.Button("必须的添加", EditorStyles.miniButtonMid)) { GUIUtility.systemCopyBuffer = "ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();"; }
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("获取组件专用System", EditorStyles.miniButtonMid)) { GetALlComponent(new ACFindConfig() { KeyValue = ETUITool_Prefix, }); }
                        if (GUILayout.Button("获取组件专用System的方法", EditorStyles.miniButtonMid)) { GetALlComponentGetSet(); }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************去除组件RayCastTarget*****************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("去除组件RayCastTarget:", EditorStyles.largeLabel);
                    if (GUILayout.Button("去除组件RayCastTarget", EditorStyles.miniButtonMid)) { UIAutoForTransformNoPath.ClearRayCastTarget(); }
                    //******************************资源包快速获取名称******************************
                    //GUILayout.Space(5f); EditorGUILayout.LabelField("资源包快速获取名称:", EditorStyles.largeLabel);
                    //EditorGUILayout.BeginHorizontal();//开始水平布局
                    //{
                    //    if (GUILayout.Button("资源包快速获取名称", EditorStyles.miniButtonMid)) 
                    //    {
                    //    }
                    //}
                    //EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndVertical(); GUILayout.Space(5f);
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// ETReferenceCollector自动化组件专用
        /// </summary>
        public static void ETReferenceCollectorTool()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            GameObject obj = Selection.objects.First() as GameObject;
            obj.transform.ACLoopGetKeyValueGO(ETUITool_Prefix, ref gameObjects);

            gameObjects?.ForEach((go) =>
            {
                obj.GetComponent<ReferenceCollector>().data.Add(new ReferenceCollectorData()
                {
                    key = go.name,
                    gameObject = go.gameObject,
                });
            });
        }

        /// <summary>
        /// 获取组件专用
        /// </summary>
        public static void GetALlComponent(ACFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            List<GameObject> gameObjects = new List<GameObject>();
            ACToolExpansion.ACLoopGetKeyValueGO(obj.transform, ETUITool_Prefix, ref gameObjects);
            //打印
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region ");
            for (int i = 0; i < gameObjects.Count; i++)
            {
                sb.AppendLine($"\tself.{gameObjects[i].name} = rc.Get<GameObject>(\"{gameObjects[i].name}\");");
            }
            //sb.AppendLine("#endregion");
            sb.AppendLine();//空行
            sb.AppendLine("#endregion");
            //复制
            GUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log(sb.ToString());
        }
        /// <summary>
        /// 获取组件专用GetSet
        /// </summary>
        public static void GetALlComponentGetSet()
        {
            //查找自定义的需要的组件
            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> obj = ACToolExpansion.ACGetSelectionGos();
            for (int i = 0; i < obj?.Count; i++)
                ACToolExpansion.ACLoopGetAllGO(obj[i].transform, ref gameObjects);
            //拼接
            Type type = ACToolExpansion.ACReflectClass("Button", "UnityEngine.UI");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < gameObjects?.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject.transform.GetComponent(type) != null && gameObject.name.StartsWith("T_"))
                {
                    switch (type.Name)
                    {
                        case "Button":
                            sb.AppendLine($" /// <summary>\r\n        /// \r\n        /// </summary>\r\n        /// <param name=\"self\"></param>");
                            sb.AppendLine($"public static void On{gameObject.name}(this UIMateLoginComponent self)\r\n        {{\r\n\r\n        }}");
                            break;
                    }
                }
            }
            ACToolExpansion.UnityCopyWord(sb.ToString());
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 生成GameObject专用
        /// </summary>
        public static void GetUIALlGoName(ACFindConfig findtConfig)
        {
            string isGetSet = findtConfig.isGetSet ? "{ get; set; }" : ";";
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            List<GameObject> gameObjects = new List<GameObject>();
            ACToolExpansion.ACLoopGetKeyValueGO(obj.transform, ETUITool_Prefix, ref gameObjects);
            //打印
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 请自行填写注释");
            for (int i = 0; i < gameObjects.Count; i++)
                sb.AppendLine($"\tpublic GameObject {gameObjects[i].name}{isGetSet}");
            sb.AppendLine("#endregion");
            //复制
            GUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 编译代码
        /// </summary>
        [MenuItem("Tools/Build/BuildCodeDebug-ac _F5 ")]
        public static void BuildCode()
        {
            ET.BuildAssemblieEditor.BuildCodeDebug();
            ET.BuildAssemblieEditor.BuildCodeRelease();
        }
    }
}

