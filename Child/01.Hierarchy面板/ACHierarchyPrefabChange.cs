using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace ACTool
{
    public static class ACHierarchyPrefabChange
    {
        //***************************替换物体***************************
        private static Vector2 ACHierarchyTool_ScrollRoot { get; set; }
        private static GameObject newPrefab { get; set; }//新的物体
        private static GameObject tonewPrefab { get; set; }//预制体
        private static string replaceName { get; set; } = String.Empty;//重命名

        /// <summary>
        /// 转换物体
        /// </summary>
        public static void OnShow()
        {
            ACHierarchyTool_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyTool_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Hierarchy替换物体", EditorStyles.boldLabel);
                    //******************************前缀******************************
                    EditorGUILayout.BeginVertical("box");
                    {
                        //******************************一键替换场景中的物体******************************
                        GUILayout.Space(5f); EditorGUILayout.LabelField("选择一个新的物体");
                        newPrefab = (GameObject)EditorGUILayout.ObjectField(newPrefab, typeof(GameObject), true, GUILayout.MinWidth(100f));
                        tonewPrefab = newPrefab;
                        if (GUILayout.Button("替换选中的物体")) { ReplaceObjects(); }
                        replaceName = EditorGUILayout.TextField("需要替换的物体名字", replaceName);
                        if (GUILayout.Button("替换相同名字的")) { ReplaceObjectsByName(replaceName, false); }
                        if (GUILayout.Button("替换包含名字的 慎用")) { ReplaceObjectsByName(replaceName, true); }
                        if (GUILayout.Button("保存修改")) { EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()); }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// 替换物体
        /// </summary>
        public static void ReplaceObjects()
        {
            if (tonewPrefab == null) return;
            UnityEngine.Object[] objects = Selection.objects;
            List<ReplaceData> replaceDatas = new List<ReplaceData>();
            foreach (UnityEngine.Object item in objects)
            {
                GameObject temp = (GameObject)item;
                ReplaceData replaceData = new ReplaceData();
                replaceData.old = temp;
                ReplaceOne(replaceData);
                replaceDatas.Add(replaceData);
            }
            HandleReplaceData(replaceDatas);
        }

        /// <summary>
        /// 根据名称替换
        /// </summary>
        /// <param name="name">物体名称</param>
        /// <param name="isContain">是否包含</param>
        static void ReplaceObjectsByName(string name, bool isContain)
        {
            if (string.IsNullOrEmpty(name)) return;//名称不为空
            List<ReplaceData> replaceDatas = new List<ReplaceData>();//新的列表

            //选中的物体
            UnityEngine.Object[] objects = Selection.objects;
            List<Transform> all = new List<Transform>();

            foreach (var item in objects)
            {
                GameObject go = (GameObject)item;
                Transform transform = go.GetComponent<Transform>();
                transform.ACLoopGetAllTransform(ref all);
            }

            foreach (var item in all)
            {
                //Debug.LogError(item.name);
                ReplaceData replaceData = new ReplaceData();
                replaceData.old = item.gameObject;
                if (!isContain && item.gameObject.name == name)
                {
                    ReplaceOne(replaceData);
                    replaceDatas.Add(replaceData);
                }
                else if (isContain && item.gameObject.name.Contains(name))
                {
                    ReplaceOne(replaceData);
                    replaceDatas.Add(replaceData);
                }
            }
            HandleReplaceData(replaceDatas);
        }

        /// <summary>
        /// 替换一个
        /// </summary>
        /// <param name="replaceData"></param>
        public static void ReplaceOne(ReplaceData replaceData)
        {
            GameObject replace = (GameObject)PrefabUtility.InstantiatePrefab(tonewPrefab);
            replace.transform.SetParent(replaceData.old.transform.parent);
            replace.transform.localPosition = replaceData.old.transform.localPosition;
            replace.transform.localRotation = replaceData.old.transform.localRotation;
            replace.transform.localScale = replaceData.old.transform.localScale;
            replaceData.replace = replace;
            replaceData.index = replaceData.old.transform.GetSiblingIndex();
        }

        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="replaceDatas"></param>
        public static void HandleReplaceData(List<ReplaceData> replaceDatas)
        {
            foreach (var replaceData in replaceDatas)
            {
                replaceData.replace.transform.SetSiblingIndex(replaceData.index);
                if (null != replaceData.old && null != replaceData.old.gameObject)
                    UnityEngine.Object.DestroyImmediate(replaceData.old.gameObject);
            }
        }

        /// <summary>
        /// 替换的数据
        /// </summary>
        public class ReplaceData
        {
            public GameObject old;
            public GameObject replace;
            public int index = 0;
        }

    }
}
