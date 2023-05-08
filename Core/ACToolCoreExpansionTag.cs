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
    /// 标签类
    /// </summary>
    public class ACToolCoreExpansionTag
    {
        /// <summary>
        /// 设置ab包名称
        /// </summary>
        public static void ACSetAssetBundleName(string path, string assetBundleName)
        {
            // 设置ab包
            AssetImporter assetImporter1 = AssetImporter.GetAtPath(path);
            assetImporter1.assetBundleName = assetBundleName;
            AssetDatabase.Refresh();
            Debug.Log("set assetbundle success!");
        }

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
