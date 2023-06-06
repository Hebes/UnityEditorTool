using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace ACTool
{
    /// <summary> 检车组件丢失 </summary>
    public class ACChackComponentMiss : EditorWindow
    {
        [MenuItem("Assets/暗沉EditorTool/检查丢失组件")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACChackComponentMiss), false, "检查丢失组件").Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("测试"))
            {
                var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
                var previousSelection = Selection.objects;
                Selection.objects = allGos;
                var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
                Selection.objects = previousSelection;
                foreach (var trans in selectedTransforms)
                {
                    Debug.Log(trans.name);
                }
            }


        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }

        [MenuItem("Assets/MyTools/删除组件")]
        public static void RemoveComponent()
        {
            GameObject[] selections = Selection.gameObjects;
            for (int i = 0; i < selections.Length; i++)
            {
                //Category：准备删除的组件
                Component[] t = selections[i].GetComponentsInChildren<Component>(true);
                for (int j = 0; j < t.Length; j++)
                {
                    DestroyImmediate(t[j], true);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
