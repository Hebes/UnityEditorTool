using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 物体脚本
    /// </summary>
    public class ACHierarchyToolCSharpCSharp : EditorWindow
    {
        [MenuItem("Assets/暗沉EditorTool/Hierarchy面板/物体脚本操作")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACHierarchyToolCSharpCSharp), false, "Hierarchy面板物体脚本操作").Show();
        }

        private void OnGUI()
        {
            ACHierarchyRemoveDemo();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        void OnInspectorUpdate()
        {
            // Call Repaint on OnInspectorUpdate as it repaints the windows
            // less times as if it was OnGUI/Update
            Repaint();
        }

        private static Vector2 ACHierarchyToolCSharpCSharp_ScrollRoot { get; set; }//滑动条
        private static string ACHierarchyToolCSharp_InputCustom { get; set; }//输入自定义

        /// <summary>
        /// 移除脚本
        /// </summary>
        public static void ACHierarchyRemoveDemo()
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
            ACToolCoreExpansionFind.ACLoopGetAllGameObject(ACToolCoreExpansionFind.ACGetGo.transform, ref gameObjects);
            //移除自定义脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyToolCSharp_InputCustom);
        }

        /// <summary>
        /// 移除自定义脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACRemoveSelectCustomScript()
        {
            List<GameObject> gameObjects = ACToolCoreExpansionFind.ACGetObjs().ACGetGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACRemoveScript(ACHierarchyToolCSharp_InputCustom);
        }

        /// <summary>
        /// 添加脚本(选中的物体不包括其子物体)
        /// </summary>
        public static void ACAddSelectCustomScript()
        {
            List<GameObject> gameObjects = ACToolCoreExpansionFind.ACGetObjs().ACGetGos();
            for (int i = 0; i < gameObjects?.Count; i++)
                gameObjects[i].ACAddScript(ACHierarchyToolCSharp_InputCustom);
        }
    }
}
