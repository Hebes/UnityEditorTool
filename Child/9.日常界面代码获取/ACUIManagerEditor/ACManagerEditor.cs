using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    修改ACManager样式

    SerializedProperty  以完全通用的方式编辑对象上的属性（可自动处理撤销），同时还能调整预制件的 UI 样式
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
    EditorGUILayout  EditorGUI 的自动布局版本
    https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
    SerializedObject     以完全通用的方式编辑 Unity 对象上的可序列化字段。
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html

-----------------------*/

namespace ACTool
{
    /// <summary> 自定义ReferenceCollector类在界面中的显示与功能 </summary>
    [CustomEditor(typeof(ACUIManager), true)]
    public class ACManagerEditor : Editor
    {
        private string ACUIData_key = "key";
        private string ACUIData_gameObject = "gameObject";
        private string ACUIManager_data = "data";

        private string Prefix = "T_";
        private string _searchKey = "";
        private Object heroPrefab;

        /// <summary> 输入在textfield中的字符串 </summary>
        private string searchKey
        {
            get
            {
                return _searchKey;
            }
            set
            {
                if (_searchKey != value)
                {
                    _searchKey = value;
                    heroPrefab = aCManager.Get<Object>(searchKey);
                }
            }
        }
        private ACUIManager aCManager { get; set; }
        public string ETUITool_ClassName { get; set; }
        public int toolbarid { get; set; }
        public string ETUITool_ClassName1 { get; private set; }


        /// <summary> 组件列表 </summary>
        public List<string> components = new List<string>()
    {
         typeof(Button).Name,
         typeof(Text).Name,
         typeof(Image).Name,
         typeof(GameObject).Name,
         typeof(InputField).Name,
    };


        private void OnEnable()
        {
            Debug.Log("面板启动了");
            aCManager = (ACUIManager)target;
        }
        private void OnDestroy()
        {
            Debug.Log("面板关闭了");
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty dataProperty = OnUIGetSerializedProperty();
            EditorGUILayout.LabelField("前缀按钮", EditorStyles.boldLabel);
            OnUIPrefixButton(dataProperty);
            EditorGUILayout.LabelField("获取物体", EditorStyles.boldLabel);
            OnUIGetGameObject(dataProperty);
            EditorGUILayout.LabelField("获取脚本", EditorStyles.boldLabel);
            OnUIGetScript(dataProperty);
            EditorGUILayout.LabelField("内容", EditorStyles.boldLabel);
            OnUIContent1(dataProperty);
            EditorGUILayout.Space(10);
            OnUIContent2(dataProperty);
            EditorGUILayout.Space(10);
            OnUIDrog(dataProperty);
            OnUIUpdata();
        }

        /// <summary> 获取属性 </summary>
        private SerializedProperty OnUIGetSerializedProperty()
        {
            //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
            Undo.RecordObject(aCManager, "Changed Settings");
            var dataProperty = serializedObject.FindProperty(ACUIManager_data);//按名称查找序列化属性。
            return dataProperty;
        }
        /// <summary> 前缀按钮 </summary>
        private void OnUIPrefixButton(SerializedProperty dataProperty)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"添加{Prefix}"))
                OnAddPrefix(dataProperty, Prefix);
            if (GUILayout.Button($"移除{Prefix}"))
                OnRemovePrefix(dataProperty, Prefix);
            if (GUILayout.Button("去除空格"))
                OnDelTrim(dataProperty);
            EditorGUILayout.EndHorizontal();
        }
        /// <summary> 获取物体 </summary>
        private void OnUIGetGameObject(SerializedProperty dataProperty)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加物体"))
                AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
            if (GUILayout.Button("删除全部"))
                aCManager.Clear();
            if (GUILayout.Button($"获取{Prefix}物体"))
                OoGetKeyGos(dataProperty);
            if (GUILayout.Button("删除空引用"))
                OnDelNullReference();
            if (GUILayout.Button("排序"))
                aCManager.Sort();
            EditorGUILayout.EndHorizontal();
        }
        /// <summary> 内容 </summary>
        private void OnUIContent1(SerializedProperty dataProperty)
        {
            EditorGUILayout.BeginHorizontal();
            //可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
            searchKey = EditorGUILayout.TextField(searchKey);
            //添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
            //第三个参数为是否只能引用scene中的Object
            EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
            if (GUILayout.Button("删除"))
            {
                aCManager.Remove(searchKey);
                heroPrefab = null;
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        /// <summary> 内容二 </summary>
        private void OnUIContent2(SerializedProperty dataProperty)
        {
            if (dataProperty.arraySize == 0) return;
            SerializedProperty property;
            for (int i = aCManager.data.Count - 1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_key);
                property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(200));
                property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);
                if (GUILayout.Button("删除"))
                    dataProperty.DeleteArrayElementAtIndex(i);
                if (GUILayout.Button("复制名称"))
                    Copy(aCManager.data[i].key);
                EditorGUILayout.EndHorizontal();
            }
        }
        /// <summary> 拖拽 </summary>
        private void OnUIDrog(SerializedProperty dataProperty)
        {
            var eventType = Event.current.type;
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                // Show a copy icon on the drag
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();//接受拖动操作。
                    foreach (var o in DragAndDrop.objectReferences)
                        AddReference(dataProperty, o.name, o);
                }
                Event.current.Use();
            }
        }
        /// <summary> 更新 </summary>
        private void OnUIUpdata()
        {
            serializedObject.ApplyModifiedProperties();//应用属性修改。
            serializedObject.UpdateIfRequiredOrScript();//更新序列化对象的表示形式
        }
        /// <summary> 获取脚本按钮组 </summary>
        private void OnUIGetScript(SerializedProperty dataProperty)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("获取脚本必要代码"))
                OnGetEssentialCode();
            if (GUILayout.Button("获取脚本"))
                OnGetCode();
            EditorGUILayout.EndHorizontal();
            //***********************************************************************************
            toolbarid = GUILayout.SelectionGrid(toolbarid, components.ToArray(), 4);
            ETUITool_ClassName = EditorGUILayout.TextField("组件类型", components[toolbarid]);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("变量获取"))
                OnGetValue(dataProperty, ETUITool_ClassName);
            if (GUILayout.Button("组件获取"))
                OnGetButtonCode(dataProperty, ETUITool_ClassName);
            if (GUILayout.Button("常用方法获取"))
                OnCreatCommonGetCode(dataProperty, ETUITool_ClassName);
            EditorGUILayout.EndHorizontal();
            //***********************************************************************************
            EditorGUILayout.BeginHorizontal();
            ETUITool_ClassName1 = EditorGUILayout.TextField("组件类型", ETUITool_ClassName1);
            if (GUILayout.Button("清空"))
                ETUITool_ClassName1 = string.Empty;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("常用方法生成"))
                OnCreatCommonCreatCode(dataProperty, ETUITool_ClassName);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary> 添加元素 </summary>
        private void AddReference(SerializedProperty dataProperty, string key, Object obj)
        {
            int index = dataProperty.arraySize;
            dataProperty.InsertArrayElementAtIndex(index);
            var element = dataProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(ACUIData_key).stringValue = key;
            element.FindPropertyRelative(ACUIData_gameObject).objectReferenceValue = obj;
        }
        /// <summary> 修改引用名称 </summary>
        private void ReplaceReferenceName(SerializedProperty dataProperty, string key)
        {
            Debug.Log("走的序列化的修改并设置");
            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty serializedProperty = dataProperty.GetArrayElementAtIndex(i);
                var keyProperty = serializedProperty.FindPropertyRelative(ACUIData_key);//物体名称
                var gameObjectProperty = serializedProperty.FindPropertyRelative(ACUIData_gameObject);//查找物体
                if (gameObjectProperty.objectReferenceValue != null)
                {
                    switch (key)
                    {
                        case "Add"://设置名称
                            gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                                $"{Prefix}{gameObjectProperty.objectReferenceValue.name}";
                            break;
                        case "Remove"://设置名称
                            gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                                gameObjectProperty.objectReferenceValue.name.Replace(Prefix, "");
                            break;
                        case "Trim"://设置名称
                            gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                                gameObjectProperty.objectReferenceValue.name.Trim().Replace(" ", "");
                            break;
                    }
                }
            }
            //https://docs.unity.cn/cn/2019.4/ScriptReference/EditorUtility.SetDirty.html
            EditorUtility.SetDirty(aCManager);
            OnUIUpdata();
        }
        /// <summary> 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板) </summary>
        public void ACLoopGetKeywordGO(Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                ACLoopGetKeywordGO(transform.GetChild(i), keyValue, ref goList);
        }


        /// <summary> 去除空白 </summary>
        private void OnDelTrim(SerializedProperty dataProperty)
        {
            if (dataProperty.arraySize == 0)
                Array.ForEach(Selection.gameObjects, (go) => { go.name = go?.name.Trim().Replace(" ", ""); });
            else
                ReplaceReferenceName(dataProperty, "Trim");
        }
        /// <summary> 添加前缀 </summary>
        private void OnAddPrefix(SerializedProperty dataProperty, string Prefix)
        {
            if (dataProperty.arraySize == 0)
                Array.ForEach(Selection.gameObjects, (go) => { go.name = $"{Prefix}{go.name}"; });
            else
                ReplaceReferenceName(dataProperty, "Add");
        }
        /// <summary> 移除前缀 </summary>
        private void OnRemovePrefix(SerializedProperty dataProperty, string Prefix)
        {
            if (dataProperty.arraySize == 0)
                Array.ForEach(Selection.gameObjects, (go) => { go.name = go.name.Replace(Prefix, ""); });
            else
                ReplaceReferenceName(dataProperty, "Remove");
        }
        /// <summary> 反射命名空间 </summary>
        private Type ACReflectClass(string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }
        /// <summary> 复制 </summary>
        private void Copy(string content)
        {
            TextEditor te = new TextEditor();
            te.text = content.ToString();
            te.SelectAll();
            te.Copy();
        }


        /// <summary> 删除空引用 </summary>
        private void OnDelNullReference()
        {
            var dataProperty = serializedObject.FindProperty(ACUIManager_data);
            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
                if (gameObjectProperty.objectReferenceValue == null)
                {
                    dataProperty.DeleteArrayElementAtIndex(i);
                    EditorUtility.SetDirty(aCManager);
                    OnUIUpdata();
                }
            }
        }
        /// <summary> 获取关键字开头的物体 </summary>
        private void OoGetKeyGos(SerializedProperty dataProperty)
        {
            if (dataProperty.arraySize >= 0)
            {
                Debug.Log("有数据先清理");
                //根据PropertyPath读取prefab文件中的数据
                //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
                dataProperty.ClearArray();
                UnityEditor.EditorUtility.SetDirty(this);
                OnUIUpdata();
            }

            //获取物体
            List<GameObject> gameObjects = new List<GameObject>();
            ACLoopGetKeywordGO(aCManager.gameObject.transform, Prefix, ref gameObjects);
            for (int i = 0; i < gameObjects?.Count; i++)
                AddReference(dataProperty, gameObjects[i].name, gameObjects[i]);
        }


        /// <summary> 获取脚本必要代码 请自行填写 </summary>
        private void OnGetEssentialCode()
        {
            StringBuilder sb = new StringBuilder();
            //自己填写需要的代码
            sb.AppendLine($"aCUIManager = GetComponent<ACUIManager>();");
            Debug.Log(sb.ToString());
            Copy(sb.ToString());

        }
        /// <summary> 获取脚本 请自行填写 </summary>
        private void OnGetCode()
        {
            Debug.Log("暂时没写");
        }
        /// <summary> 获取变量 请自行填写 </summary>
        private void OnGetValue(SerializedProperty dataProperty, string eTUITool_ClassName)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
                Type type = ACReflectClass(eTUITool_ClassName);
                if (type == null) type = ACReflectClass(eTUITool_ClassName, "UnityEngine");

                if (type == typeof(GameObject))
                {
                    sb.AppendLine($"public {eTUITool_ClassName} {gameObjectProperty.objectReferenceValue.name} {{get;set;}}");
                }
                else
                {
                    Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
                    if (component != null)
                        sb.AppendLine($"public {eTUITool_ClassName} {gameObjectProperty.objectReferenceValue.name}{eTUITool_ClassName} {{get;set;}}");
                }
            }

            Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }
        /// <summary> 获取Button脚本 </summary>
        private void OnGetButtonCode(SerializedProperty dataProperty, string eTUITool_ClassName)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
                Type type = ACReflectClass(eTUITool_ClassName);
                if (type == null) type = ACReflectClass(eTUITool_ClassName, "UnityEngine");
                //if (type == typeof(GameObject))
                //{
                //    sb.AppendLine($"self.{gameObjectProperty.objectReferenceValue.name} = aCUIManager.GetComponent<{eTUITool_ClassName}>(\"{gameObjectProperty.objectReferenceValue.name}\");");
                //}
                //else
                //{
                //    Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
                //    if (component != null)
                //        sb.AppendLine($"{gameObjectProperty.objectReferenceValue.name}{eTUITool_ClassName} = aCUIManager.GetComponent<{eTUITool_ClassName}>(\"{gameObjectProperty.objectReferenceValue.name}\");");
                //}

                if (type == typeof(GameObject))
                {
                    sb.AppendLine($"self.{gameObjectProperty.objectReferenceValue.name} = rc.Get<{eTUITool_ClassName}>(\"{gameObjectProperty.objectReferenceValue.name}\");");
                }
                else
                {
                    Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
                    if (component != null)
                        sb.AppendLine($"{gameObjectProperty.objectReferenceValue.name} = rc.Get<{eTUITool_ClassName}>(\"{gameObjectProperty.objectReferenceValue.name}\");");
                }
            }

            Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }

        /// <summary> 常用获取 </summary>
        private void OnCreatCommonGetCode(SerializedProperty dataProperty, string eTUITool_ClassName)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
                Type type = ACReflectClass(eTUITool_ClassName);
                if (type == null) type = ACReflectClass(eTUITool_ClassName, "UnityEngine");
                if (type == typeof(GameObject))
                {
                    Debug.Log("自行填写");
                }
                else if (type == typeof(Button))
                {
                    //self.T_Close.GetComponent<Button>().onClick.AddListener(self.OnT_Close);
                    Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
                    if (component != null)
                        sb.AppendLine($"self.{gameObjectProperty.objectReferenceValue.name}.GetComponent<{eTUITool_ClassName}>().onClick.AddListener(self.On{gameObjectProperty.objectReferenceValue.name});");
                }
                else if (type == typeof(Text))
                {
                    //self.T_Close.GetComponent<Button>().onClick.AddListener(self.OnT_Close);
                    Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
                    if (component != null)
                        sb.AppendLine($"self.{gameObjectProperty.objectReferenceValue.name}.GetComponent<{eTUITool_ClassName}>().text =;");
                }
            }
            Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }
        /// <summary> 常用方法生成 </summary>
        private void OnCreatCommonCreatCode(SerializedProperty dataProperty, string eTUITool_ClassName)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
                GameObject go = gameObjectProperty.objectReferenceValue as GameObject;
                Type type = ACReflectClass(eTUITool_ClassName);
                if (type == typeof(Button))
                {
                    Component component = go.GetComponent(type);
                    if (component != null)
                    {
                        sb.AppendLine($"/// <summary>  </summary>");
                        sb.AppendLine($"public static void On{gameObjectProperty.objectReferenceValue.name}(this {ETUITool_ClassName1} self)\t\n{{\t\n}}");
                    }
                }
            }
            Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }
    }
}
