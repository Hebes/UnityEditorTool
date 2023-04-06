using System;
using System.Data;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CSVAnalysisWindow : EditorWindow
{
    public static string LoadCSVFilePath { get; private set; }
    public bool isSelectFile { get; private set; }
    public string LoadCSVDirectoryPath { get; private set; }

    //[MenuItem("Assets/csv解析窗口(Shift+A) #A")]
    //[MenuItem("Tool/自动化工具窗口窗口(Shift+A) #A", false, 0)]
    public static void ExcelChangeWindow() => EditorWindow.GetWindow(typeof(CSVAnalysisWindow), false, "csv解析").Show();

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
        {
            //**************************csv解析**************************
            GUILayout.Space(5f); EditorGUILayout.LabelField("csv解析", EditorStyles.label);
            EditorGUILayout.BeginHorizontal("box");//解析单个CSV文件
            {
                //选择CSV文件 路径版
                EditorGUILayout.LabelField("选择的csv文件路径:", EditorStyles.label, GUILayout.Width(130));
                LoadCSVFilePath = EditorGUILayout.TextField(LoadCSVFilePath);
                if (GUILayout.Button("Browse...", GUILayout.Width(80f))) { BrowseLoadFilePanel(); }
                //选择加载文件 选择版
                isSelectFile = EditorGUILayout.ToggleLeft("点击加载文件路径", isSelectFile, GUILayout.Width(130f));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal("box");
            {
                //选择CSV文件夹
                EditorGUILayout.LabelField("选择存放csv的文件夹:", EditorStyles.label, GUILayout.Width(130));
                //LoadCSVDirectoryPath = EditorGUILayout.TextField(LoadCSVDirectoryPath);
                //if (GUILayout.Button("Browse...", GUILayout.Width(80f))) { BrowseLoadFilePanel(); }
                //选择加载文件 选择版
                //isSelectFile = EditorGUILayout.ToggleLeft("点击加载文件路径", isSelectFile, GUILayout.Width(130f));
            }
            EditorGUILayout.EndHorizontal();
            ClickFileLoadPath();
            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("生成CSV解析类", GUILayout.Width(110f)))
                {
                    CSVClass.CSVPropertyClassCreat(new CSVConfig()
                    {
                        readCSVPath = $"Assets/{LoadCSVFilePath.Split("Assets")[1]}",
                        saveDirectoryPath = $"{Application.dataPath}/Core/DataAPI",
                    });
                }
                if (GUILayout.Button("解析CSV", GUILayout.Width(110f)))
                {
                    //CSVClass.CSVAnalysis<Charect>($"Assets/{LoadCSVPath.Split("Assets")[1]}");
                }
                if (GUILayout.Button("Excel转换CSV", GUILayout.Width(110f)))
                {
                    
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }



    /// <summary>
    /// 点击文件加载路径,Unity专用
    /// </summary>
    private void ClickFileLoadPath()
    {
        if (isSelectFile == false) return;
        if (Selection.activeObject == null) return;
        SelectionChange();
        string path;
        path = AssetDatabase.GetAssetPath(Selection.activeObject);//选择的文件的路径 
        if (path.Contains("csv") || path.Contains("txt"))
        {
            path = path.Split("Assets")[1];
            LoadCSVFilePath = string.Format($"{Application.dataPath}{path}");
        }
        else
        {
            EditorGUILayout.HelpBox("请选择正确的", MessageType.Info, true);
        }
    }

    /// <summary>
    /// 打开文件
    /// </summary>
    private void BrowseLoadFilePanel()
    {
        string directory = EditorUtility.OpenFilePanel("选择Execl文件", LoadCSVFilePath, "xls,xlsx,csv");
        if (!string.IsNullOrEmpty(directory))
        {
            LoadCSVFilePath = directory;
            //GUIUtility.keyboardControl = 0;//失去焦点
            GUI.FocusControl(null);//失去焦点
        }
    }

    /// <summary>
    /// 选择立刻刷新显示
    /// </summary>
    private void SelectionChange() => Repaint();

    /// <summary>
    /// 开启窗口的重绘，不然窗口信息不会刷新
    /// </summary>
    void OnInspectorUpdate() => Repaint();

    

}
