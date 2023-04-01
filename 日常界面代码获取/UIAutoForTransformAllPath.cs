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
    public class UIAutoForTransformAllPath : EditorWindow
    {
        /// <summary>
        /// 是否复制到剪切板-tf直接获取的版本
        /// </summary>
        public static bool isCopyBoard3 { get; private set; }
        public static string  TransformGetPrefix { get; private set; }//组件获取的前缀
        public static string TransformPrefix { get; set; } = "T_";
        private static Vector2 scrollRoot { get; set; }

        [MenuItem("Assets/UI组件获取工具/Transform组件查找-Transform全路径(Shift+A) ")]//#A
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(UIAutoForTransformAllPath), false, "Transform组件查找(全路径)").Show();
        }

        private void OnGUI()
        {
            OnUIAutoForTransformAllPath();
        }

        public static void OnUIAutoForTransformAllPath()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
            {
                //******************************Transform组件直接查找打印******************************可以直接使用
                GUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Transform组件查找", EditorStyles.boldLabel);
                    //******************************请输入Transform组件直接查找前缀******************************
                    GUILayout.Space(5f); GUILayout.Label("请输入Transform组件查找前缀:", EditorStyles.largeLabel);
                    if (GUILayout.Button("清空前缀", EditorStyles.miniButtonMid))
                    {
                        TransformGetPrefix = string.Empty;
                    }
                    if (GUILayout.Button("常用前缀:transform", EditorStyles.miniButtonMid))
                    {
                        TransformGetPrefix = "transform";
                    }
                    TransformGetPrefix = GUILayout.TextField(TransformGetPrefix, "BoldTextField");
                    //******************************组件重命名******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField($"前缀添加{TransformPrefix}:", EditorStyles.largeLabel);
                    if (GUILayout.Button($"前缀添加{TransformPrefix}", EditorStyles.miniButtonMid)) { AddPrefix(TransformPrefix); }
                    if (GUILayout.Button($"去除前缀{TransformPrefix}", EditorStyles.miniButtonMid)) { RemovePrefix(TransformPrefix); }
                    if (GUILayout.Button($"去除空白和特殊字符", EditorStyles.miniButtonMid)) { ClearTrim(); }
                    if (GUILayout.Button($"保存修改", EditorStyles.miniButtonMid)) { SaveModification(); }
                    //******************************获取属性******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取属性或变量(二选一):", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取属性", EditorStyles.miniButtonMid))
                    {
                        GetComonpentProperty(new ACToolFindConfig() { isGetSet = true, KeyValue = TransformPrefix, });
                    }
                    if (GUILayout.Button("获取变量", EditorStyles.miniButtonMid))
                    {
                        GetComonpentProperty(new ACToolFindConfig() { isGetSet = false, KeyValue = TransformPrefix, });
                    }
                    //******************************获取组件******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取组件:", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取组件(赋值版)", EditorStyles.miniButtonMid))
                    {
                        GetComonpentFind(new ACToolFindConfig()
                        {
                            isAssign = true,
                        });
                    }
                    if (GUILayout.Button("获取组件(不赋值版)", EditorStyles.miniButtonMid))
                    {
                        GetComonpentFind(new ACToolFindConfig()
                        {
                            isAssign = false,
                        });
                    }
                    //******************************获取监听******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("获取监听:", EditorStyles.largeLabel);
                    if (GUILayout.Button("获取监听", EditorStyles.miniButtonMid))
                    {
                        GetComonpentListener();
                    }
                    //******************************一键获取******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("一键获取:", EditorStyles.largeLabel);
                    if (GUILayout.Button("一键获取(GetSet版本)", EditorStyles.miniButtonMid))
                    {
                        GetAll(new ACToolFindConfig()
                        {
                            isAssign = true,
                            isGetSet = true,
                        });
                    }
                    if (GUILayout.Button("一键获取(变量版本)", EditorStyles.miniButtonMid))
                    {
                        GetAll(new ACToolFindConfig()
                        {
                            isAssign = true,
                            isGetSet = false,
                        });
                    }
                    //******************************提示框******************************
                    if (isCopyBoard3)
                        EditorGUILayout.HelpBox("已经复制到剪切板", MessageType.Info, true);
                }
                GUILayout.EndVertical(); GUILayout.Space(5f);
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        

        //******************************组合功能******************************

        /// <summary>
        /// 获取属性代码
        /// </summary>
        private static void GetComonpentProperty(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, findtConfig.KeyValue);
            StringBuilder sb = new StringBuilder();
            sb.Append(ComonpentProperty(findtConfig)); sb.AppendLine();
            Debug.Log(sb);
            //显示提示信息
            CopyWord(sb.ToString());
            isCopyBoard3 = true;
            CloseBoard();
        }

        /// <summary>
        /// 获取监听
        /// </summary>
        private static void GetComonpentListener()
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            Dictionary<string, List<Component>> controlDic = UIFindComponent.FindComponents1(obj, TransformPrefix);
            StringBuilder sb = new StringBuilder();
            sb.Append(ComonpentListener(controlDic)); sb.AppendLine();
            Debug.Log(sb);
            //显示提示信息
            CopyWord(sb.ToString());
            isCopyBoard3 = true;
            CloseBoard();
        }

        /// <summary>
        /// 一键获取
        /// </summary>
        private static void GetAll(ACToolFindConfig findtConfig)
        {
            //获取到当前选择的物体
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.selectGoName = obj.name;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, TransformPrefix);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("#region UI代码 自动化生成"); sb.AppendLine();
            sb.Append(ComonpentProperty(findtConfig)); sb.AppendLine();
            sb.Append(ComonpentFind(findtConfig)); sb.AppendLine();
            sb.Append(ComonpentListener(findtConfig.controlDic)); sb.AppendLine();
            sb.AppendLine("#endregion");
            Debug.Log(sb);
            //显示提示信息
            CopyWord(sb.ToString());
            isCopyBoard3 = true;
            CloseBoard();
        }

        //TODO 需要修改
        /// <summary>
        /// 直接获取组件--生成组件获取代码
        /// </summary>
        private static void GetComonpentFind(ACToolFindConfig findtConfig)
        {
            StringBuilder sb = new StringBuilder();
            GameObject obj = Selection.objects.First() as GameObject;
            findtConfig.selectGoName = obj.name;
            findtConfig.controlDic = UIFindComponent.FindComponents1(obj, TransformPrefix);
            //获取到当前选择的物体
            sb.AppendLine(ComonpentFind(findtConfig));
            Debug.Log(sb.ToString());
            //显示提示信息
            CopyWord(sb.ToString());
            isCopyBoard3 = true;
            CloseBoard();
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        private static void AddPrefix(string prefix)
        {
            Object[] obj = Selection.objects;//获取到当前选择的物体
            foreach (var item in obj)
            {
                GameObject go = item as GameObject;

                if (go.name.StartsWith(prefix))
                    continue;

                go.name = $"{prefix}{go.name}";
            }
        }

        /// <summary>
        /// 删除前缀
        /// </summary>
        private static void RemovePrefix(string prefix)
        {
            Object[] obj = Selection.objects;//获取到当前选择的物体 
            foreach (var item in obj)
            {
                GameObject go = item as GameObject;
                if (go.name.Contains(prefix))
                    go.name = go.name.Replace(prefix, "");
            }
        }

        /// <summary>
        /// 去除空白
        /// </summary>
        private static void ClearTrim()
        {
            Object[] obj = Selection.objects;//获取到当前选择的物体 
            foreach (var item in obj)
                item.name = ClearSpecificSymbol(item.name);
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        private static void SaveModification()
        {
            Object[] obj = Selection.objects;
            for (int i = 0; i < obj.Length; i++)
            {
                Undo.RecordObject(obj[i], obj[i].name);
                EditorUtility.SetDirty(obj[i]);
            }
        }

        //******************************基础功能******************************
        /// <summary>
        /// 获取组件路径
        /// </summary>
        /// <param name="transformTF">需要获取路径的子物体</param>
        /// <param name="selectGoName">选择的父物体(transformTF要在这个父物体下)</param>
        /// <returns>返回的路径</returns>
        private static string GetTransformPath(Transform transformTF, string selectGoName)
        {
            //临时变量-存放路径
            List<string> strs = new List<string>();
            string path = string.Empty;
            //获取路径
            strs.Add(transformTF.name);
            while (transformTF.parent != null)
            {
                transformTF = transformTF.parent;
                if (transformTF.name == selectGoName) break;
                strs.Add(transformTF.name);
            }
            //转换成路径
            for (int j = strs.Count - 1; j >= 0; j--)
            {
                //if (j != 0)
                //    path += $"{strs[j]}/";
                //else
                //    path += $"{strs[j]}";
                path += j != 0 ? $"{strs[j]}/" : $"{strs[j]}";
            }
            return path;
        }

        /// <summary>
        /// 组件属性
        /// </summary>
        /// <param name="controlDic">物体的字典</param>
        /// <returns></returns>
        private static string ComonpentProperty(ACToolFindConfig findtConfig)
        {
            if (findtConfig.controlDic.Count == 0) return null;
            //生成组件属性
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 组件属性");
            foreach (string Key in findtConfig.controlDic.Keys)
            {
                findtConfig.controlDic[Key].ForEach((component) => //component每一个组件
                {
                    string componentName = ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                    string type = TypeChange(Key);//类型
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
        /// <param name="controlDic">物体的字典</param>
        /// <param name="selectGoName">选中的物体的名称</param>
        /// <returns></returns>
        private static string ComonpentFind(ACToolFindConfig findtConfig)
        {
            if (findtConfig.controlDic.Count == 0) return null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region 组件通过路径获取");
            sb.AppendLine("private void OnGetComponent()");
            sb.AppendLine("{");
            foreach (string Key in findtConfig.controlDic.Keys)
            {
                findtConfig.controlDic[Key].ForEach((component) =>
                {
                    string componentName = ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                    string type = TypeChange(Key);//类型
                    string path = GetTransformPath(component.transform, findtConfig.selectGoName);//组件的路径
                    string dot = string.IsNullOrEmpty(TransformGetPrefix) ? string.Empty : ".";//是否添加点
                    if (findtConfig.isAssign)
                        sb.AppendLine($"\t{componentName}{type} = {TransformGetPrefix}{dot}GetComponentInChildren<{type}>(\"{path}\");");
                    else
                        sb.AppendLine($"\t{TransformGetPrefix}{dot}GetComponentInChildren<{type}>(\"{path}\");");
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
        /// <param name="controlDic">物体的字典</param>
        /// <returns></returns>
        private static string ComonpentListener(Dictionary<string, List<Component>> controlDic)
        {
            if (controlDic.Count == 0) return null;
            StringBuilder sb = new StringBuilder();
            //按钮代码监听
            sb.AppendLine("#region 按钮监听");
            sb.AppendLine("private void OnAddListener()");
            sb.AppendLine("{");
            foreach (string Key in controlDic.Keys)
            {
                controlDic[Key].ForEach((component) =>
                {
                    string componentName = ClearSpecificSymbol(component.name);//组件名称,順便出去空白
                    string type = TypeChange(Key);//类型
                    switch (type)
                    {
                        case "Button":
                            sb.AppendLine($"\t{componentName}{type}.onClick.AddListener({componentName}AddListener);");
                            break;
                        case "Toggle":
                            sb.AppendLine($"\t{componentName}{type}.onValueChanged.AddListener({componentName}AddListener);");
                            break;
                    }
                });
            }
            sb.AppendLine("}");
            sb.AppendLine("#endregion");
            return sb.ToString();
        }

        //******************************其他******************************
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
        /// 将信息复制到剪切板当中 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void CopyWord(string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// 关闭提示版
        /// </summary>
        private static async void CloseBoard()
        {
            await Task.Delay(500);
            isCopyBoard3 = false;
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        public static string ClearSpecificSymbol(string str)
        {
            return str.Replace(" ", "").Replace("(", "").Replace(")", "").Trim();//组件名称,順便出去空白
        }
    }
}
