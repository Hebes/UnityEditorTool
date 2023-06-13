using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    组件设置

-----------------------*/

namespace ACTool
{
    public class ACHierarchyToolComponent : EditorWindow
    {
        private static Font ACHierarchyToolComponent_OhterTool_Prefab { get; set; }

        /// <summary>
        /// HierarchyPanel其他工具
        /// </summary>
        public static void OnShow()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("修改组件Text字体设置", EditorStyles.boldLabel);
            //******************************Text组件*****************************
            ACHierarchyToolComponent_OhterTool_Prefab = (Font)EditorGUILayout.ObjectField(ACHierarchyToolComponent_OhterTool_Prefab, typeof(Font), true, GUILayout.MinWidth(100f));
            if (GUILayout.Button("修改选择的物体和子物体", EditorStyles.miniButtonMid))
            {
                List<GameObject> gameObjects = ACCoreExpansion_Find.ACGetGo.ACLoopGetKeywordGO(String.Empty);
                gameObjects?.ForEach((go) =>
                {
                    go.ACSetFont(ACHierarchyToolComponent_OhterTool_Prefab);
                });
            }

            //******************************去除组件RayCastTarget*****************************
            GUILayout.Space(5f); EditorGUILayout.LabelField("去除组件RayCastTarget:", EditorStyles.largeLabel);
            if (GUILayout.Button($"去除组件RayCastTarget", EditorStyles.miniButtonMid)) 
            {
                ACCoreExpansion_Find.ACGetObjs().ClearRayCastTarget(); 
            }
        }
    }
}
