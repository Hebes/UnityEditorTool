using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

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
            if (GUILayout.Button("����"))
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
        /// ˢ�½���
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }

        [MenuItem("Assets/MyTools/ɾ�����")]
        public static void RemoveComponent()
        {
            GameObject[] selections = Selection.gameObjects;
            for (int i = 0; i < selections.Length; i++)
            {
                //Category��׼��ɾ�������
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
