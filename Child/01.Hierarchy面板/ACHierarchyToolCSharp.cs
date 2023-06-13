using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    脚本

-----------------------*/

namespace ACTool
{
    public class ACHierarchyToolCSharpCSharp : EditorWindow
    {
        private static Vector2 ACHierarchyToolCSharpCSharp_ScrollRoot { get; set; }//滑动条
        private static string ACHierarchyToolCSharp_InputCustom { get; set; }//输入自定义

        /// <summary>
        /// 移除脚本
        /// </summary>
        public static void OnShow()
        {
            ACHierarchyToolCSharpCSharp_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyToolCSharpCSharp_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Hierarchy通用工具", EditorStyles.boldLabel);
                    //******************************移除丢失脚本******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("移除丢失脚本:", EditorStyles.largeLabel);
                    if (GUILayout.Button("移除丢失脚本", EditorStyles.miniButtonMid))
                    {
                        ACRemoveMissScript();
                    }
                    if (GUILayout.Button("查找丢失脚本的物体", EditorStyles.miniButtonMid))
                    {
                        ACGetMissScript();
                    }
                    //if (GUILayout.Button("判断是否是预制体", EditorStyles.miniButtonMid))
                    //{
                    //    List<GameObject> gos = ACToolCoreExpansionFind.GetAllGameObject();
                    //    PrefabUtility.getpa
                    //}
                    ACHierarchyToolCSharp_InputCustom = EditorGUILayout.TextField("自定义脚本的名称", ACHierarchyToolCSharp_InputCustom);
                    if (GUILayout.Button("移除自定义脚本(单个物体和其子物体)", EditorStyles.miniButtonMid))
                    {
                        ACRemoveCustomScript();
                    }
                    if (GUILayout.Button("移除自定义脚本(选中的物体不包括其子物体)", EditorStyles.miniButtonMid))
                    {
                        ACRemoveSelectCustomScript();
                    }
                    if (GUILayout.Button("添加脚本(选中的物体不包括其子物体)", EditorStyles.miniButtonMid))
                    {
                        ACAddSelectCustomScript();
                    }

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Animator", EditorStyles.miniButtonMid))
                        {
                            ACHierarchyToolCSharp_InputCustom = "Animator";
                        }
                        if (GUILayout.Button("Button", EditorStyles.miniButtonMid))
                        {
                            ACHierarchyToolCSharp_InputCustom = "Button";
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }

        /// <summary>
        /// 移除丢失的脚本
        /// </summary>
        private static void ACRemoveMissScript()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < Selection.objects?.Length; i++)
                (Selection.objects[i] as GameObject).transform.ACLoopGetAllGameObject(ref gameObjects); //获取所有的物体
            //移除所有的物体的丢失脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObjects[i]);
            AssetDatabase.Refresh();
            Debug.Log("清理完成!");

        }

        /// <summary>
        /// 移除自定义脚本(单个物体和其子物体)
        /// </summary>
        public static void ACRemoveCustomScript()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            //获取所有的组件
            ACCoreExpansion_Find.ACLoopGetAllGameObject(ACCoreExpansion_Find.ACGetGo.transform, ref gameObjects);
            //移除自定义脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyToolCSharp_InputCustom);
        }

        /// <summary>
        /// 移除自定义脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACRemoveSelectCustomScript()
        {
            List<GameObject> gameObjects = ACCoreExpansion_Find.ACGetObjs().ACGetGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyToolCSharp_InputCustom);
        }

        /// <summary>
        /// 添加脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACAddSelectCustomScript()
        {
            List<GameObject> gameObjects = ACCoreExpansion_Find.ACGetObjs().ACGetGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACAddScript(ACHierarchyToolCSharp_InputCustom);
        }

        /// <summary>
        /// 获取丢失的脚本的物体
        /// </summary>
        public static void ACGetMissScript()
        {
            //Get the current scene and all top-level GameObjects in the scene hierarchy
            Scene currentScene = SceneManager.GetActiveScene();//获取当前的活动场景
            GameObject[] rootObjects = currentScene.GetRootGameObjects();//获取当前场景的顶级GameObjects

            List<UnityEngine.Object> objectsWithDeadLinks = new List<UnityEngine.Object>();
            foreach (GameObject g in rootObjects)
            {
                var trans = g.GetComponentsInChildren<Transform>();
                foreach (Transform tran in trans)
                {
                    Component[] components = tran.GetComponents<Component>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        Component currentComponent = components[i];

                        //If the component is null, that means it's a missing script!
                        if (currentComponent == null)
                        {
                            //Add the sinner to our naughty-list
                            objectsWithDeadLinks.Add(tran.gameObject);
                            Selection.activeGameObject = tran.gameObject;
                            Debug.Log(tran.gameObject + " has a missing script!"); //Console中输出
                            break;
                        }
                    }
                }
                //Get all components on the GameObject, then loop through them 
            }
            if (objectsWithDeadLinks.Count > 0)
            {
                //Set the selection in the editor
                Selection.objects = objectsWithDeadLinks.ToArray();
            }
            else
            {
                Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
            }
        }
    }
}
