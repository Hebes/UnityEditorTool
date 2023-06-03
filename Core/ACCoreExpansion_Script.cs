using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public static class ACCoreExpansion_Script
    {
        /// <summary>
        /// 移除脚本(PS:自定义版本,一个物体里面)
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="InputCustom">自定义脚本名称</param>
        public static void ACRemoveScript(this GameObject gameObject, String InputCustom)
        {
            //正常来说肯定有一个
            Array.ForEach(gameObject.GetComponents<Component>(), (component) =>
            {
                Type type = component.GetType();
                if (type.Name == InputCustom)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject.GetComponent(type));
                    Debug.Log($"{gameObject.name} 移除 {InputCustom}脚本成功");
                }
            });
        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this UnityEngine.Object obj)
        {
            //获取所有的物体
            List<GameObject> gameObjects = new List<GameObject>();
            (obj as GameObject).transform.ACLoopGetAllGameObject(ref gameObjects);
            //移除Miss脚本
            gameObjects?.ForEach(gameObject =>
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            });
            AssetDatabase.Refresh();
            Debug.Log("清理完成!");
        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this UnityEngine.Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                //获取所有的物体
                List<GameObject> gameObjects = new List<GameObject>();
                (obj as GameObject).transform.ACLoopGetAllGameObject(ref gameObjects);
                //移除Miss脚本
                gameObjects?.ForEach(gameObject =>
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                });
                objs.ACAssetDatabaseRefresh();
                Debug.Log("清理完成!");
            });

        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this GameObject gameObject)
        {
            //获取所有的物体
            List<GameObject> gameObjects = new List<GameObject>();
            gameObject.transform.ACLoopGetAllGameObject(ref gameObjects);
            //移除Miss脚本
            gameObjects?.ForEach(gameObject =>
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            });
            gameObject.ACAssetDatabaseRefresh();
            Debug.Log("清理完成!");
        }

        /// <summary>
        /// 移除Miss的脚本
        /// </summary>
        /// <param name="gameObject"></param>
        public static void ACRemoveMissScriptOne(this GameObject gameObject)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        }


        /// <summary>
        /// 添加脚本(PS:自定义版本,一个物体里面)
        /// </summary>
        public static void ACAddScript(this GameObject gameObject, String InputCustom)
        {
            Component[] components = gameObject.GetComponents<Component>();
            bool isExist = false;//是否存在
            foreach (Component comp in components)//正常来说肯定有一个
            {
                Type type = comp.GetType();
                if (type.Name == InputCustom)
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                //Animator
                Type type = null;
                if (type == null) type = InputCustom.ACReflectClass("UnityEngine.UI");
                if (type == null) type = InputCustom.ACReflectClass("UnityEngine");
                gameObject.AddComponent(type);
                Debug.Log($"{gameObject.name}添加{type.Name}完成");
            }
        }
    }
}
