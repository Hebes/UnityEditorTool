using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public static class ACCoreExpansion_Find
    {
        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] ACGetObjs()
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] ACGetObjs(this UnityEngine.Object obj)
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取一个
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object ACGetObj()
        {
            return Selection.objects.First();
        }

        /// <summary>
        /// 获取选中的一个
        /// </summary>
        /// <returns></returns>
        public static GameObject ACGetGo
        {
            get { return Selection.objects.First() as GameObject; }
        }


        /// <summary>
        /// 获取一个
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object ACGetObj(this UnityEngine.Object obj)
        {
            return Selection.objects.First();
        }

        /// <summary>
        /// 获取返回的
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> ACGetGos(this UnityEngine.Object[] objects)
        {
            List<GameObject> gos = new List<GameObject>();
            Array.ForEach(objects, (obj) => { gos.Add(obj as GameObject); });
            return gos;
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static void ACLoopGetKeywordGO(this Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).ACLoopGetKeywordGO(keyValue, ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板,子物体)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static List<GameObject> ACLoopGetKeywordGO(this GameObject transform, string keyValue)
        {
            List<GameObject> goList = new List<GameObject>();
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.transform.childCount; i++)
                transform.transform.GetChild(i).ACLoopGetKeywordGO(keyValue, ref goList);
            return goList;
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="goList"></param>
        public static void ACLoopGetAllGameObject(this Transform transform, ref List<GameObject> goList)
        {
            goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).ACLoopGetAllGameObject(ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="transformPrefix"></param>
        /// <param name="gameObjects"></param>
        public static void ACLoopGetAllTransform(this Transform transform, ref List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).ACLoopGetAllTransform(ref transforms);
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板,隐藏除外)
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetAllGameObject(this UnityEngine.Object obj)
        {
            return UnityEngine.Object.FindObjectsOfType<GameObject>().ToList();//查找物体
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板,隐藏除外)
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetAllGameObject()
        {
            return UnityEngine.Object.FindObjectsOfType<GameObject>().ToList();//查找物体
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板)
        /// </summary>
        /// <returns></returns>
        public static List<Transform> GetAllTransform(this UnityEngine.Object obj)
        {
            return UnityEngine.Object.FindObjectsOfType<Transform>().ToList();//查找物体
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板)
        /// </summary>
        /// <returns></returns>
        public static List<Transform> GetAllTransform()
        {
            return UnityEngine.Object.FindObjectsOfType<Transform>().ToList();//查找物体
        }

        /// <summary>
        /// 获取Hierarchy全部物体
        /// </summary>
        /// <returns></returns>
        public static List<UnityEngine.Object> ACGetHierarchyAllGameObject()
        {
            return Resources.FindObjectsOfTypeAll<UnityEngine.Object>().ToList();
        }

        /// <summary>
        /// 获取所有物体
        /// </summary>
        /// <returns></returns>
        public static List<UnityEngine.Object> ACGetResourcesAllObject()
        {
            return Resources.FindObjectsOfTypeAll<UnityEngine.Object>().ToList();
        }

        /// <summary>
        /// 查找子物体.(PS:适用于单个,代码中可直接用,可找到隐藏物体)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childName">物体名称</param>
        /// <returns></returns>
        public static Transform ACLoopGetOneTransform(this Transform transform, string childName)
        {
            //递归:方法内部又调用自身的过程。
            //1.在子物体中查找
            Transform childTF = transform.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = transform.GetChild(i).ACLoopGetOneTransform(childName); // 2.将任务交给子物体
                if (childTF != null) return childTF;
            }
            Debug.Log($"没有找到物体{childName}"); return null;
        }

        /// <summary>
        /// 找到子对象的对应控件 返回一本字典(P:组件类型为Key)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="keyValue"></param>
        /// <param name="controlDic"></param>
        public static void ACGetAllToDic<T>(this GameObject gameObject, string keyValue, ref Dictionary<string, List<Component>> controlDic) where T : Component
        {
            //返回所有keyValue开头的物品
            List<GameObject> transforms = gameObject.ACLoopGetKeywordGO(keyValue);

            //添加进字典
            for (int i = 0; i < transforms?.Count; i++)
            {
                GameObject tempGo = transforms[i];//组件的物体
                T go = tempGo.GetComponent<T>();
                if (go == null) continue;//如果获取组件空 跳过
                string objType = go.GetType().Name;//组件的类型
                if (controlDic.ContainsKey(objType))//添加组件
                    controlDic[objType].Add(go);
                else
                    controlDic.Add(objType, new List<Component>() { go });
            }
        }

        /// <summary>
        /// 查找物体,包含隐藏的
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="keyWord">关键词</param>
        /// <returns></returns>
        public static List<Transform> ACGetTransforms(this GameObject gameObject, string keyWord)
        {
            List<Transform> gos = gameObject.GetComponentsInChildren<Transform>(true).ToList();
            return gos.FindAll((go) => { return go.name.StartsWith(keyWord); });
        }

        /// <summary>
        /// 获取组件路径
        /// </summary>
        /// <param name="transformTF">需要获取路径的子物体</param>
        /// <param name="selectGoName">选择的父物体(transformTF要在这个父物体下)</param>
        /// <returns>返回的路径</returns>
        public static string ACGetPathTransform(this Transform transformTF, string selectGoName)
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
                path += j != 0 ? $"{strs[j]}/" : $"{strs[j]}";
            }
            return path;
        }

        /// <summary>
        /// 获取资源路径(PS:Project面板选中的物体)
        /// </summary>
        public static string ACGetAssetDataPath(this UnityEngine.Object @object)
        {
            return AssetDatabase.GetAssetPath(@object);
        }
    }
}
