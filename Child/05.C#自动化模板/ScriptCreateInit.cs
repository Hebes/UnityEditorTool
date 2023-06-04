using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 按照模板创建C#代码
/// </summary>
public class ScriptCreateInit : AssetModificationProcessor
{

    private static void OnWillCreateAsset(string path)
    {
        if (OpenTemplate.isOpenTemplate)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                //加载模板文件
                //string templateContent = File.ReadAllText($"{Application.dataPath}/Editor/CSharpAuto/Template.cs");
                string templateContent = File.ReadAllText("Assets/Editor/CSharpAuto/Template.cs");
                //文件内容
                templateContent = templateContent.Replace("AuthorName", "暗沉");
                templateContent = templateContent.Replace("null", "空");
                templateContent = templateContent.Replace("CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                templateContent = templateContent.Replace("模板文件", "");
                string[] str = path.Replace(".cs", "").Split('/');
                templateContent = templateContent.Replace("Template", str[str.Length - 1]);
                //写入文件
                File.WriteAllText(path, templateContent);
                //刷新
                AssetDatabase.Refresh();
            }
        }
    }
}
