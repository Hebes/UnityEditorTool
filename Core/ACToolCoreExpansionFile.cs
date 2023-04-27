using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class ACToolCoreExpansionFile
    {
        /// <summary>
        /// 文件以追加写入的方式
        /// https://wenku.baidu.com/view/a8fdb767fd4733687e21af45b307e87100f6f85b.html
        /// 显示IO异常请在创建文件的时候Close下
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        private static void ACFileWriteContent(this string path, string content)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
            using (FileStream fsWrite = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            }
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void ACCreatScript(string folderPath, string fileName, string content)
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

        /// <summary>
        /// 设置单个资源的ABName
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="path">资源路径</param>
        public static void ACSetABName(this string path, string abName)
        {
            AssetImporter ai = AssetImporter.GetAtPath(path);
            if (ai != null)
                ai.assetBundleName = abName;
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void ACFileDelete(this string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// 创建文件并写入
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="str"></param>
        public static void ACCreateFileText(this string filePath,string str)
        {
            using (StreamWriter writer = File.CreateText(filePath)) { writer.Write(str); Debug.Log("内容写入成功!"); }
        }
    }
}
