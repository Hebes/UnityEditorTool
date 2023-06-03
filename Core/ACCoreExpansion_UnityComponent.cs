using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ACTool
{
    public static class ACCoreExpansion_UnityComponent
    {
        /// <summary>
        /// 去除组件RayCast Target
        /// </summary>
        public static void ClearRayCastTarget(this UnityEngine.Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go.GetComponent<Text>() != null) { go.GetComponent<Text>().raycastTarget = false; }
                if (go.GetComponent<Image>() != null) { go.GetComponent<Image>().raycastTarget = false; }
                if (go.GetComponent<RawImage>() != null) { go.GetComponent<RawImage>().raycastTarget = false; }
                if(go.GetComponent<Text>() == null&& go.GetComponent<Image>() == null&& go.GetComponent<RawImage>() == null)
                    if (EditorUtility.DisplayDialog("消息提示", go.name + "没有找到需要去除的RayCast Target选项", "确定")) { }
            });
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="font"></param>
        public static void ACSetFonts(this UnityEngine.Object[] objs,Font font)
        {
            Array.ForEach(objs, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go.GetComponent<Text>() != null)
                    go.GetComponent<Text>().font = font;
            });
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="font"></param>
        public static void ACSetFont(this GameObject go, Font font)
        {
            if (go.GetComponent<Text>() != null)
                go.GetComponent<Text>().font = font;
        }
    }
}
