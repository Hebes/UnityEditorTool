using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


namespace ACTool
{
    /// <summary>
    /// Ui组件自动化获取
    /// </summary>
    public class ACUIGetCodeNoPath : EditorWindow
    {
        private static string ACUIGetCodeNoPath_Prefix { get; set; } = string.Empty;//关键的前缀
        private static string ACUIGetCodeNoPath_InputPrefix { get; set; }//输入物体的Transform，就是前缀
        private static Vector2 ACUIGetCodeNoPath_scrollRoot { get; set; }

        public static string[] options = new string[] { "Transform", "Button", "InputField", "Text" };
        public static int index = 0;

        //[MenuItem("GameObject/组件查找和重命名(Shift+A) #A", false, 0)]
        //[MenuItem("Assets/组件查找和重命名(Shift+A) #A")]
        //[MenuItem("Tool/组件查找和重命名(Shift+A) #A", false, 0)]
        [MenuItem("Assets/UI组件获取工具/Transform组件查找-Transform没路径(Shift+A) ")]//#A
        public static void GeneratorFindComponentTool() => GetWindow(typeof(ACUIGetCodeNoPath), false, "Transform组件查找(没路径)").Show();
        private void OnGUI()
        {
            ACHierarchyTool.ACHierarchyPrefix();
            ACHierarchyTool.ACHierarchyPanelCode();
            OnUIAutoForTransformNoPath();
        }

        public static void OnUIAutoForTransformNoPath()
        {
            ACUIGetCodeNoPath_scrollRoot = EditorGUILayout.BeginScrollView(ACUIGetCodeNoPath_scrollRoot); //开启滚动视图
            {
                //******************************Transform组件查找打印 * *****************************配合Transform拓展
                GUILayout.BeginVertical("box");
                {
                    //index = EditorGUILayout.Popup("选择组件:", index, options);
                    //******************************Transform组件查找打印******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取属性或变量(二选一):", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取属性", EditorStyles.miniButtonMid))
                    {
                        GetComonpentProperty(new ACToolFindConfig() { KeyValue = ACUIGetCodeNoPath_Prefix, isGetSet = true, });
                    }
                    if (GUILayout.Button("获取变量", EditorStyles.miniButtonMid))
                    {
                        GetComonpentProperty(new ACToolFindConfig() { KeyValue = ACUIGetCodeNoPath_Prefix, isGetSet = false, });
                    }
                    //******************************获取组件******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取组件:", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取组件(赋值版)", EditorStyles.miniButtonMid))
                    {
                        GetComponentFind(new ACToolFindConfig()
                        {
                            isAssign = true,
                            isAddPrefix = true,
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                            beginStr = ACUIGetCodeNoPath_InputPrefix,
                        });
                    }
                    if (GUILayout.Button("获取组件(不赋值版)", EditorStyles.miniButtonMid))
                    {
                        GetComponentFind(new ACToolFindConfig()
                        {
                            isAssign = false,
                            isAddPrefix = true,
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                            beginStr = ACUIGetCodeNoPath_InputPrefix,
                        });
                    }
                    //******************************获取监听******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("按钮监听代码:", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取监听", EditorStyles.miniButtonMid))
                    {
                        GetComonpentListener(new ACToolFindConfig()
                        {
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                            beginStr = ACUIGetCodeNoPath_InputPrefix,
                        });
                    }
                    //******************************一键生成******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField($"一键生成:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"一键获取(GetSet版本)", EditorStyles.miniButtonMid))
                    {
                        OneKeyGeneration(new ACToolFindConfig()
                        {
                            isAssign = true,
                            beginStr = ACUIGetCodeNoPath_InputPrefix,
                            isAddPrefix = true,
                            isGetSet = true,
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                        });
                    }
                    if (GUILayout.Button($"一键获取(变量版本)", EditorStyles.miniButtonMid))
                    {
                        OneKeyGeneration(new ACToolFindConfig()
                        {
                            isAssign = true,
                            beginStr = ACUIGetCodeNoPath_InputPrefix,
                            isAddPrefix = true,
                            isGetSet = false,
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                        });
                    }
                    //******************************获取选中的物体组件获取******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField($"获取选中的物体组件获取:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"获取选中的物体组件获取", EditorStyles.miniButtonMid))
                    {
                        GetSelectGoCompent(new ACToolFindConfig());
                    }
                    //******************************一键去除组件RayCast Target******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("一键去除组件RayCast Target:", EditorStyles.largeLabel);
                    if (GUILayout.Button("一键去除组件RayCast Target", EditorStyles.miniButtonMid)) { Selection.objects.ClearRayCastTarget(); }
                    //******************************获取所有的T_开头的******************************
                    EditorGUILayout.Space(5f); EditorGUILayout.LabelField("获取所有T_开头的物体的名称:", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取所有T_开头的物体的名称", EditorStyles.miniButtonMid))
                    {
                        GetALlGoName(new ACToolFindConfig()
                        {
                            KeyValue = ACUIGetCodeNoPath_Prefix,
                        });
                    }
                }
                GUILayout.EndVertical(); GUILayout.Space(5f);
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        //******************************组合功能******************************
        #region 组件属性
        /// <summary>
        /// 生成组件属性代码
        /// </summary>
        /// <param name="findtConfig">配置文件</param>
        private static void GetComonpentProperty(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            string str1 = ComonpentProperty(findtConfig);
            Debug.Log(str1.ToString());
            GUIUtility.systemCopyBuffer = str1.ToString();
        }

        #endregion

        #region 组件查找

        /// <summary>
        /// 生成组件查找代码
        /// </summary>
        private static void GetComponentFind(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            string str1 = ComponentFind(findtConfig);

            Debug.Log(str1.ToString());
            GUIUtility.systemCopyBuffer = str1.ToString();
        }
        #endregion

        #region 组件监听

        /// <summary>
        /// 生成监听代码
        /// </summary>
        private static void GetComonpentListener(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            string str1 = ComonpentListener(findtConfig);

            Debug.Log(str1.ToString());
            GUIUtility.systemCopyBuffer = str1.ToString();
        }
        #endregion

        #region 一键生成

        /// <summary>
        /// 一键生成
        /// </summary>
        /// <param name="KeyValue"></param>
        private static void OneKeyGeneration(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            Dictionary<string, List<Component>> ComponentsDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            findtConfig.controlDic = ComponentsDic;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region UI代码 自动化生成");
            //组件属性
            sb.AppendLine(ComonpentProperty(findtConfig));
            //组件获取
            sb.AppendLine(ComponentFind(findtConfig));
            //组件监听
            sb.AppendLine(ComonpentListener(findtConfig));
            sb.AppendLine("#endregion");
            Debug.Log(sb.ToString());
            GUIUtility.systemCopyBuffer = sb.ToString();
        }
        #endregion

        #region 获取选中的物体的组件

        /// <summary>
        /// 获取选中的物体的组件
        /// </summary>
        /// <param name="KeyValue"></param>
        private static void GetSelectGoCompent(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj1 = Selection.objects.First() as GameObject;
            List<Component> components = new List<Component>();
            List<string> vs = new List<string>()
        {
            typeof(Image).Name,
            typeof(Transform).Name,
            typeof(Button).Name ,
            typeof(CanvasGroup).Name ,
            typeof(Text).Name ,
            //请后续自行添加
        };
            vs.ForEach((str) =>
            {
                Component Temp = obj1.transform.GetComponent(str);
                if (Temp != null)
                    components.Add(Temp);
            });
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 自动化获取代码组件");
            //属性代码
            foreach (var item in components)
            {
                string temp = TypeChange(item.GetType().Name);
                sb.AppendLine($"public {temp} {item.gameObject.name}{temp} {{ set; get; }}");
            }
            sb.AppendLine();
            //获取组件代码
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// 获取选中物体的组件");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("private void OnGetSelectComponent()");
            sb.AppendLine("{");
            foreach (var item in components)
            {
                string temp = TypeChange(item.GetType().Name);
                sb.AppendLine($"\t{item.gameObject.name}{temp} = GetComponent<{temp}>();");
            }
            sb.AppendLine("}");
            sb.AppendLine("#endregion");
            Debug.Log(sb);
            GUIUtility.systemCopyBuffer = sb.ToString();
        }
        #endregion

        #region 获取所有T_开头的物体的名称
        /// <summary>
        /// 获取所有的T_开头的物体的名称
        /// </summary>
        public static void GetALlGoName(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            List<string> goNameList = new List<string>();
            foreach (KeyValuePair<string, List<Component>> item in findtConfig.controlDic)
            {
                item.Value?.ForEach((go) =>
                {
                    bool isExist = goNameList.Exists((name) => { return name == go.name; });
                    if (isExist == false) { goNameList.Add(go.name); }
                });
            }
            //打印
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < goNameList.Count; i++)
            {
                sb.AppendLine($"//{goNameList[i]}");
            }
            //复制
            GUIUtility.systemCopyBuffer = sb.ToString();
            Debug.Log(sb.ToString());
        }
        #endregion

        //******************************基础功能******************************
        #region 基础功能

        /// <summary>
        /// 组件属性
        /// </summary>
        /// <param name="obj"></param>
        public static string ComonpentProperty(ACToolFindConfig findtConfig)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 组件模块 不要的代码请自行删除");
            foreach (string Key in findtConfig.controlDic?.Keys)
            {
                string type = TypeChange(Key);//类型
                findtConfig.controlDic[Key]?.ForEach((component) =>
                {
                    string componentName = UIFindComponent.ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                    if (findtConfig.isGetSet)
                        sb.AppendLine($"public {type} {componentName}{type} {{ set; get; }}");
                    else
                        sb.AppendLine($"public {type} {componentName}{type};");
                });
                sb.AppendLine();
            }
            sb.AppendLine("#endregion");
            return sb.ToString();
        }

        /// <summary>
        /// 组件查找
        /// </summary>
        /// <param name="beginStr"></param>
        /// <param name="controlDic"></param>
        public static string ComponentFind(ACToolFindConfig findtConfig)
        {
            //添加前缀
            string beginStr = findtConfig.isAddPrefix ? AddPrefix1(findtConfig.beginStr) : string.Empty;
            //打印
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 获取组件模块");
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// 获取组件");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("private void OnGetComponent()");
            sb.AppendLine("{");
            foreach (string Key in findtConfig.controlDic?.Keys)
            {
                string type = TypeChange(Key);//类型
                findtConfig.controlDic[Key]?.ForEach((component) =>
                {
                    string componentName = UIFindComponent.ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                    if (findtConfig.isAssign)
                        sb.AppendLine($"\t{componentName}{type} = {beginStr}OnGet{type}(\"{componentName}\");");
                    else
                        sb.AppendLine($"\t{beginStr}OnGet{type}(\"{componentName}\");");
                });
                sb.AppendLine();
            }
            sb.AppendLine("}");
            sb.AppendLine("#endregion");

            return sb.ToString();
        }


        /// <summary>
        /// 组件监听
        /// </summary>
        /// <param name="beginStr"></param>
        /// <param name="controlDic"></param>
        public static string ComonpentListener(ACToolFindConfig findtConfig)
        {
            //添加前缀
            //string beginStr = findtConfig.isAddPrefix ? AddPrefix1(findtConfig.beginStr) : string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 按钮监听模块");
            sb.AppendLine("/// <summary>"); sb.AppendLine("/// 按钮监听"); sb.AppendLine("/// </summary>");
            sb.AppendLine("private void OnAddListener()");
            sb.AppendLine("{");
            foreach (string Key in findtConfig.controlDic?.Keys)
            {
                switch (Key)
                {
                    case "Button":
                        //V_HeiShiButton.onClick.AddListener(cityUI.HeiShi); 模板
                        findtConfig.controlDic[Key]?.ForEach((component) =>
                        {
                            string componentName = UIFindComponent.ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                            sb.AppendLine($"\t{componentName}{Key}.onClick.AddListener({componentName}AddListener);");
                        });
                        sb.AppendLine();
                        break;
                    case "Toggle":
                        //toggle.onValueChanged.AddListener(toggleAddListener); 模板
                        findtConfig.controlDic[Key]?.ForEach((component) =>
                        {
                            string componentName = UIFindComponent.ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                            sb.AppendLine($"\t{componentName}{Key}.onValueChanged.AddListener({componentName}AddListener);");
                        });
                        sb.AppendLine();
                        break;
                    default:
                        break;
                }
            }
            sb.AppendLine("}");
            sb.AppendLine("#endregion");
            return sb.ToString();
        }

        #endregion

        //******************************其他******************************
        #region 其他

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        private static string TypeChange(string typeStr)
        {
            switch (typeStr)
            {
                case "RectTransform": return "Transform";
                default: return typeStr;
            }
        }
        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <returns></returns>
        private static string AddPrefix1(string beginStr)
        {
            return !string.IsNullOrEmpty(beginStr) ? $"{beginStr}." : beginStr;//添加前缀
        }

        #endregion
    }
}