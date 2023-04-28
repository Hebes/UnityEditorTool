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

        [MenuItem("Assets/暗沉EditorTool/清理文件操作面板保存数据")]//#E
        public static void ClaerACFileToolOperationAllData()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        public static void ACFileOperation()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("文件操作", EditorStyles.boldLabel);
            //******************************添加前缀******************************
            EditorGUILayout.Space(5f);

            //**************加载选择路径**************
            EditorGUILayout.BeginHorizontal();
            {
                ACFileToolOperation_SelectPath = EditorGUILayout.TextField("加载选择路径", PlayerPrefs.GetString(ACFileToolOperation_SelectPath_Key));

                if (GUILayout.Button("浏览", GUILayout.Width(100)))
                {
                    ACFileToolOperation_SelectPath = EditorUtility.OpenFolderPanel("转移的目录", PlayerPrefs.GetString(ACFileToolOperation_SelectPath_Key), "");
                    if (ACFileToolOperation_SelectPath.Length > 0)
                        PlayerPrefs.SetString(ACFileToolOperation_SelectPath_Key, ACFileToolOperation_SelectPath);

                }

                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    PlayerPrefs.DeleteKey(ACFileToolOperation_SelectPath_Key);
                    ACFileToolOperation_SelectPath = null;
                }

            }
            EditorGUILayout.EndHorizontal();



            if (!string.IsNullOrEmpty(ACFileToolOperation_SelectPath))
            {
                ACFileToolOperation_ScrollRoot = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot, GUILayout.Height(200)); //开启滚动视图
                {
                    string[] strings1 = Directory.GetDirectories(ACFileToolOperation_SelectPath, "*", SearchOption.AllDirectories);
                    for (int i = 0; i < strings1?.Length; i++)
                    {
                        string path = strings1[i];
                        if (path.EndsWith("OutputCache"))
                        {
                            continue;
                        }
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField($"点击选择路路径:   {path}", EditorStyles.largeLabel);
                            if (GUILayout.Button("打开", GUILayout.Width(100)))
                                EditorUtility.RevealInFinder(path);
                            if (GUILayout.Button("删除", GUILayout.Width(100)))
                                Directory.Delete(path, true);
                            if (GUILayout.Button("转移(只限定有文件的)", GUILayout.Width(200)))
                            {
                                //创建文件夹
                                string str = path.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                                ACToolCoreExpansionFolder.ACChackFolder(str);

                                ////移动文件
                                //string[] filePaths = Directory.GetFiles(ACFileToolOperation_SelectPath, "*", SearchOption.AllDirectories);
                                //for (int k = 0; k < filePaths?.Length; k++)
                                //{
                                //    string path1 = filePaths[k];
                                //    string strings222 = path1.Replace("\\", "/");
                                //    string str = strings222.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                                //    File.Copy(path, str, true);
                                //}
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView(); //结束滚动视图
            }

            ////**************选择的文件夹**************
            //EditorGUILayout.BeginHorizontal();
            //{
            //    ACFileToolOperation_OldPathName = EditorGUILayout.TextField("选择的文件夹", PlayerPrefs.GetString(ACFileToolOperation_OldPathName_Key));
            //    if (GUILayout.Button("清除记录", GUILayout.Width(100)))
            //    {
            //        PlayerPrefs.DeleteKey(ACFileToolOperation_OldPathName_Key);
            //        ACFileToolOperation_OldPathName = null;
            //    }
            //}
            //EditorGUILayout.EndHorizontal();
            //if (!string.IsNullOrEmpty(ACFileToolOperation_OldPathName))
            //{
            //    if (!string.IsNullOrEmpty(ACFileToolOperation_OldPathName))
            //    {
            //        DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_OldPathName);
            //        DirectoryInfo[] di = root.GetDirectories();
            //        ACFileToolOperation_ScrollRoot1 = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot1, GUILayout.Height(100)); //开启滚动视图
            //        {
            //            for (int i = 0; i < di?.Length; i++)
            //            {
            //                EditorGUILayout.BeginHorizontal();
            //                {
            //                    string path = di[i].FullName;
            //                    EditorGUILayout.LabelField($"删除文件:   {path}", EditorStyles.largeLabel);
            //                    if (GUILayout.Button("打开", GUILayout.Width(100)))
            //                        EditorUtility.RevealInFinder(path);
            //                    if (GUILayout.Button("删除", GUILayout.Width(100)))
            //                        Directory.Delete(path, true);
            //                }
            //                EditorGUILayout.EndHorizontal();
            //            }
            //        }
            //        EditorGUILayout.EndScrollView(); //结束滚动视图
            //    }
            //}

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
                string[] di = Directory.GetDirectories(ACFileToolOperation_NewPathName, "*", SearchOption.AllDirectories);
                //DirectoryInfo root = new DirectoryInfo(ACFileToolOperation_NewPathName);
                //DirectoryInfo[] di = root.GetDirectories();
                ACFileToolOperation_ScrollRoot1 = EditorGUILayout.BeginScrollView(ACFileToolOperation_ScrollRoot1, GUILayout.Height(100)); //开启滚动视图
                {
                    for (int i = 0; i < di.Length; i++)
                    {
                        if (di[i].EndsWith("OutputCache"))
                        {
                            continue;
                        }
                        EditorGUILayout.BeginHorizontal();
                        {
                            string path = di[i];
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
