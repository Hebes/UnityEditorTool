using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public static class ACToolExpansionFind
    {
        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] ACGetSelection()
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] ACGetSelection(this UnityEngine.Object obj)
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取一个
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object ACGetSelectionOne()
        {
            return Selection.objects.First();
        }

        /// <summary>
        /// 获取一个
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object ACGetSelectionOne(this UnityEngine.Object obj)
        {
            return Selection.objects.First();
        }

        /// <summary>
        /// 获取返回的
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> ACGetSelectionGos(this UnityEngine.Object[] objects)
        {
            List<GameObject> gos = new List<GameObject>();
            gos.AddRange(objects);
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
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static List<GameObject> ACLoopGetKeywordGO(this GameObject transform, string keyValue)
        {
            List <GameObject> goList = new List<GameObject>(); 
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
        public static void LoopGetAllTransform(this Transform transform, ref List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).LoopGetAllTransform(ref transforms);
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
        public static List<Transform> ACGetGameObjects(this GameObject gameObject, string keyWord)
        {
            List<Transform> gos = gameObject.GetComponentsInChildren<Transform>(true).ToList();
            return gos.FindAll((go) => { return go.name.StartsWith(keyWord); });
        }
    }
}
