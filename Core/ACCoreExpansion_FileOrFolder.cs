using System.IO;

namespace ACTool
{
    /// <summary>
    /// 文件或者文件夹
    /// </summary>
    public static class ACCoreExpansion_FileOrFolder
    {
        #region 文件操作

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

        #endregion


        #region 文件夹操作

        /// <summary>
        /// 通过路径检文件夹是否存在，如果不存在则创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static string ACChackFolder(this string folderPath)
        {
            if (!folderPath.ACFolderChackExists())//是否存在这个文件
            {
                Debug.Log("文件夹不存在,正在创建...");
                folderPath.ACCreatFolder();//创建
                Debug.Log("创建成功!");
            }
            return folderPath;
        }

        /// <summary>
        /// 检查文件夹是否存在
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool ACFolderChackExists(this string folderPath)
        {
            return Directory.Exists(folderPath);//是否存在这个文件
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void ACCreatFolder(this string folderPath)
        {
            Directory.CreateDirectory(folderPath);//创建
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

        #endregion

    }
}
