using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

namespace ACTool
{

    //使其能在Inspector面板显示，并且可以被赋予相应值
    [Serializable]
    public class ACUIData
    {
        public string key;
        public Object gameObject;
        //public Component DataComponent;
    }

    //继承IComparer对比器，Ordinal会使用序号排序规则比较字符串，因为是byte级别的比较，所以准确性和性能都不错
    public class ACUIDataComparer : IComparer<ACUIData>
    {
        public int Compare(ACUIData x, ACUIData y)
        {
            return string.Compare(x.key, y.key, StringComparison.Ordinal);
        }
    }

    public class ACUIManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ACUIData> data = new List<ACUIData>();
        private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();//Object并非C#基础中的Object，而是 UnityEngine.Object

#if UNITY_EDITOR
        public void Add(string key, Object obj)
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            //根据PropertyPath读取数据
            //如果不知道具体的格式，可以右键用文本编辑器打开一个prefab文件（如Bundles/UI目录中的几个）
            //因为这几个prefab挂载了ReferenceCollector，所以搜索data就能找到存储的数据
            UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
            int i;
            //遍历data，看添加的数据是否存在相同key
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            //不等于data.Count意为已经存在于data List中，直接赋值即可
            if (i != data.Count)
            {
                //根据i的值获取dataProperty，也就是data中的对应ReferenceCollectorData，不过在这里，是对Property进行的读取，有点类似json或者xml的节点
                UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
                //对对应节点进行赋值，值为gameobject相对应的fileID
                //fileID独一无二，单对单关系，其他挂载在这个gameobject上的script或组件会保存相对应的fileID
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            else
            {
                //等于则说明key在data中无对应元素，所以得向其插入新的元素
                dataProperty.InsertArrayElementAtIndex(i);
                UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("key").stringValue = key;
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            //应用与更新
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }//添加新的元素
        public void Remove(string key)
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (data[i].key == key)
                {
                    break;
                }
            }
            if (i != data.Count)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }//删除元素，知识点与上面的添加相似
        public void Clear()
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            //根据PropertyPath读取prefab文件中的数据
            //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
            var dataProperty = serializedObject.FindProperty("data");
            dataProperty.ClearArray();
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
        public void Sort()
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            data.Sort(new ACUIDataComparer());
            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
#endif
        public T Get<T>(string key) where T : class
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return dictGo as T;
        }
        public T GetComponent<T>(string key) where T : Component
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return (dictGo as GameObject).GetComponent<T>();
        }
        public Object GetObject(string key)
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo)) return null;
            return dictGo;
        }

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            Debug.Log("执行了反序列化");
            dict.Clear();
            foreach (ACUIData referenceCollectorData in data)
            {
                if (!dict.ContainsKey(referenceCollectorData.key))
                {
                    dict.Add(referenceCollectorData.key, referenceCollectorData.gameObject);
                }
            }
        }//在反序列化后运行
    }
}