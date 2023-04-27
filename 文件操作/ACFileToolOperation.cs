using DG.Tweening.Plugins.Core.PathCore;
using I18N.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ACTool
{
    public class ACFileToolOperation : EditorWindow
    {
        public static string ACFileToolOperation_OldPathName { get; set; }
        public static string ACFileToolOperation_OldPathName_Key { get; set; } = "ACFileToolOperation_OldPathName_Key";
        public static string ACFileToolOperation_NewPathName { get; set; }
        public static string ACFileToolOperation_NewPathName_Key { get; set; } = "ACFileToolOperation_NewPathName_Key";
        public static string ACFileToolOperation_SelectPath { get; set; }
        public static string ACFileToolOperation_SelectPath_Key { get; set; } = "ACFileToolOperation_SelectPath_Key";
        public static Vector2 ACFileToolOperation_ScrollRoot { get; set; }
        public static Vector2 ACFileToolOperation_ScrollRoot1 { get; private set; }

        [MenuItem("Assets/暗沉EditorTool/文件操作面板")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACFileToolOperation), false, "文件操作面板").Show();
        }

        private void OnGUI()
        {
            ACFileOperation();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        void OnInspectorUpdate()
        {
            // Call Repaint on OnInspectorUpdate as it repaints the windows
            // less times as if it was OnGUI/Update
            Repaint();
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        public static void ACFileOperation()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("文件操作", EditorStyles.boldLabel);
            //******************************添加前缀******************************
            EditorGUILayout.Space(5f);

            if (GUILayout.Button("创建文件路径", GUILayout.Width(100)))
            {
                //string newFolderPath = ACToolCoreExpansionFolder.ACChackFolder($"{"C:\\Users\\yet\\Desktop\\Hot\\CDN\\PC"}/{"}");
                string newFolderPath = ACToolCoreExpansionFolder.ACChackFolder("C:\\Users\\yet\\Desktop\\Hot\\CDN\\PC/Code/Test");
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("选择的文件夹:", EditorStyles.largeLabel);
                //https://blog.csdn.net/Zhu_daye/article/details/103098324 C#使用DirectoryInfo.MoveTo将文件、目录（文件夹）移动或复制到另一个目录中
                if (GUILayout.Button("移动", GUILayout.Width(100)))
                {

                    //创建文件夹
                    string[] strings2 = Directory.GetDirectories(ACFileToolOperation_OldPathName, "*");
                    for (int i = 0; i < strings2?.Length; i++)
                    {
                        string strings = strings2[i];
                        string strings222 = strings.Replace("\\", "/");
                        string str = strings222.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                        ACToolCoreExpansionFolder.ACChackFolder(str);
                    }
                    //移动文件
                    string[] filePaths = Directory.GetFiles(ACFileToolOperation_OldPathName, "*", SearchOption.AllDirectories);
                    for (int i = 0; i < filePaths?.Length; i++)
                    {
                        string path = filePaths[i];
                        string strings222 = path.Replace("\\", "/");
                        string str = strings222.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                        File.Copy(path, str, true);
                    }

                    //string[] strings = ACFileToolOperation_OldPathName.Split('\\');
                    //string folderName = strings[strings.Length - 1];

                    //for (int i = 0; i < strings2?.Length; i++)
                    //{
                    //   string[] strs = strings2[i].Split(folderName);
                    //    string str = strings2[i][]
                    //    string newFolderPath = ACToolCoreExpansionFolder.ACChackFolder($"{ACFileToolOperation_NewPathName}/{folderName}/{}");
                    //}



                    //string[] strings1 = ACFileToolOperation_OldPathName.Split(folderName);
                    //string folderName1 = strings[strings1.Length - 1];
                    //Debug.Log(folderName1);


                    //DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_OldPathName);
                    //DirectoryInfo[] di = root.GetDirectories();
                    //for (int i = 0; i < di.Length; i++)
                    //{
                    //    string path = di[i].FullName;
                    //    Debug.Log(path);
                    //    string newFolderPath1 = ACToolCoreExpansionFolder.ACChackFolder($"{newFolderPath}/{folderName1}");
                    //}



                    ////移动文件
                    ////获取原来的文件夹内的所有文件
                    //string[] paths = Directory.GetFiles(ACFileToolOperation_OldPathName, "*", SearchOption.AllDirectories);
                    //for (int i = 0; i < paths.Length; i++)
                    //{
                    //    FileInfo file = new FileInfo(paths[i]);
                    //    file.CopyTo(newFolderPath);
                    //}

                    ////复制文件
                    //Array.ForEach(Directory.GetFiles(ACFileToolOperation_OldPathName), (path) =>
                    //{
                    //    FileInfo fileInfo = new FileInfo(path);
                    //    string newPath = newFolderPath + "/" + fileInfo.Name;
                    //    File.Copy(path, newPath, true);
                    //});
                }
            }
            EditorGUILayout.EndHorizontal();
            //**************加载选择路径**************
            EditorGUILayout.BeginHorizontal();
            {
                ACFileToolOperation_SelectPath = EditorGUILayout.TextField("加载选择路径", PlayerPrefs.GetString(ACFileToolOperation_SelectPath_Key));
                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    PlayerPrefs.DeleteKey(ACFileToolOperation_SelectPath_Key);
                    ACFileToolOperation_SelectPath = null;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(ACFileToolOperation_SelectPath))
            {
                DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_SelectPath);
                DirectoryInfo[] di = root.GetDirectories();
                ACFileToolOperation_ScrollRoot = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot, GUILayout.Height(100)); //开启滚动视图
                {
                    for (int i = 0; i < di.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            string path = di[i].FullName;
                            EditorGUILayout.LabelField($"点击选择路路径:   {path}", EditorStyles.largeLabel);
                            if (GUILayout.Button("打开", GUILayout.Width(100)))
                                EditorUtility.RevealInFinder(path);
                            if (GUILayout.Button("选择", GUILayout.Width(100)))
                                PlayerPrefs.SetString(ACFileToolOperation_OldPathName_Key, path);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView(); //结束滚动视图
                //string[] paths = Directory.GetFiles(ACFileToolOperation_SelectPath, "*", SearchOption.AllDirectories);
                //ACFileToolOperation_ScrollRoot = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot, GUILayout.Height(200)); //开启滚动视图
                //{
                //    for (int i = 0; i < paths?.Length; i++)
                //    {
                //        string path = paths[i];

                //        if (path.EndsWith(".json") ||
                //            path.EndsWith(".manifest") ||
                //            path.EndsWith(".version") ||
                //            path.EndsWith(".bundle") ||
                //            path.EndsWith(".bytes") ||
                //            path.EndsWith("OutputCache\\OutputCache") ||
                //            path.EndsWith(".hash")
                //            )
                //        {
                //            continue;
                //        }
                //        EditorGUILayout.BeginHorizontal();
                //        {
                //            EditorGUILayout.LabelField($"点击选择路路径:   {path}", EditorStyles.largeLabel);
                //            if (GUILayout.Button("选择", GUILayout.Width(100)))
                //            {
                //                PlayerPrefs.SetString(ACFileToolOperation_OldPathName_Key, path);
                //            }
                //        }
                //        EditorGUILayout.EndHorizontal();
                //    }
                //}
                //EditorGUILayout.EndScrollView(); //结束滚动视图
            }

            //**************选择的文件夹**************
            EditorGUILayout.BeginHorizontal();
            {
                ACFileToolOperation_OldPathName = EditorGUILayout.TextField("选择的文件夹", PlayerPrefs.GetString(ACFileToolOperation_OldPathName_Key));
                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    PlayerPrefs.DeleteKey(ACFileToolOperation_OldPathName_Key);
                    ACFileToolOperation_OldPathName = null;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(ACFileToolOperation_OldPathName))
            {
                DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_OldPathName);
                DirectoryInfo[] di = root.GetDirectories();
                ACFileToolOperation_ScrollRoot1 = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot1, GUILayout.Height(100)); //开启滚动视图
                {
                    for (int i = 0; i < di.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            string path = di[i].FullName;
                            EditorGUILayout.LabelField($"删除文件:   {path}", EditorStyles.largeLabel);
                            if (GUILayout.Button("打开", GUILayout.Width(100)))
                                EditorUtility.RevealInFinder(path);
                            if (GUILayout.Button("删除", GUILayout.Width(100)))
                                Directory.Delete(path, true);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView(); //结束滚动视图
            }

            //**************转移的目录**************
            EditorGUILayout.LabelField("转移的目录:", EditorStyles.largeLabel);
            EditorGUILayout.BeginHorizontal();
            {
                ACFileToolOperation_NewPathName = EditorGUILayout.TextField("转移的目录", PlayerPrefs.GetString(ACFileToolOperation_NewPathName_Key));
                if (GUILayout.Button("浏览", GUILayout.Width(100)))
                {
                    ACFileToolOperation_NewPathName = EditorUtility.OpenFolderPanel("转移的目录", PlayerPrefs.GetString(ACFileToolOperation_NewPathName_Key), "");
                    if (ACFileToolOperation_NewPathName.Length > 0)
                        PlayerPrefs.SetString(ACFileToolOperation_NewPathName_Key, ACFileToolOperation_NewPathName);
                }
                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    PlayerPrefs.DeleteKey(ACFileToolOperation_NewPathName_Key);
                    ACFileToolOperation_NewPathName = null;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(ACFileToolOperation_NewPathName))
            {
                DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_NewPathName);
                DirectoryInfo[] di = root.GetDirectories();
                ACFileToolOperation_ScrollRoot1 = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot1, GUILayout.Height(100)); //开启滚动视图
                {
                    for (int i = 0; i < di.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            string path = di[i].FullName;
                            EditorGUILayout.LabelField($"删除文件:   {path}", EditorStyles.largeLabel);
                            if (GUILayout.Button("打开", GUILayout.Width(100)))
                                EditorUtility.RevealInFinder(path);
                            if (GUILayout.Button("删除", GUILayout.Width(100)))
                                Directory.Delete(path, true);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView(); //结束滚动视图
            }
        }
    }
}
