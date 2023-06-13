using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACProjectToolOther : EditorWindow
    {
        /// <summary>
        /// 场景配置文件
        /// </summary>
        public static void OnShow()
        {
            if (GUILayout.Button("场景配置文件", EditorStyles.miniButtonMid)) { ttt(); }
        }

        public static void ttt()
        {
            StringBuilder sb = new StringBuilder();
            //获取场景名称
            string[] files = Directory.GetFiles($"{Application.dataPath}/Scenes", "*.unity");
            Dictionary<string, string> censeName = new Dictionary<string, string>();
            foreach (var fileName in files)
            {
                string[] tempfileName = fileName.Split('\\');
                string temp = tempfileName[tempfileName.Length - 1].Replace(".unity", "");
                string[] scene = temp.Split('.');
                censeName.Add(scene[1], temp);
            }
            if (censeName.Count == 0) { Debug.Log("路径错误"); }
            foreach (var item in censeName)
            {
                string itemNameTemp = Regex.Replace(item.Key, @"\s", "");//正则表达中，"\s" 是指空白，包括空格、换行、tab缩进等所有的空白。
                sb.AppendLine($"\tpublic static string {itemNameTemp} => \"{item.Value}\";");
            }
            Debug.Log(sb.ToString());
            ACCoreExpansion_AssetDatabase.ACRefresh();
        }
    }
}
