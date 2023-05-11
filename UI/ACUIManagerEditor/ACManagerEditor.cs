using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

/// <summary>
/// 修改ACManager样式
/// </summary>
[CustomEditor(typeof(ACUIManager), true)]//自定义ReferenceCollector类在界面中的显示与功能
public class ACManagerEditor : Editor
{
    private string ACUIData_key = "key";
    private string ACUIData_gameObject = "gameObject";
    private string ACUIManager_data = "data";

    private string Prefix = "T_";
    private string _searchKey = "";
    private Object heroPrefab;
    //输入在textfield中的字符串
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
        ButtonShow();
    }

    //https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
    //https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
    //https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html
    /// <summary>
    /// 显示按钮
    /// </summary>
    private void ButtonShow()
    {
        //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
        Undo.RecordObject(aCManager, "Changed Settings");
        var dataProperty = serializedObject.FindProperty(ACUIManager_data);//按名称查找序列化属性。
        //***********************************************************************************
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("去除空格"))
        {
            OnDelTrim();
        }
        if (GUILayout.Button($"添加{Prefix}前缀"))
        {
            OnAddPrefix(Prefix);
        }
        if (GUILayout.Button($"移除{Prefix}前缀"))
        {
            OnRemovePrefix(Prefix);
        }
        EditorGUILayout.EndHorizontal();
        //***********************************************************************************
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加物体"))
        {
            AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
        }
        if (GUILayout.Button("删除全部"))
        {
            aCManager.Clear();
        }
        if (GUILayout.Button($"获取{Prefix}物体"))
        {
            OoGetKeyGos(dataProperty);
        }
        if (GUILayout.Button("删除空引用"))
        {
            OnDelNullReference();
        }
        if (GUILayout.Button("排序"))
        {
            aCManager.Sort();
        }
        EditorGUILayout.EndHorizontal();
        //***********************************************************************************
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("获取脚本"))
        {
        }
        if (GUILayout.Button("删除脚本"))
        {
        }
        if (GUILayout.Button($"脚本"))
        {
        }
        if (GUILayout.Button("删除脚本"))
        {
        }
        if (GUILayout.Button("脚本"))
        {
        }
        EditorGUILayout.EndHorizontal();
        //***********************************************************************************
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
        //***********************************************************************************
        EditorGUILayout.Space(10);
        //***********************************************************************************
        SerializedProperty property;
        for (int i = aCManager.data.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_key);// GetArrayElementAtIndex 返回数组中指定索引处的元素。
            property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(100));
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);
            property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);
            if (GUILayout.Button("删除"))
            {
                //aCManager.data.Remove(aCManager.data[i]);
                dataProperty.DeleteArrayElementAtIndex(i);
            }
            if (GUILayout.Button("复制名称"))
            {
                TextEditor te = new TextEditor();
                te.text = aCManager.data[i].key.ToString();
                te.SelectAll();
                te.Copy();
            }
            EditorGUILayout.EndHorizontal();
        }
        //***********************************************************************************
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
        //***********************************************************************************
        //遍历删除list，将其删除掉
        serializedObject.ApplyModifiedProperties();//应用属性修改。
        serializedObject.UpdateIfRequiredOrScript();//更新序列化对象的表示形式
    }

    /// <summary>
    /// 添加元素，具体知识点在ReferenceCollector中说了
    /// </summary>
    /// <param name="dataProperty"></param>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    private void AddReference(SerializedProperty dataProperty, string key, Object obj)
    {
        int index = dataProperty.arraySize;
        dataProperty.InsertArrayElementAtIndex(index);
        var element = dataProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative(ACUIData_key).stringValue = key;
        element.FindPropertyRelative(ACUIData_gameObject).objectReferenceValue = obj;
    }

    /// <summary>
    /// 去除空白
    /// </summary>
    private void OnDelTrim()
    {
        Array.ForEach(Selection.gameObjects, (go) =>
        {
            go.name = go?.name.Trim().Replace(" ", "");
        });
    }

    /// <summary>
    /// 添加前缀
    /// </summary>
    private void OnAddPrefix(string Prefix)
    {
        Array.ForEach(Selection.gameObjects, (go) =>
        {
            go.name = $"{Prefix}{go.name}";
        });
    }

    /// <summary>
    /// 移除前缀
    /// </summary>
    private void OnRemovePrefix(string Prefix)
    {
        Array.ForEach(Selection.gameObjects, (go) =>
        {
            go.name = go.name.Replace(Prefix, "");
        });
    }

    /// <summary>
    /// 获取关键字开头的物体
    /// </summary>
    private void OoGetKeyGos(SerializedProperty dataProperty)
    {
        //获取物体
        List<GameObject> gameObjects = new List<GameObject>();
        ACLoopGetKeywordGO(aCManager.gameObject.transform, Prefix, ref gameObjects);
        for (int i = 0; i < gameObjects?.Count; i++)
            AddReference(dataProperty, gameObjects[i].name, gameObjects[i]);
    }

    /// <summary>
    /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="keyValue">关键词</param>
    /// <param name="goList"></param>
    public void ACLoopGetKeywordGO(Transform transform, string keyValue, ref List<GameObject> goList)
    {
        if (transform.name.StartsWith(keyValue))
            goList.Add(transform.gameObject);
        for (int i = 0; i < transform?.childCount; i++)
            ACLoopGetKeywordGO(transform.GetChild(i), keyValue, ref goList);
    }

    /// <summary>
    /// 删除空引用
    /// </summary>
    private void OnDelNullReference()
    {
        var dataProperty = serializedObject.FindProperty(ACUIManager_data);
        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);
            if (gameObjectProperty.objectReferenceValue == null)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
                EditorUtility.SetDirty(aCManager);
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfRequiredOrScript();
            }
        }
    }
}