using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
           
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
