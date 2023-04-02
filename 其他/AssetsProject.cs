using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 参考连接：https://blog.csdn.net/liqiangeastsun/article/details/42124283
/// https://blog.csdn.net/LIQIANGEASTSUN/article/details/42173941
/// </summary>
public class AssetsProject : MonoBehaviour
{
    #region 旧版代码
    //[MenuItem("Assets/测试Project目录")]
    //public static void Search()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    // 获取所有选中 文件、文件夹的 GUID
    //    string[] guids = Selection.assetGUIDs;
    //    foreach (var guid in guids)
    //    {
    //        // 将 GUID 转换为 路径
    //        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
    //        // 判断是否文件夹
    //        if (Directory.Exists(assetPath))
    //        {
    //            SearchDirectory(sb, assetPath);
    //        }
    //    }
    //    Debug.Log(sb);
    //}

    //private static string _extension = "*.anim";
    //static void SearchDirectory(StringBuilder sb, string directory)
    //{
    //    DirectoryInfo dInfo = new DirectoryInfo(directory);
    //    // 获取 文件夹以及子文件加中所有扩展名为  _extension 的文件
    //    FileInfo[] fileInfoArr = dInfo.GetFiles(_extension, SearchOption.AllDirectories);
    //    for (int i = 0; i < fileInfoArr.Length; ++i)
    //    {
    //        string fullName = fileInfoArr[i].FullName;
    //        sb.AppendLine(fullName);
    //    }
    //}
    #endregion

    private static string prefabLoadPath = "Prefabs";//Resourcesm目录下的
    private static string ClassName = "ConfigUIPrefab";

    [MenuItem("Assets/生成prefab的配置文件")]
    public static void SearchPrefab()
    {
        //输出内容
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"public static class {ClassName}");
        sb.AppendLine("{");
        sb.AppendLine("\t#region 需要加载的物体的名称");
        Transform[] tf = Resources.LoadAll<Transform>(prefabLoadPath);
        if (tf.Length == 0) { Debug.Log("路径错误"); }
        foreach (var item in tf)
        {
            string itemNameTemp = Regex.Replace(item.name, @"\s", "");//正则表达中，"\s" 是指空白，包括空格、换行、tab缩进等所有的空白。
            sb.AppendLine($"\tpublic static string {itemNameTemp} => \"{item.name}\";");
        }
        sb.AppendLine("\t#endregion");
        sb.AppendLine("}");

        //生成文件的路径
        string filePath = $"{Application.dataPath}/Core/Config/{ClassName}.cs";//Assets/Core/Config
        if (File.Exists(filePath)) { File.Delete(filePath); }
        using (StreamWriter writer = File.CreateText(filePath)) { writer.Write(sb); Debug.Log("内容写入成功!"); }
        //刷新编辑器
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/生成场景配置文件")]//C#获取文件夹下的所有文件的文件名 https://www.cnblogs.com/technology/archive/2011/07/12/2104786.html
    public static void ScenesPrefab()
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
