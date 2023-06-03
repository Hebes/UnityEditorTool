using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 标签类
    /// </summary>
    public class ACCoreExpansion_Tag
    {
        

        /// <summary>
        /// 反射获取当前Game视图，提示编译完成
        /// </summary>
        /// <param name="tips"></param>
        public static void ShowNotification(string tips)
        {
            var game = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
            game?.ShowNotification(new GUIContent($"{tips}"));
        }
    }
}
