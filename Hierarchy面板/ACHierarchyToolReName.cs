using ACTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 重命名
/// </summary>
public class ACHierarchyToolReName : EditorWindow
{
    [MenuItem("Assets/暗沉EditorTool/Hierarchy面板/Hierarchy面板通用功能/重命名")]//#E
    public static void GeneratorFindComponentTool()
    {
        GetWindow(typeof(ACHierarchyToolReName), false, "Hierarchy面板重命名").Show();
    }

    private void OnGUI()
    {
        ACHierarchyReNameTool();
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

    private static Vector2 ACHierarchyTool_ScrollRoot { get; set; }//滑动条
    public static string ACHierarchyTool_Prefix1 { get; set; }//前缀
    public static string ACHierarchyTool_SuffixNumber { get; set; }//末尾编号
    public static string ACHierarchyTool_REName { get; set; }//重新的命名
    public static string ACHierarchyTool_OldChangeName { get; set; }//替换的命名
    public static string ACHierarchyTool_NewChangeName { get; set; }//替换的命名


    public static void ACHierarchyReNameTool()
    {
        ACHierarchyTool_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyTool_ScrollRoot); //开启滚动视图
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Unity编辑器Hierarchy面板通用功能", EditorStyles.boldLabel);
                //******************************添加前缀******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                ACHierarchyTool_Prefix1 = EditorGUILayout.TextField("请输入前缀", ACHierarchyTool_Prefix1);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    if (GUILayout.Button("清空输入栏", EditorStyles.miniButtonMid)) { ACHierarchyTool_Prefix1 = string.Empty; }
                    if (GUILayout.Button("添加前缀", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetObjs().ACAddPrefixLoop($"{ACHierarchyTool_Prefix1}"); }
                    if (GUILayout.Button("移除前缀", EditorStyles.miniButtonMid)) { ACToolExpansionFind.ACGetObjs().ACRemovePrefix($"{ACHierarchyTool_Prefix1}"); }
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("T_", EditorStyles.miniButtonMid)) { ACHierarchyTool_Prefix1 = "T_"; }
                //******************************添加后缀(序列号)******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("添加后缀(序列号)", EditorStyles.largeLabel);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    if (GUILayout.Button("添加后缀(序列号)", EditorStyles.miniButtonMid))
                    {
                        int number = 0;
                        bool result = int.TryParse(ACHierarchyTool_SuffixNumber, out number); //i now = 108
                        if (!result) { Debug.Log("默认序列号添加"); }
                        Array.ForEach(ACToolExpansionFind.ACGetObjs(), (obj) =>
                        {
                            string name = obj.name.Trim();//去除头尾空白字符串
                            (obj as GameObject).name = $"{name}_{number}";
                            number++;
                        });
                    }
                    ACHierarchyTool_SuffixNumber = EditorGUILayout.TextField("输入后缀", ACHierarchyTool_SuffixNumber);
                }
                EditorGUILayout.EndHorizontal();
                //******************************移除所有后缀******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("移除所有后缀", EditorStyles.largeLabel);
                if (GUILayout.Button("移除所有后缀", EditorStyles.miniButtonMid))
                {
                    if (ACToolExpansionFind.ACGetObjs().Length == 0) return;
                    Array.ForEach(ACToolExpansionFind.ACGetObjs(), (obj) =>
                    {
                        if (obj.name.Contains("_")) { (obj as GameObject).name = obj.name.Split("_")[0]; }
                    });
                }
                //******************************重命名******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("重命名", EditorStyles.largeLabel);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                    {
                        if (ACToolExpansionFind.ACGetObjs().Length == 0) return;
                        Array.ForEach(ACToolExpansionFind.ACGetObjs(), (obj) =>
                        {
                            (obj as GameObject).name = ACHierarchyTool_REName;
                        });
                    }
                    ACHierarchyTool_REName = EditorGUILayout.TextField("输入新的名称", ACHierarchyTool_REName);
                }
                EditorGUILayout.EndHorizontal();
                //******************************替换物体名称******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("替换物体名称", EditorStyles.largeLabel);
                ACHierarchyTool_OldChangeName = EditorGUILayout.TextField("老的名称", ACHierarchyTool_OldChangeName);
                if (GUILayout.Button("获取老的关键词(请手动删除不需要的)", EditorStyles.miniButtonMid))
                {
                    ACHierarchyTool_OldChangeName = ACToolExpansionFind.ACGetObj().name;
                }
                ACHierarchyTool_NewChangeName = EditorGUILayout.TextField("输入新的名称", ACHierarchyTool_NewChangeName);
                if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                {
                    if (ACToolExpansionFind.ACGetObjs().Length == 0) return;
                    Array.ForEach(ACToolExpansionFind.ACGetObjs(), (obj) =>
                    {
                        if (obj.name.Contains(ACHierarchyTool_OldChangeName))
                        {
                            (obj as GameObject).name = obj.name.Replace(ACHierarchyTool_OldChangeName, ACHierarchyTool_NewChangeName);
                        }
                    });
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView(); //结束滚动视图
    }
}
