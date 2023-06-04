using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ����ģ�崴��C#����
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
                //����ģ���ļ�
                //string templateContent = File.ReadAllText($"{Application.dataPath}/Editor/CSharpAuto/Template.cs");
                string templateContent = File.ReadAllText("Assets/Editor/CSharpAuto/Template.cs");
                //�ļ�����
                templateContent = templateContent.Replace("AuthorName", "����");
                templateContent = templateContent.Replace("null", "��");
                templateContent = templateContent.Replace("CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                templateContent = templateContent.Replace("ģ���ļ�", "");
                string[] str = path.Replace(".cs", "").Split('/');
                templateContent = templateContent.Replace("Template", str[str.Length - 1]);
                //д���ļ�
                File.WriteAllText(path, templateContent);
                //ˢ��
                AssetDatabase.Refresh();
            }
        }
    }
}
