using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary> �쳵�����ʧ </summary>
    public class ACChackComponentMiss : EditorWindow
    {
        [MenuItem("Assets/����EditorTool/��鶪ʧ���")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACChackComponentMiss), false, "��鶪ʧ���").Show();
        }

        private void OnGUI()
        {
           
        }

        /// <summary>
        /// ˢ�½���
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
