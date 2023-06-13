using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACPrefabPreview : EditorWindow
    {
        private static Object Obj; 
        private static Editor PreviewObj;

        private static Object LatsObj;

        public static void OnShow()
        {
            //可以在这里点击输出框
            Obj = EditorGUILayout.ObjectField(Obj, typeof(Object), false);

            //创建一个窗口
            if (Obj != null && Obj != LatsObj)
            {
                PreviewObj = Editor.CreateEditor(Obj);
                LatsObj = Obj;
            }

            //创建预览画面
            if (PreviewObj != null)
            {
                PreviewObj.OnPreviewGUI(GUILayoutUtility.GetRect(400, 400), EditorStyles.label);
            }
        }
    }
}
