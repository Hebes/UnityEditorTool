using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVConfig
{
    /// <summary>
    /// csv读取路径
    /// </summary>
    public string readCSVPath { get; set; }
    /// <summary>
    /// 保存文件的文件路径
    /// </summary>
    public string saveFilePath { get; set; }
    /// <summary>
    /// 保存文件的文件夹路径
    /// </summary>
    public string saveDirectoryPath { get; set; }
    /// <summary>
    /// 需要生成的类名
    /// </summary>
    public string className { get; set; }
}

public class CSVClass
{
    //**********************************************外部调用方法**********************************************
    /// <summary>
    /// 生成CSV文件的属性类
    /// </summary>
    /// <param name="Path"></param>
    public static void CSVPropertyClassCreat(CSVConfig cSVConfig)
    {
        string[] textAssetText = CSVReadContent(cSVConfig.readCSVPath);
        string ClassName = CSVGetClassName(textAssetText, 2, 1);
        cSVConfig.className = $"{ClassName}Info";//需要生成的类名
        cSVConfig.saveFilePath = $"{cSVConfig.saveDirectoryPath}/{cSVConfig.className}.cs";//文件路径
        List<List<string>> rowsContents = RedRow(textAssetText, 3, 6);//读取指定数据
        List<List<string>> reRowsContents = CSVDataChange(rowsContents);//数据转换
        string sb = CSVPropertyClass(cSVConfig.className, reRowsContents);//代码模板
        ChickDirectory(cSVConfig.saveDirectoryPath); //检查文件夹路径
        ChickFile(cSVConfig.saveFilePath, sb.ToString());//保存文件
    }

    /// <summary>
    /// CSV文件解析
    /// </summary>
    /// <param name="Path">文件路径</param>
    public static List<T> CSVAnalysis<T>(string Path, int StartRow = 6) where T : new()
    {
        List<T> Temp = new List<T>();
        string[] textAssetText = CSVReadContent(Path);
        for (int i = StartRow; i < textAssetText.Length; i++)
        {
            if (string.IsNullOrEmpty(textAssetText[i])) continue;
            Temp.Add(CSVOneDataAnalysis<T>(textAssetText[i]));
        }
        return Temp;
        #region 旧代码--学习用
        ////解析成字典数据
        //for (int d = 0; d < dataSet.Tables.Count; d++)
        //{
        //    //读取表中的一个工作表的内容
        //    DataTable dataTable = dataSet.Tables[d];
        //    //解析工作表的内容 5 是开始开始的行数
        //    List<ExcelData> excelDatas = ExcelReadData.ParseExcelRow(dataTable, out string tableName, 5, dataTable.Rows.Count);//tableName表名

        //    //方法一
        //    //实例化需要赋值的类
        //    Type charect = Type.GetType(ExcelToAsset.nameSpaceName + "." + tableName);//获取类
        //                                                                              //反射创建列表
        //    Type list = typeof(List<>).MakeGenericType(charect);//创建实力类列表
        //    object charectList = Activator.CreateInstance(list); //实例化列表
        //    MethodInfo addMethod = charectList.GetType().GetMethod("Add");//反射列表的添加方法
        //                                                                  //添加数据
        //    for (int e = 0; e < excelDatas.Count; e++)
        //    {
        //        object instanceClass = Activator.CreateInstance(charect);//实例化类等同new
        //        Type instanceClassType = instanceClass.GetType();//以下是添加数据
        //        FieldInfo[] fieldInfos = instanceClassType.GetFields();
        //        for (int i = 0; i < excelDatas[e].ExcelDataInfo.Count; i++)
        //        {
        //            fieldInfos[i].SetValue(instanceClass, ExcelToAsset.GetTypeForExcel(excelDatas[e].ExcelDataInfo[i], fieldInfos[i].FieldType));
        //        }
        //        addMethod.Invoke(charectList, new object[] { instanceClass });//执行添加方法，并添加数据 charectList需要实例化才能执行方法
        //    }
        //    //执行生成方法
        //    Type charectData = Type.GetType(ExcelToAsset.nameSpaceName + "." + tableName + "AssetData");//获取类
        //    ScriptableObject.CreateInstance(ExcelToAsset.nameSpaceName + "." + tableName + "AssetData");
        //    object charectDataNew = Activator.CreateInstance(charectData);  //实例化反射
        //    MethodInfo methodInfo = charectData.GetMethod("CreatAsset");    //获取方法
        //    methodInfo.Invoke(charectDataNew, new object[] { charectList });//传入参数
        //    sw.Stop();
        //    UnityEngine.Debug.Log($"用时total: {sw.ElapsedMilliseconds} ms");
        //}
        #endregion
    }

    /// <summary>
    /// 一条数据解析
    /// </summary>
    private static T CSVOneDataAnalysis<T>(string textAssetText) where T : new()
    {
        List<string> count = RedOneRow(textAssetText);
        T charect = new T();
        Type type = charect.GetType();
        PropertyInfo[] fieldInfos = type.GetProperties();
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            PropertyInfo item = fieldInfos[i];
            item.SetValue(charect, CSVDataChange(count[i], item.PropertyType));
        }
        return charect;
    }


    //**********************************************内部封装方法**********************************************
    //**********************************************解析类生成方法**********************************************
    #region 解析类生成方法
    /// <summary>
    /// 读取指定数据
    /// </summary>
    /// <param name="textAssetText">文本内容</param>
    /// <param name="start">开始读取的行</param>
    /// <param name="end">结束读取的行</param>
    private static List<List<string>> RedRow(string[] textAssetText, int start, int end)
    {
        List<List<string>> contents = new List<List<string>>();
        for (int i = start; i < end; i++)
            contents.Add(RedOneRow(textAssetText[i]));
        return contents;
    }

    /// <summary>
    /// 读取单行
    /// </summary>
    /// <param name="textAssetText">单行内容</param>
    private static List<string> RedOneRow(string textAssetText)
    {
        List<string> content = new List<string>();
        string tempStr = textAssetText.Replace("\r", "");
        for (int i = 0; i < tempStr.Split(',').Length; i++)
        {
            string temp = tempStr.Split(',')[i];
            if (string.IsNullOrEmpty(temp)) continue;
            content.Add(temp);
        }
        return content;
    }

    /// <summary>
    /// 读取内容
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    private static string[] CSVReadContent(string Path)
    {
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{Path}");
        string[] textAssetText = textAsset.text.Split("\n");
        return textAssetText;
    }

    /// <summary>
    /// 获取类名
    /// </summary>
    /// <param name="textAssetText">文本的内容</param>
    /// <param name="row">第几行</param>
    /// <param name="col">第几列</param>
    /// <returns></returns>
    private static string CSVGetClassName(string[] textAssetText, int row, int col)
    {
        string ClassName = RedOneRow(textAssetText[row])[col];
        return ClassName;
    }

    /// <summary>
    /// 生成属性脚本模板
    /// </summary>
    /// <param name="className">生成的类名</param>
    /// <param name="reRowsContents">转换后的数据</param>
    /// <returns></returns>
    private static string CSVPropertyClass(string className, List<List<string>> reRowsContents)
    {
        //生成类
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("");
        sb.AppendLine("");
        sb.AppendLine("using System.Collections.Generic;\nusing UnityEngine;");
        sb.AppendLine("");
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");
        sb.AppendLine("\t#region 自动化生成文件");
        sb.AppendLine("");
        foreach (var item in reRowsContents)
        {
            sb.AppendLine($"\t/// <summary>{item[0]}</summary>");
            sb.AppendLine($"\tpublic {item[1]} {item[2]} {{ get; set; }}");
            sb.AppendLine("");
        }
        sb.AppendLine("\t#endregion");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>
    /// 数据转换
    /// </summary>
    /// <param name="rowsContents">原版数据</param>
    /// <returns></returns>
    private static List<List<string>> CSVDataChange(List<List<string>> rowsContents)
    {
        //列表重新排列
        List<List<string>> reRowsContents = new List<List<string>>();
        for (int i = 0; i < rowsContents.Count; i++)
        {
            List<string> rowsContentsI = rowsContents[i];
            for (int r = 0; r < rowsContentsI.Count; r++)
            {
                string rowsContentsIR = rowsContentsI[r];
                if (i == 0)
                    reRowsContents.Add(new List<string>() { rowsContentsIR });
                else
                    reRowsContents[r].Add(rowsContentsIR);
            }
        }
        return reRowsContents;
    }
    #endregion

    //**********************************************解析封装方法**********************************************
    #region 解析封装方法
    /// <summary>
    /// 反射数据类型并赋值
    /// </summary>
    /// <param name="str">内容</param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    private static object CSVDataChange(string str, Type type)
    {
        if (type == typeof(int)) { return int.Parse(str); }
        else if (type == typeof(float)) { return float.Parse(str); }
        else if (type == typeof(string)) { return str; }
        else if (type == typeof(bool)) { return str.ToLower() == "ture" ? true : false; }
        else if (type == typeof(List<string>)) { return str.Split('|').ToList(); }
        else if (type == typeof(List<int>))
        {
            List<string> vs = str.Split('|').ToList();
            return vs.Select<string, int>(a => Convert.ToInt32(a)).ToList(); ;
        }
        else if (type == typeof(List<float>))
        {
            List<string> vs = str.Split('|').ToList();
            return vs.Select<string, float>(a => Convert.ToSingle(a)).ToList(); ;
        }
        else if (type == typeof(Sprite))
        {
            //Sprite sprite = null;
            ////编辑器加载图片png
            //string pngPath = string.Format($"{loadImagePath}/{str}.png");
            //sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
            //if (sprite != null)
            //    return sprite;
            ////编辑器加载图片jpg
            //string jpgPath = string.Format($"{loadImagePath}/{str}.jpg");
            //sprite = AssetDatabase.LoadAssetAtPath<Sprite>(jpgPath);
            //if (sprite != null)
            //    return sprite;
            //UnityEngine.Debug.Log(string.Format("<color=#FFFF00>未知类型图片,请在上面代码中添加!</color>"));
            return null;
        }
        else
        {
            Debug.Log(string.Format($"<color=#FFFF00>{type.Name} 未知类型,请添加解析类型!</color>"));
            return null;
        }
    }
    #endregion

    //**********************************************检测文件或者文件夹是否存在**********************************************
    #region 检测文件或者文件夹是否存在
    /// <summary>
    /// 检查文件夹是否存在
    /// </summary>
    /// <param name="DirectoryPath">文件夹路径</param>
    private static void ChickDirectory(string DirectoryPath)
    {
        if (!Directory.Exists(DirectoryPath))//是否存在这个文件
        {
            Directory.CreateDirectory(DirectoryPath);//创建
            UnityEngine.Debug.Log("文件夹创建成功!");
            AssetDatabase.Refresh();//刷新编辑器
        }
    }

    /// <summary>
    /// 检查文件并写入
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="content">需要写入的内容</param>
    private static void ChickFile(string path, string content)
    {
        if (File.Exists(path)) File.Delete(path);
        using (StreamWriter file = File.CreateText(path))
            file.Write(content);
        AssetDatabase.Refresh();//刷新编辑器
        Debug.Log("文件写入成功!");
    }
    #endregion
}
