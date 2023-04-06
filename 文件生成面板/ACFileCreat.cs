using ACTtool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ACTool
{





    public class ACFileCreat : EditorWindow
    {
        public static Vector2 scrollRoot { get; private set; }

        private static string SceneConfig { get; set; }

        //[MenuItem("Assets/暗沉EditorTool/文件生成/生成配置文件/生成场景文件")]//#E
        public static void FileCreatTool()
        {
            GetWindow(typeof(ACFileCreat), false, "Hierarchy面板通用功能").Show();
        }

        private void OnGUI()
        {

        }

        /// <summary>
        /// 场景配置文件
        /// </summary>
        public static void ACACFileSceneConfigCreat()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
            {
                if (GUILayout.Button("场景配置文件", EditorStyles.miniButtonMid)) { ttt(SceneConfig); }
            }
            EditorGUILayout.EndScrollView();
        }

        public static void ttt(string sceneConfig)
        {

            //输出内容
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public static class ConfigScenes");
            sb.AppendLine("{");
            sb.AppendLine("\t#region 场景名称");
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
            sb.AppendLine("\t#endregion");
            sb.AppendLine("}");

            //生成文件的路径
            string filePath = $"{Application.dataPath}/Core/Config/ConfigScenes.cs";//Assets/Scripts/Config
            if (File.Exists(filePath)) { File.Delete(filePath); }
            using (StreamWriter writer = File.CreateText(filePath)) { writer.Write(sb); Debug.Log("内容写入成功!"); }
            //刷新编辑器
            AssetDatabase.Refresh();
        }
    }
}
