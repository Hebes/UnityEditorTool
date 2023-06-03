using System;
using System.IO;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public static class ACCoreExpansion_Folder
    {
        /// <summary>
        /// 通过路径检文件夹是否存在，如果不存在则创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static string ACChackFolder(this string folderPath)
        {
            if (!Directory.Exists(folderPath))//是否存在这个文件
            {
                Debug.Log("文件夹不存在,正在创建...");
                Directory.CreateDirectory(folderPath);//创建
                //AssetDatabase.Refresh();//刷新编辑器
                Debug.Log("创建成功!");
            }
            return folderPath;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public static bool ACFolderExist(this string folderPath, Action<bool> action = null)
        {
            action?.Invoke(Directory.Exists(folderPath));
            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatCSharpScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            string filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using (StreamWriter writer = File.CreateText(filePath))//生成文件
                {
                    writer.Write(content);
                    Debug.Log("内容写入成功!");
                }
            }
            folderPath.ACAssetDatabaseRefresh();
        }
    }
}
