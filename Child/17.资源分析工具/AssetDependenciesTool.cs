




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
namespace ACTool
{
    public class AssetDependenciesTool : EditorWindow
    {
        //--- 固定数据 ---

        private readonly Color listItemColor = new Color(0.9f, 0.9f, 0.9f, 1);
        private readonly string[] displayBarArray = new string[] { "查找引用方", "查找被引用方", "引用关系", "被引用关系" };

        //--- 初始数据 ---

        //Prefab/Scene的引用资源
        private Dictionary<string, string[]> prefabDependDic = new Dictionary<string, string[]>();
        //被Prefab/Scene引用的资源
        private Dictionary<string, string[]> assetRelatedDic = new Dictionary<string, string[]>();

        //Prefab/Scene的名字
        private string[] prefabNameArray;
        //资源类型、名字
        private string[] assetExtentArray;
        private Dictionary<string, string[]> assetExtentNameDic;

        //--- 变化数据 ---

        //====================

        private static bool recursiveFind = false;

        [MenuItem("MyTools/资源引用关系/查看直接引用")]
        public static void FindAssetsDependencies1()
        {
            recursiveFind = false;
            GetWindow<AssetDependenciesTool>(true, "查找资源引用关系-直接引用");
        }
        [MenuItem("MyTools/资源引用关系/查看递归引用")]
        public static void FindAssetsDependencies2()
        {
            recursiveFind = true;
            GetWindow<AssetDependenciesTool>(true, "查找资源引用关系-递归引用");
        }

        private void OnEnable()
        {
            FindAllPrefabDependencies(ref prefabDependDic, ref assetRelatedDic, out prefabNameArray, out assetExtentArray, out assetExtentNameDic);
        }

        private int displayIndex = 0;
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("资源引用方数量 : " + prefabDependDic.Count);
            EditorGUILayout.LabelField("资源被引用方数量 : " + assetRelatedDic.Count);
            EditorGUILayout.LabelField("所统计的资源引用关系，不包含代码动态加载");
            EditorGUILayout.EndVertical();

            displayIndex = GUILayout.Toolbar(displayIndex, displayBarArray);

            switch (displayIndex)
            {
                case 0:
                    OnGUI_FindPrefab();
                    break;
                case 1:
                    OnGUI_FindAssets();
                    break;
                case 2:
                    OnGUI_AllPrefab();
                    break;
                case 3:
                    OnGUI_AllAssets();
                    break;
                default:
                    break;
            }
        }

        //查找某个Prefab/Scene引用的所有资源
        Object selectPrefabObject;
        PrefabDependItem selectPrefabDependItem;
        private void OnGUI_FindPrefab()
        {
            EditorGUILayout.BeginHorizontal();
            selectPrefabObject = EditorGUILayout.ObjectField("引用资源:", selectPrefabObject, typeof(Object), true, GUILayout.Width(600));

            if (GUILayout.Button("分析引用详情", GUILayout.Width(100)))
            {
                if (selectPrefabObject != null)
                {
                    string prefabPath = AssetDatabase.GetAssetPath(selectPrefabObject);
                    prefabDependDic.TryGetValue(prefabPath, out var depPathArray);
                    selectPrefabDependItem = new PrefabDependItem(prefabPath, depPathArray);
                }
            }
            EditorGUILayout.EndHorizontal();

            OnGUI_PrefabDetial(ref selectPrefabDependItem);
        }

        //查找某个资源被哪些Prefab/Scene引用
        Object selectAssetObject;
        AssetRelatedItem selectAssetRelatedItem;
        private void OnGUI_FindAssets()
        {
            EditorGUILayout.BeginHorizontal();
            selectAssetObject = EditorGUILayout.ObjectField("被引用资源:", selectAssetObject, typeof(Object), true, GUILayout.Width(600));

            if (GUILayout.Button("分析被引用详情", GUILayout.Width(100)))
            {
                if (selectAssetObject != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(selectAssetObject);
                    assetRelatedDic.TryGetValue(assetPath, out var depPathArray);
                    selectAssetRelatedItem = new AssetRelatedItem(assetPath, depPathArray);
                }
            }
            EditorGUILayout.EndHorizontal();

            OnGUI_AssetDetial(ref selectAssetRelatedItem);
        }

        int showPageIndex1 = 0;
        string searchPrefabNameSign;
        bool isSearchPrefabName;
        Vector2 scrollViewPos03;
        Vector2 scrollViewPos031;
        PrefabDependItem previewPrefabDependItem;
        private void OnGUI_AllPrefab()
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("搜索关键字（仅搜索当前页！！！）:", GUILayout.Width(200));
            searchPrefabNameSign = EditorGUILayout.TextField(searchPrefabNameSign, GUILayout.Width(200));
            isSearchPrefabName = !string.IsNullOrEmpty(searchPrefabNameSign);
            EditorGUILayout.EndHorizontal();

            scrollViewPos03 = EditorGUILayout.BeginScrollView(scrollViewPos03);
            for (int i = showPageIndex1; i < showPageIndex1 + 100; i++)
            {
                if (i >= prefabNameArray.Length)
                    break;

                string prefabPath = prefabNameArray[i];

                if (isSearchPrefabName && !prefabPath.Contains(searchPrefabNameSign))
                    continue;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField($"{i}. 资源文件名: {prefabPath}");
                EditorGUILayout.LabelField($"引用数量: {prefabDependDic[prefabPath].Length}", GUILayout.Width(200));
                if (GUILayout.Button("查看引用详情", GUILayout.Width(100)))
                {
                    prefabDependDic.TryGetValue(prefabPath, out var depPathArray);
                    previewPrefabDependItem = new PrefabDependItem(prefabPath, depPathArray);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            OnGUI_PrefabDetial(ref previewPrefabDependItem);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("首页", GUILayout.Width(100)))
            {
                showPageIndex1 = 0;
            }
            if (GUILayout.Button("上一页", GUILayout.Width(100)))
            {
                showPageIndex1 -= 100;
                if (showPageIndex1 < 0)
                    showPageIndex1 = 0;
            }
            EditorGUILayout.LabelField($"{showPageIndex1}/{prefabNameArray.Length}", GUILayout.Width(100));
            if (GUILayout.Button("下一页", GUILayout.Width(100)))
            {
                if ((showPageIndex1 + 100) < prefabNameArray.Length)
                    showPageIndex1 += 100;
            }
            if (GUILayout.Button("尾页", GUILayout.Width(100)))
            {
                showPageIndex1 = prefabNameArray.Length / 100 * 100;
            }
            EditorGUILayout.EndHorizontal();
        }

        int showPageIndex2 = 0;
        int selectExtentIndex = -1;
        string searchAssetNameSign;
        bool isSearchAssetName;
        Vector2 scrollViewPos04;
        Vector2 scrollViewPos041;
        AssetRelatedItem previewAssetRelatedItem;
        private void OnGUI_AllAssets()
        {
            selectExtentIndex = EditorGUILayout.Popup("查找资源类型 : ", selectExtentIndex, assetExtentArray, GUILayout.MaxWidth(300));
            if (selectExtentIndex < 0 || selectExtentIndex >= assetExtentArray.Length)
                return;

            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("搜索关键字（仅搜索当前页！！！）:", GUILayout.Width(200));
            searchAssetNameSign = EditorGUILayout.TextField(searchAssetNameSign, GUILayout.Width(200));
            isSearchAssetName = !string.IsNullOrEmpty(searchAssetNameSign);
            EditorGUILayout.EndHorizontal();

            scrollViewPos04 = EditorGUILayout.BeginScrollView(scrollViewPos04);

            var nameArray = assetExtentNameDic[assetExtentArray[selectExtentIndex]];

            for (int i = showPageIndex2; i < showPageIndex2 + 100; i++)
            {
                if (i >= nameArray.Length)
                    break;

                string assetPath = nameArray[i];

                if (isSearchAssetName && !assetPath.Contains(searchAssetNameSign))
                    continue;

                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField($"{i}. 资源文件名: {assetPath}");
                EditorGUILayout.LabelField($"被引用次数: {assetRelatedDic[assetPath].Length}", GUILayout.Width(200));
                if (GUILayout.Button("查看引用详情", GUILayout.Width(100)))
                {
                    assetRelatedDic.TryGetValue(assetPath, out var depPathArray);
                    previewAssetRelatedItem = new AssetRelatedItem(assetPath, depPathArray);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            OnGUI_AssetDetial(ref previewAssetRelatedItem);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("首页", GUILayout.Width(100)))
            {
                showPageIndex2 = 0;
            }
            if (GUILayout.Button("上一页", GUILayout.Width(100)))
            {
                showPageIndex2 -= 100;
                if (showPageIndex2 < 0)
                    showPageIndex2 = 0;
            }
            EditorGUILayout.LabelField($"{showPageIndex2}/{nameArray.Length}", GUILayout.Width(100));
            if (GUILayout.Button("下一页", GUILayout.Width(100)))
            {
                if ((showPageIndex2 + 100) < nameArray.Length)
                    showPageIndex2 += 100;
            }
            if (GUILayout.Button("尾页", GUILayout.Width(100)))
            {
                showPageIndex2 = nameArray.Length / 100 * 100;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnGUI_PrefabDetial(ref PrefabDependItem prefabDependItem)
        {
            EditorGUILayout.Space(5);
            if (prefabDependItem != null)
            {
                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button("关闭引用详情", GUILayout.Width(100)))
                {
                    prefabDependItem = null;
                }
                EditorGUILayout.Space(2);

                scrollViewPos031 = EditorGUILayout.BeginScrollView(scrollViewPos031, GUILayout.Height(500));

                EditorGUILayout.ObjectField("引用方:", prefabDependItem.prefabObject, typeof(Object), true, GUILayout.Width(400));
                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("被引用方列表:");
                for (int i = 0; i < prefabDependItem.dependObjectArray.Length; i++)
                {
                    EditorGUILayout.Space(2);
                    EditorGUILayout.ObjectField($"({i}) 被引用方:", prefabDependItem.dependObjectArray[i], typeof(Object), true, GUILayout.Width(400));
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space(5);
        }

        private void OnGUI_AssetDetial(ref AssetRelatedItem assetRelatedItem)
        {
            EditorGUILayout.Space(5);
            if (assetRelatedItem != null)
            {
                EditorGUILayout.BeginVertical("box");
                if (GUILayout.Button("关闭引用详情", GUILayout.Width(100)))
                {
                    assetRelatedItem = null;
                }
                EditorGUILayout.Space(2);

                scrollViewPos041 = EditorGUILayout.BeginScrollView(scrollViewPos041, GUILayout.Height(500));

                EditorGUILayout.ObjectField("被引用方:", assetRelatedItem.assetObject, typeof(Object), true, GUILayout.Width(400));
                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("引用方列表:");
                for (int i = 0; i < assetRelatedItem.relatedObjectArray.Length; i++)
                {
                    EditorGUILayout.Space(2);
                    EditorGUILayout.ObjectField($"({i}) 引用方:", assetRelatedItem.relatedObjectArray[i], typeof(Object), true, GUILayout.Width(400));
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.Space(5);
        }

        //==========

        //准备初始数据
        private void FindAllPrefabDependencies(ref Dictionary<string, string[]> _prefabDependDic, ref Dictionary<string, string[]> _assetRelatedDic, out string[] _prefabNameArray, out string[] _assetExtentArray, out Dictionary<string, string[]> _assetExtentNameDic)
        {
            EditorUtility.DisplayProgressBar("读取项目所有资源", "读取所有资源", 0);

            //string[] allFilePathArray = Directory.GetFiles(Application.dataPath,"*.*",SearchOption.AllDirectories);
            string[] allFilePathArray = AssetDatabase.GetAllAssetPaths();

            EditorUtility.ClearProgressBar();

            //----------

            _prefabDependDic.Clear();

            EditorUtility.DisplayProgressBar("获取资源引用关系", "遍历所有资源", 0);
            for (int i = 0; i < allFilePathArray.Length; i++)
            {
                if (i % 100 == 0)
                {
                    //间隔刷新UI提示框
                    if (EditorUtility.DisplayCancelableProgressBar("获取资源引用关系", $"遍历所有文件 : {i}/{allFilePathArray.Length}", (float)i / allFilePathArray.Length))
                        break;
                }
                _prefabDependDic[allFilePathArray[i]] = AssetDatabase.GetDependencies(allFilePathArray[i], recursiveFind);
            }

            EditorUtility.DisplayProgressBar("读取项目所有资源", "资源引用排序", 0);
            _prefabDependDic = _prefabDependDic.OrderBy(item => item.Value.Length).ToDictionary(item => item.Key, item => item.Value);

            EditorUtility.ClearProgressBar();

            //----------

            EditorUtility.DisplayProgressBar("反向解析引用关系", "遍历所有资源", 0);

            Dictionary<string, List<string>> tempAssetDic = new Dictionary<string, List<string>>();
            for (int i = 0; i < allFilePathArray.Length; i++)
            {
                if (i % 200 == 0)
                {
                    //间隔刷新UI提示框
                    if (EditorUtility.DisplayCancelableProgressBar("反向解析引用关系", $"遍历所有文件 : {i}/{allFilePathArray.Length}", (float)i / allFilePathArray.Length))
                        break;
                }
                if (!tempAssetDic.TryGetValue(allFilePathArray[i], out var relList))
                {
                    relList = tempAssetDic[allFilePathArray[i]] = new List<string>();
                }

                foreach (var item in _prefabDependDic)
                {
                    if (item.Value.Contains(allFilePathArray[i]))
                    {
                        relList.Add(item.Key);
                    }
                }
            }

            EditorUtility.DisplayProgressBar("反向解析引用关系", "资源引用排序", 0);

            _assetRelatedDic.Clear();
            foreach (var item in tempAssetDic)
            {
                _assetRelatedDic[item.Key] = item.Value.ToArray();
            }

            _assetRelatedDic = _assetRelatedDic.OrderBy(item => item.Value.Length).ToDictionary(item => item.Key, item => item.Value);

            EditorUtility.ClearProgressBar();

            //----------

            EditorUtility.DisplayProgressBar("读取引用资源类型", "获取所有引用文件类型", 0);

            var tempExtentDic = new Dictionary<string, List<string>>();
            foreach (var assetName in _assetRelatedDic.Keys)
            {
                string assetExtent = Path.GetExtension(assetName);
                if (!tempExtentDic.TryGetValue(assetExtent, out var nameList))
                {
                    nameList = tempExtentDic[assetExtent] = new List<string>();
                }
                nameList.Add(assetName);
            }

            _assetExtentNameDic = new Dictionary<string, string[]>(tempExtentDic.Count);
            foreach (var item in tempExtentDic)
            {
                _assetExtentNameDic[item.Key] = item.Value.ToArray();
            }

            _assetExtentArray = tempExtentDic.Keys.ToArray();

            _prefabNameArray = _prefabDependDic.Keys.ToArray();

            EditorUtility.ClearProgressBar();
        }

        //==========

        //查找某个Prefab/Scene引用的所有资源
        public class PrefabDependItem
        {
            private string prefabPath;
            private string[] dependPathArray;

            public Object prefabObject;
            public Object[] dependObjectArray;

            public PrefabDependItem(string _prefabPath, string[] _dependPathArray)
            {
                prefabPath = _prefabPath;
                dependPathArray = _dependPathArray;

                LoadDependObjects();
            }

            private void LoadDependObjects()
            {
                prefabObject = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);

                if (dependPathArray == null || dependPathArray.Length == 0)
                {
                    dependObjectArray = new Object[0];
                    return;
                }

                dependObjectArray = new Object[dependPathArray.Length];
                for (int i = 0; i < dependPathArray.Length; i++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("加载引用该资源的Prefab|Scene", $"加载进度 : {i}/{dependPathArray.Length}", (float)i / dependPathArray.Length))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                    Object tempObj = AssetDatabase.LoadAssetAtPath<Object>(dependPathArray[i]);
                    dependObjectArray[i] = tempObj;
                }
                EditorUtility.ClearProgressBar();
            }
        }

        //查找某个资源被哪些Prefab/Scene引用
        public class AssetRelatedItem
        {
            private string assetPath;
            private string[] relatedPathArray;

            public Object assetObject;
            public Object[] relatedObjectArray;

            public AssetRelatedItem(string _assetPath, string[] _relatedPathArray)
            {
                assetPath = _assetPath;
                relatedPathArray = _relatedPathArray;

                LoadRelatedObjects();
            }

            private void LoadRelatedObjects()
            {
                assetObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (relatedPathArray == null || relatedPathArray.Length == 0)
                {
                    relatedObjectArray = new Object[0];
                    return;
                }

                relatedObjectArray = new Object[relatedPathArray.Length];
                for (int i = 0; i < relatedPathArray.Length; i++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("加载引用该资源的Prefab|Scene", $"加载进度 : {i}/{relatedPathArray.Length}", (float)i / relatedPathArray.Length))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                    Object tempObj = AssetDatabase.LoadAssetAtPath<Object>(relatedPathArray[i]);
                    relatedObjectArray[i] = tempObj;
                }
                EditorUtility.ClearProgressBar();
            }
        }

    }
}