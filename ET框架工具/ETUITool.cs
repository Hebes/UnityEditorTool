//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEngine;

//namespace ACTool
//{
//    public class ETUITool : EditorWindow
//    {
//        private static string ETUITool_Prefix { get; set; } = "T_";//关键的前缀
//        private static Vector2 ETUITool_ScrollRoot { get; set; }
//        public static string ETUITool_ClassName { get; set; }
//        public static string ETUITool_ETClassName { get; private set; }

//        //[MenuItem("Assets/暗沉EditorTool/文件操作面板")]//#E
//        [MenuItem("Assets/暗沉EditorTool/ET专用工具-暗沉(Shift+E) #E")]//#E UI组件获取工具/
//        public static void GeneratorFindComponentTool()
//        {
//            GetWindow(typeof(ETUITool), false, "ET工具-暗沉").Show();
//        }

//        private void OnGUI()
//        {
//            ACHierarchyToolReNameReName.ACHierarchyPrefix();
//            OnETUITool();
//        }

//        public static void OnETUITool()
//        {
//            ETUITool_ScrollRoot = EditorGUILayout.BeginScrollView(ETUITool_ScrollRoot); //开启滚动视图
//            {
//                EditorGUILayout.BeginVertical("box");
//                {
//                    //******************************文件夹******************************
//                    EditorGUILayout.LabelField("文件夹", EditorStyles.boldLabel);
//                    if (GUILayout.Button("打开Mate目录", EditorStyles.miniButtonMid))
//                    {
//                        ("/").ACOpenPath();
//                    }
//                    if (GUILayout.Button("打开StreamingAssets目录", EditorStyles.miniButtonMid))
//                    {
//                        ("F:\\Yet\\Project\\Mate\\Release\\PC\\StreamingAssets\\StreamingAssets").ACOpenPath();
//                    }
//                    //******************************AB包******************************
//                    EditorGUILayout.LabelField("ET工具", EditorStyles.boldLabel);
//                    if (GUILayout.Button("设置AB包标签", EditorStyles.miniButtonMid))
//                    {
//                        Debug.Log("设置AB包标签");
//                        Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), obj => obj.ACGetAssetDataPath().ACSetABName($"{obj.name}.unity3d"));
//                        ACCoreExpansion_DateSave.ACReAssets();
//                    }
//                    if (GUILayout.Button("打成AB包", EditorStyles.miniButtonMid))
//                    {
//                        UnityEngine.Object[] objs = ACCoreExpansion_Find.ACGetObjs();
//                        Debug.Log(AssetDatabase.GetAssetPath(objs[0]));
//                        AssetBundleBuild[] buildMap = new AssetBundleBuild[objs.Length];
//                        for (int i = 0; i < objs.Length; i++)
//                        {
//                            UnityEngine.Object obj = objs[i];
//                            obj.ACGetAssetDataPath().ACSetABName($"{obj.name.ToLower()}.unity3d");
//                            buildMap[i].assetBundleName = $"{obj.name.ToLower()}.unity3d";
//                            string[] enemyAssets = new string[1];
//                            enemyAssets[0] = AssetDatabase.GetAssetPath(obj);
//                            buildMap[i].assetNames = enemyAssets;
//                        }
//                        string path = "F:/test";// "F:\\Yet\\Project\\Mate\\Release\\PC\\StreamingAssets\\StreamingAssets";
//                        path.ACChackFolder();
//                        BuildPipeline.BuildAssetBundles(path, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
//                        ($"{path}/").ACOpenPath();
//                    }
//                    if (GUILayout.Button("进入目录", EditorStyles.miniButtonMid))
//                    {
//                        string path = "F:/test";
//                        if (!path.ACFolderExist()) return;
//                        ($"{path}/").ACOpenPath();
//                    }
//                    //******************************编译代码******************************
//                    GUILayout.Space(5f); EditorGUILayout.LabelField("编译代码:", EditorStyles.largeLabel);
//                    if (GUILayout.Button("编译代码", EditorStyles.miniButtonMid)) { BuildCode(); }
//                    //******************************ReferenceCollector自动化组件专用******************************
//                    GUILayout.Space(5f); EditorGUILayout.LabelField("ReferenceCollector自动设置:", EditorStyles.largeLabel);
//                    if (GUILayout.Button("ReferenceCollector自动化组件专用", EditorStyles.miniButtonMid)) { ETReferenceCollectorTool(); }
//                    if (GUILayout.Button("ReferenceCollector全场景添加", EditorStyles.miniButtonMid)) { ETScaneReferenceCollectorTool(); }
//                    //******************************生成GameObject专用******************************
//                    GUILayout.Space(5f); EditorGUILayout.LabelField("ReferenceCollector获取组件代码:", EditorStyles.largeLabel);
//                    EditorGUILayout.BeginHorizontal();//开始水平布局
//                    {
//                        //if (GUILayout.Button("RC获取代码", EditorStyles.miniButtonMid)) { GetUIALlGoName(new ACToolConfig() { KeyValue = ETUITool_Prefix, isGetSet = false, }); }
//                        if (GUILayout.Button("RC获取代码Get.Set", EditorStyles.miniButtonMid))
//                        {
//                            GetUIALlGoName(new ACToolConfig() { KeyValue = ETUITool_Prefix, isGetSet = true, });
//                        }
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    //******************************获取组件专用******************************
//                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取组件专用:", EditorStyles.largeLabel);
//                    if (GUILayout.Button("必须的添加", EditorStyles.miniButtonMid)) { GUIUtility.systemCopyBuffer = "ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();"; }
//                    if (GUILayout.Button("获取组件专用System", EditorStyles.miniButtonMid))
//                    {
//                        GetALlComponent(new ACToolConfig() { KeyValue = ETUITool_Prefix, });
//                    }
//                    EditorGUILayout.BeginHorizontal();//开始水平布局
//                    {
//                        ETUITool_ETClassName = EditorGUILayout.TextField("请输入方法里面的类名", ETUITool_ETClassName);
//                        if (GUILayout.Button("获取组件专用System的方法", EditorStyles.miniButtonMid)) { GetALlComponentGetSet(); }
//                        if (GUILayout.Button("清空", EditorStyles.miniButtonMid)) { ETUITool_ETClassName = string.Empty; }
//                    }
//                    EditorGUILayout.EndHorizontal();


//                    //******************************获取物体组件******************************
//                    GUILayout.Space(5f); ETUITool_ClassName = EditorGUILayout.TextField("请输入需要查找的组件", ETUITool_ClassName);
//                    EditorGUILayout.LabelField("获取物体变量或属性:", EditorStyles.largeLabel);
//                    EditorGUILayout.BeginHorizontal();//开始水平布局
//                    {
//                        if (GUILayout.Button($"Button", EditorStyles.miniButtonMid)) { ETUITool_ClassName = "Button"; }
//                        if (GUILayout.Button($"InputField", EditorStyles.miniButtonMid)) { ETUITool_ClassName = "InputField"; }
//                        if (GUILayout.Button($"Text", EditorStyles.miniButtonMid)) { ETUITool_ClassName = "Text"; }
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    EditorGUILayout.BeginHorizontal();//开始水平布局
//                    {
//                        if (GUILayout.Button($"获取{ETUITool_ClassName}变量", EditorStyles.miniButtonMid))
//                        {
//                            ACGetComponentCode(ACETUIToolShowCode);
//                        }
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    //******************************获取组件******************************
//                    //EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取组件:", EditorStyles.largeLabel);
//                    //if (GUILayout.Button($"获取{ETUITool_ClassName}组件", EditorStyles.miniButtonMid))
//                    //{
//                    //    //AcGetComponentFind(ACETUIToolShowCode);
//                    //}
//                }
//                EditorGUILayout.EndVertical();
//            }
//            EditorGUILayout.EndScrollView(); //结束滚动视图
//        }

//        /// <summary>
//        /// ETReferenceCollector自动化组件专用
//        /// </summary>
//        public static void ETReferenceCollectorTool()
//        {
//            List<GameObject> gameObjects = new List<GameObject>();
//            GameObject obj = Selection.objects.First() as GameObject;
//            obj.transform.ACLoopGetKeywordGO(ETUITool_Prefix, ref gameObjects);

//            obj.GetComponent<ReferenceCollector>().data.Clear();//清空原来的数据

//            gameObjects?.ForEach((go) =>
//            {
//                obj.GetComponent<ReferenceCollector>().data.Add(new ReferenceCollectorData()
//                {
//                    key = go.name,
//                    gameObject = go.gameObject,
//                });
//            });
//        }

//        /// <summary>
//        /// 从场景添加
//        /// </summary>
//        public static void ETScaneReferenceCollectorTool()
//        {
//            List<GameObject> gos = ACCoreExpansion_Find.GetAllGameObject().FindAll((go) =>
//            {
//                return go.name.StartsWith(ETUITool_Prefix);
//            });
//            ReferenceCollector referenceCollector = ACCoreExpansion_Find.ACGetGo.GetComponent<ReferenceCollector>();
//            referenceCollector.data.Clear();//清空原来的数据

//            gos?.ForEach((go) =>
//            {
//                referenceCollector.data.Add(new ReferenceCollectorData() { key = go.name, gameObject = go, });
//            });
//        }

//        /// <summary>
//        /// 获取组件专用
//        /// </summary>
//        public static void GetALlComponent(ACToolConfig findtConfig)
//        {
//            //获取到当前选择的物体
//            GameObject obj = Selection.objects.First() as GameObject;
//            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
//            List<GameObject> gameObjects = new List<GameObject>();
//            obj.transform.ACLoopGetKeywordGO(ETUITool_Prefix, ref gameObjects);
//            //打印
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("#region ");
//            for (int i = 0; i < gameObjects.Count; i++)
//            {
//                sb.AppendLine($"\tself.{gameObjects[i].name} = rc.Get<GameObject>(\"{gameObjects[i].name}\");");
//            }
//            sb.AppendLine();//空行
//            sb.AppendLine("#endregion");
//            //复制
//            GUIUtility.systemCopyBuffer = sb.ToString();
//            Debug.Log(sb.ToString());


//            //StringBuilder sb = new StringBuilder();
//            //sb.AppendLine("#region 请自行填写注释");
//            //ACToolCoreExpansionFind.ACGetGo.GetComponent<ReferenceCollector>().data?.ForEach((date) =>
//            //{
//            //    sb.AppendLine($"\tself.{date.gameObject.name} = rc.Get<GameObject>(\"{date.gameObject.name}\");");
//            //});
//            //sb.AppendLine("#endregion");
//            ////复制
//            //GUIUtility.systemCopyBuffer = sb.ToString();
//            //Debug.Log(sb.ToString());
//        }

//        /// <summary>
//        /// 获取组件专用GetSet
//        /// </summary>
//        public static void GetALlComponentGetSet()
//        {
//            if (string.IsNullOrEmpty(ETUITool_ETClassName))
//            {
//                Debug.Log("请输入ET的类型名称");
//                return;
//            }
//            //查找自定义的需要的组件
//            List<GameObject> gameObjects = new List<GameObject>();
//            List<GameObject> obj = ACCoreExpansion_Find.ACGetObjs().ACGetGos();
//            for (int i = 0; i < obj?.Count; i++)
//                obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);
//            //拼接
//            Type type = ("Button").ACReflectClass("UnityEngine.UI");
//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < gameObjects?.Count; i++)
//            {
//                GameObject gameObject = gameObjects[i];
//                if (gameObject.transform.GetComponent(type) != null && gameObject.name.StartsWith("T_"))
//                {
//                    switch (type.Name)
//                    {
//                        case "Button":
//                            sb.AppendLine($" /// <summary>\r\n        /// \r\n        /// </summary>\r\n        /// <param name=\"self\"></param>");
//                            sb.AppendLine($"public static void On{gameObject.name}(this {ETUITool_ETClassName} self)\r\n        {{\r\n\r\n        }}");
//                            break;
//                    }
//                }
//            }
//            sb.ToString().ACUnityCopyWord();
//            Debug.Log(sb.ToString());
//        }


//        /// <summary>
//        /// 生成GameObject专用
//        /// </summary>
//        public static void GetUIALlGoName(ACToolConfig findtConfig)
//        {
//            StringBuilder sb = new StringBuilder();
//            string isGetSet = findtConfig.isGetSet ? "{ get; set; }" : ";";
//            ReferenceCollector referenceCollector = ACCoreExpansion_Find.ACGetGo.GetComponent<ReferenceCollector>();
//            sb.AppendLine("#region 请自行填写注释");
//            referenceCollector.data?.ForEach((date) =>
//            {
//                sb.AppendLine($"\tpublic GameObject {date.gameObject.name}{isGetSet}");
//            });
//            sb.AppendLine("#endregion");
//            GUIUtility.systemCopyBuffer = sb.ToString();//复制
//            Debug.Log(sb.ToString());
//        }

//        /// <summary>
//        /// 编译代码
//        /// </summary>
//        [MenuItem("Tools/Build/BuildCodeDebug-ac _F5 ")]
//        public static void BuildCode()
//        {
//            ET.BuildAssemblieEditor.BuildCodeDebug();
//            //ET.BuildAssemblieEditor.BuildCodeRelease();
//        }

//        /// <summary>
//        /// ET的代码
//        /// </summary>
//        /// <param name="sb"></param>
//        /// <param name="gameObject"></param>
//        /// <param name="type"></param>
//        private static void ACETUIToolShowCode(StringBuilder sb, GameObject gameObject, Type type)
//        {
//            switch (type.Name)
//            {
//                case "Text":
//                    sb.AppendLine($"self.{gameObject.name}.GetComponent<Text>().text = String.Empty;");
//                    break;
//                case "InputField":
//                    sb.AppendLine($"self.{gameObject.name}.GetComponent<InputField>().text = String.Empty;");
//                    break;
//                case "Button":
//                    sb.AppendLine($"self.{gameObject.name}.GetComponent<Button>().onClick.AddListener(self.On{gameObject.name});");
//                    break;
//            }
//        }

//        /// <summary>
//        /// 获取代码组件
//        /// </summary>
//        public static void ACGetComponentCode(Action<StringBuilder, GameObject, Type> action = null)
//        {
//            //查找自定义的需要的组件
//            List<GameObject> gameObjects = new List<GameObject>();
//            List<GameObject> obj = ACCoreExpansion_Find.ACGetObjs().ACGetGos();
//            if (obj == null) { Debug.Log("未选中物体"); return; }
//            for (int i = 0; i < obj?.Count; i++)
//                obj[i].transform.ACLoopGetAllGameObject(ref gameObjects);
//            //清理不匹配的开头
//            List<GameObject> newGos = gameObjects.FindAll((go) => { return go.name.StartsWith(ETUITool_Prefix); });

//            //获取类型
//            Type type = null;
//            type = ETUITool_ClassName.ACReflectClass("UnityEngine.UI");
//            if (type == null)
//                type = ETUITool_ClassName.ACReflectClass("UnityEngine");

//            //拼接
//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < newGos?.Count; i++)
//            {
//                GameObject gameObject = newGos[i];
//                if (gameObject.transform.GetComponent(type) != null)
//                    action?.Invoke(sb, gameObject, type);
//                else
//                    Debug.Log("没有组件类型,请检查!");
//            }
//            sb.ACCopyWord();
//            Debug.Log(sb.ToString());
//        }

//        /// <summary>
//        /// 获取组件查找
//        /// </summary>
//        public static void AcGetComponentFind(Action<StringBuilder, GameObject, Type, string> action)
//        {
//            //获取所有的包含子物体和隐藏的
//            GameObject tempGo = ACCoreExpansion_Find.ACGetGo;
//            List<GameObject> gos = tempGo.ACLoopGetKeywordGO(ETUITool_Prefix);
//            //删选带有组件的
//            Type type = null;//获取类型
//            List<GameObject> tempGos = gos.FindAll((go) =>
//            {
//                type = ETUITool_ClassName.ACReflectClass("UnityEngine.UI");
//                if (type == null)
//                    type = ETUITool_ClassName.ACReflectClass("UnityEngine");
//                return go.GetComponent(type) != null;
//            });
//            //添加到字典
//            Dictionary<GameObject, string> keyValuePairs = new Dictionary<GameObject, string>();
//            tempGos?.ForEach((go) =>
//            {
//                string path = go.transform.ACGetPathTransform(tempGo.name);
//                keyValuePairs.Add(go, path);
//            });
//            //变成代码
//            StringBuilder sb = new StringBuilder();

//            foreach (GameObject item in keyValuePairs.Keys)
//                action?.Invoke(sb, item, type, keyValuePairs[item]);
//            sb.ACCopyWord();
//            Debug.Log(sb.ToString());
//        }
//    }
//}

