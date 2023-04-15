using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ACTool
{
    public  class ACHierarchyToolComponent : EditorWindow
    {

        [MenuItem("Assets/暗沉EditorTool/Hierarchy面板/组件工具")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACHierarchyToolComponent), false, "Hierarchy面板组件工具").Show();
        }

        private void OnGUI()
        {
            ACHierarchyOhterTool();
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

        private static Font ACHierarchyToolComponent_OhterTool_Prefab { get; set; }

        /// <summary>
        /// HierarchyPanel其他工具
        /// </summary>
        public static void ACHierarchyOhterTool()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("修改组件Text字体设置", EditorStyles.boldLabel);
            //******************************Text组件*****************************
            ACHierarchyToolComponent_OhterTool_Prefab = (Font)EditorGUILayout.ObjectField(ACHierarchyToolComponent_OhterTool_Prefab, typeof(Font), true, GUILayout.MinWidth(100f));
            if (GUILayout.Button("修改选择的物体和子物体", EditorStyles.miniButtonMid))
            {
                List<GameObject> gameObjects = ACToolCoreExpansionFind.ACGetGo.ACLoopGetKeywordGO(String.Empty);
                gameObjects?.ForEach((go) =>
                {
                    go.ACSetFont(ACHierarchyToolComponent_OhterTool_Prefab);
                });
            }

            //******************************去除组件RayCastTarget*****************************
            GUILayout.Space(5f); EditorGUILayout.LabelField("去除组件RayCastTarget:", EditorStyles.largeLabel);
            if (GUILayout.Button($"去除组件RayCastTarget", EditorStyles.miniButtonMid)) 
            {
                ACToolCoreExpansionFind.ACGetObjs().ClearRayCastTarget(); 
            }
        }
    }
}
