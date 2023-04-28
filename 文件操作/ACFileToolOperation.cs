using DG.Tweening.Plugins.Core.PathCore;
using I18N.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace ACTool
{
    public class ACFileToolOperation : EditorWindow
    {
        public static string ACFileToolOperation_NewPathName { get; set; }
        public static string ACFileToolOperation_SelectPath { get; set; }

        private static Dictionary<string, List<string>> configTextAssetResolution { get; set; } = new Dictionary<string, List<string>>();//configTextAsset解析
        private static bool isResolution { get; set; } = true;//是否解析


        [MenuItem("Assets/暗沉EditorTool/文件操作面板")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACFileToolOperation), false, "文件操作面板").Show();
        }

        private void OnGUI()
        {
            ResolutionTextAsset(LoadConfigPath);
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

            //**************加载选择路径**************
            EditorGUILayout.BeginHorizontal();
            {
                string key = "#加载的路径";
                string path = string.Empty;
                if (configTextAssetResolution[key].Count > 0)
                {
                    path = configTextAssetResolution[key][0];
                }

                ACFileToolOperation_SelectPath = EditorGUILayout.TextField("加载选择路径", path);

                if (GUILayout.Button("浏览", GUILayout.Width(100)))
                {
                    ACFileToolOperation_SelectPath = EditorUtility.OpenFolderPanel("转移的目录", path, "");
                    if (ACFileToolOperation_SelectPath.Length > 0)
                    {
                        configTextAssetResolution[key].Clear();
                        configTextAssetResolution[key].Add(ACFileToolOperation_SelectPath);
                        ClearTextAsset(LoadConfigPath);
                        string content = DictionaryChangeStr();
                        WriteTextAsset(LoadConfigPath, content);
                        ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                        isResolution = true;
                    }
                }

                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    ACFileToolOperation_SelectPath = null;
                    configTextAssetResolution[key].Clear();
                    ClearTextAsset(LoadConfigPath);
                    string content = DictionaryChangeStr();
                    WriteTextAsset(LoadConfigPath, content);
                    ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                    isResolution = true;
                }

            }
            EditorGUILayout.EndHorizontal();



            if (!string.IsNullOrEmpty(ACFileToolOperation_SelectPath))
            {
                string key = "#加载路径的排除";
                string[] strings1 = Directory.GetDirectories(ACFileToolOperation_SelectPath, "*", SearchOption.AllDirectories);
                for (int i = 0; i < strings1?.Length; i++)
                {
                    string path = strings1[i];

                    //跳过排除路径
                    bool isOn = configTextAssetResolution[key].Contains(path);
                    if (isOn) continue;
                    //显示
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"点击选择路路径:   {path}", EditorStyles.largeLabel);
                        if (GUILayout.Button("打开", GUILayout.Width(100)))
                            EditorUtility.RevealInFinder(path);
                        if (GUILayout.Button("删除", GUILayout.Width(100)))
                            Directory.Delete(path, true);
                        if (GUILayout.Button("排除路径", GUILayout.Width(100)))
                        {
                            configTextAssetResolution[key].Add(path);
                            ClearTextAsset(LoadConfigPath);
                            string content = DictionaryChangeStr();
                            WriteTextAsset(LoadConfigPath, content);
                            ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                            isResolution = true;
                        }
                        if (GUILayout.Button("转移(只限定有文件的)", GUILayout.Width(200)))
                        {
                            //创建文件夹
                            string str = path.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                            ACToolCoreExpansionFolder.ACChackFolder(str);

                            //移动文件
                            string[] filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                            for (int k = 0; k < filePaths?.Length; k++)
                            {
                                string path1 = filePaths[k];
                                string strings222 = path1.Replace("\\", "/");
                                string str11 = strings222.Replace(ACFileToolOperation_SelectPath, ACFileToolOperation_NewPathName);
                                File.Copy(path1, str11, true);
                                isResolution = true;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            //**************转移的目录**************
            EditorGUILayout.LabelField("转移的目录:", EditorStyles.largeLabel);
            EditorGUILayout.BeginHorizontal();
            {
                string key = "#转移的路径";
                string path = string.Empty;
                if (configTextAssetResolution[key].Count > 0)
                    path = configTextAssetResolution[key][0];

                ACFileToolOperation_NewPathName = EditorGUILayout.TextField("转移的目录", path);
                
                if (GUILayout.Button("浏览", GUILayout.Width(100)))
                {
                    ACFileToolOperation_NewPathName = EditorUtility.OpenFolderPanel("转移的目录", path, "");
                    if (ACFileToolOperation_NewPathName.Length > 0)
                    {
                        configTextAssetResolution[key].Clear();
                        configTextAssetResolution[key].Add(ACFileToolOperation_NewPathName);
                        ClearTextAsset(LoadConfigPath);
                        string content = DictionaryChangeStr();
                        WriteTextAsset(LoadConfigPath, content);
                        ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                    }
                }
                if (GUILayout.Button("清除记录", GUILayout.Width(100)))
                {
                    ACFileToolOperation_SelectPath = null;
                    configTextAssetResolution[key].Clear();
                    ClearTextAsset(LoadConfigPath);
                    string content = DictionaryChangeStr();
                    WriteTextAsset(LoadConfigPath, content);
                    ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                }
            }
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(ACFileToolOperation_NewPathName))
            {
                string[] di = Directory.GetDirectories(ACFileToolOperation_NewPathName, "*", SearchOption.AllDirectories);
                string key = "#转移路径的排除";
                for (int i = 0; i < di.Length; i++)
                {
                    string path = di[i];
                    //跳过排除路径
                    bool isOn = configTextAssetResolution[key].Contains(path);
                    if (isOn) continue;
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"删除文件:   {path}", EditorStyles.largeLabel);
                        if (GUILayout.Button("复制路径", GUILayout.Width(100)))
                            ACToolCoreExpansionCopy.ACCopyWord(path);
                        if (GUILayout.Button("打开", GUILayout.Width(100)))
                            EditorUtility.RevealInFinder(path);
                        if (GUILayout.Button("删除", GUILayout.Width(100)))
                            Directory.Delete(path, true);
                        if (GUILayout.Button("排除路径", GUILayout.Width(100)))
                        {
                            configTextAssetResolution[key].Add(path);
                            ClearTextAsset(LoadConfigPath);
                            string content = DictionaryChangeStr();
                            WriteTextAsset(LoadConfigPath, content);
                            ACToolCoreExpansionDateSave.ACAssetDatabaseRefresh();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        /// <summary>
        /// 配置文件加载解析
        /// </summary>
        private void ResolutionTextAsset(string FilePath)
        {
            if (!isResolution) return;
            string content = string.Empty;
            FileStream fsRead = null;
            configTextAssetResolution.Clear();
            using (fsRead = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                int r = fsRead.Read(buffer, 0, buffer.Length);
                content = Encoding.UTF8.GetString(buffer, 0, r);
                ////关闭流
                //fsRead.Close();
                ////释放流所占用的资源
                //fsRead.Dispose();
                //Debug.Log(content);

                //fsRead.Flush();    //清空缓冲区
                //fsRead.Close();    //关闭写数据流
                //fsRead.Close();   //关闭文件流
            }

            string[] configTextAssetContent = content.Split("\r\n");
            string handle = string.Empty;
            foreach (var item in configTextAssetContent)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.StartsWith("#"))
                {
                    handle = item;
                    if (configTextAssetResolution.ContainsKey(item))
                        configTextAssetResolution[item].Add(item);
                    else
                        configTextAssetResolution.Add(item, new List<string>());
                    continue;
                }
                configTextAssetResolution[handle].Add(item);
            }
            //Debug.Log(1);
            isResolution = false;
        }

        private static string LoadConfigPath { get; set; } = "Assets/Editor/ACExporter/UnityEditorTool/文件操作/文件操作配置文件.txt";

        /// <summary>
        /// 写入文件
        /// </summary>
        private static void WriteTextAsset(string FilePath, string Content)
        {
            FileStream fsWrite = null;
            using (fsWrite = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(Content);
                fsWrite.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 清除文件内容
        /// </summary>
        private static void ClearTextAsset(string FilePath)
        {
            FileStream clearWrite = null;
            using (clearWrite = new FileStream(FilePath, FileMode.Truncate, FileAccess.ReadWrite))
            {
                Debug.Log("清除成功");
            }
        }

        /// <summary>
        /// 字典转成字符串
        /// </summary>
        /// <returns></returns>
        private static string DictionaryChangeStr()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in configTextAssetResolution.Keys)
            {
                sb.AppendLine(item);
                for (int i = 0; i < configTextAssetResolution[item].Count; i++)
                {
                    sb.AppendLine(configTextAssetResolution[item][i]);
                }
            }
            return sb.ToString();
        }
    }
}
