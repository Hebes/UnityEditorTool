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
        private static string ETUITool_Prefix { get; set; } = "T_";//关键的前缀
        public static string ETUITool_InputPrefix { get; set; }//输入物体的Transform，就是前缀
        private static Vector2 ETUITool_ScrollRoot { get; set; }
        private ACHierarchyPanelGetCodeConfig aCHierarchyPanelGetCodeConfig;

        [MenuItem("Assets/UI组件获取工具/ET专用工具-暗沉(Shift+E) ")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ETUITool), false, "ET工具-暗沉").Show();
        }
        private void Awake()
        {
            aCHierarchyPanelGetCodeConfig = new ACHierarchyPanelGetCodeConfig();
            aCHierarchyPanelGetCodeConfig.actionGetCode = ACETUIToolShowCode;
        }

        private void OnGUI()
        {
            ACHierarchyTool.ACHierarchyPrefix();
            ACHierarchyTool.ACHierarchyPanelGetCode(aCHierarchyPanelGetCodeConfig);
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
                    //******************************生成GameObject专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("ReferenceCollector获取组件代码:", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("RC获取代码", EditorStyles.miniButtonMid)) { GetUIALlGoName(new ACToolConfig() { KeyValue = ETUITool_Prefix, isGetSet = false, }); }
                        if (GUILayout.Button("RC获取代码Get.Set", EditorStyles.miniButtonMid)) { GetUIALlGoName(new ACToolConfig() { KeyValue = ETUITool_Prefix, isGetSet = true, }); }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************获取组件专用******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取组件专用:", EditorStyles.largeLabel);
                    if (GUILayout.Button("必须的添加", EditorStyles.miniButtonMid)) { GUIUtility.systemCopyBuffer = "ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();"; }
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("获取组件专用System", EditorStyles.miniButtonMid)) { GetALlComponent(new ACToolConfig() { KeyValue = ETUITool_Prefix, }); }
                        if (GUILayout.Button("获取组件专用System的方法", EditorStyles.miniButtonMid)) { GetALlComponentGetSet(); }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************去除组件RayCastTarget*****************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("去除组件RayCastTarget:", EditorStyles.largeLabel);
                    if (GUILayout.Button("去除组件RayCastTarget", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetObjs().ClearRayCastTarget(); }
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
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// ETReferenceCollector自动化组件专用
        /// </summary>
        public static void ETReferenceCollectorTool()
        {
            //List<GameObject> gameObjects = new List<GameObject>();
            //GameObject obj = Selection.objects.First() as GameObject;
            //obj.transform.ACLoopGetKeywordGO(ETUITool_Prefix, ref gameObjects);

            //obj.GetComponent<ReferenceCollector>().data.Clear();//清空原来的数据

            //gameObjects?.ForEach((go) =>
            //{
            //    obj.GetComponent<ReferenceCollector>().data.Add(new ReferenceCollectorData()
            //    {
            //        key = go.name,
            //        gameObject = go.gameObject,
            //    });
            //});
        }

        /// <summary>
        /// 获取组件专用
        /// </summary>
        public static void GetALlComponent(ACToolConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            List<GameObject> gameObjects = new List<GameObject>();
            obj.transform.ACLoopGetKeywordGO(ETUITool_Prefix, ref gameObjects);
            //打印
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region ");
            for (int i = 0; i < gameObjects.Count; i++)
            {
                sb.AppendLine($"\tself.{gameObjects[i].name} = rc.Get<GameObject>(\"{gameObjects[i].name}\");");
            }
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
            List<GameObject> obj = ACToolExpansionFind.ACGetObjs().ACGetGos();
            for (int i = 0; i < obj?.Count; i++)
                obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);
            //拼接
            Type type = ("Button").ACReflectClass("UnityEngine.UI");
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
            sb.ToString().ACUnityCopyWord();
            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// 生成GameObject专用
        /// </summary>
        public static void GetUIALlGoName(ACToolConfig findtConfig)
        {
            string isGetSet = findtConfig.isGetSet ? "{ get; set; }" : ";";
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            List<GameObject> gameObjects = new List<GameObject>();
            obj.transform.ACLoopGetKeywordGO(ETUITool_Prefix, ref gameObjects);
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
            //ET.BuildAssemblieEditor.BuildCodeDebug();
            //ET.BuildAssemblieEditor.BuildCodeRelease();
        }

        /// <summary>
        /// ET的代码
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gameObject"></param>
        /// <param name="type"></param>
        private static void ACETUIToolShowCode(StringBuilder sb, GameObject gameObject, Type type)
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
                    sb.AppendLine($"self.{gameObject.name}.GetComponent<Button>().onClick.AddListener(self.On{gameObject.name});");
                    break;
            }
        }
    }
}

