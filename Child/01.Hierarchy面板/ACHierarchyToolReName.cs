using System;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    重命名

-----------------------*/

namespace ACTool
{
    public class ACHierarchyToolReNameReName : EditorWindow
    {
        /// <summary>
        /// 重命名
        /// </summary>
        public static void OnShow()
        {
            ACHierarchyPrefix();
            ACHierarchySuffix();
        }


        private static Vector2 ACHierarchyToolReName_ScrollRoot { get; set; }//滑动条
        public static string ACHierarchyToolReName_Prefix1 { get; set; }//前缀
        public static string ACHierarchyToolReName_SuffixNumber { get; set; }//末尾编号
        public static string ACHierarchyToolReName_REName { get; set; }//重新的命名
        public static string ACHierarchyToolReName_OldChangeName { get; set; }//替换的命名
        public static string ACHierarchyToolReName_NewChangeName { get; set; }//替换的命名



        public static void ACHierarchyPrefix()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("Hierarchy前缀工具", EditorStyles.boldLabel);
            //******************************添加前缀******************************
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);

            EditorGUILayout.BeginHorizontal();
            {
                ACHierarchyToolReName_Prefix1 = EditorGUILayout.TextField("请输入组件查找前缀", ACHierarchyToolReName_Prefix1);
                if (GUILayout.Button("复制", EditorStyles.miniButtonMid)) { ACHierarchyToolReName_Prefix1.ACCopyWord(); }
                if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid)) { ACCoreExpansion_Find.ACGetObjs().ACSaveModification(); }
                if (GUILayout.Button("清除", EditorStyles.miniButtonMid))
                {
                    ACHierarchyToolReName_Prefix1 = string.Empty;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                {
                    ACHierarchyToolReName_Prefix1 = ACCoreExpansion_Find.ACGetObj().ACGetPrefix();
                }
                if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid))
                {
                    ACCoreExpansion_Find.ACGetObjs().ACChangePrefixLoop(ACHierarchyToolReName_Prefix1);
                }
                if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid))
                {
                    ACCoreExpansion_Find.ACGetObjs().ACChangePrefixLoop(ACHierarchyToolReName_Prefix1, false);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("去除空白", EditorStyles.miniButtonMid)) { ACCoreExpansion_Find.ACGetObjs().ACClearSpecificSymbolLoop("", " "); }

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("T_", EditorStyles.miniButtonMid)) { ACHierarchyToolReName_Prefix1 = "T_"; }
            }
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// 后缀
        /// </summary>
        public static void ACHierarchySuffix()
        {
            ACHierarchyToolReName_ScrollRoot = EditorGUILayout.BeginScrollView(ACHierarchyToolReName_ScrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Unity编辑器Hierarchy后缀功能", EditorStyles.boldLabel);
                    GUILayout.Space(5f); EditorGUILayout.LabelField("添加后缀(序列号)", EditorStyles.largeLabel);
                    //******************************添加后缀(序列号)******************************
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        ACHierarchyToolReName_SuffixNumber = EditorGUILayout.TextField("输入后缀", ACHierarchyToolReName_SuffixNumber);
                        if (GUILayout.Button("添加后缀(序列号)", EditorStyles.miniButtonMid))
                        {
                            int number = 0;
                            bool result = int.TryParse(ACHierarchyToolReName_SuffixNumber, out number); //i now = 108
                            if (!result) { Debug.Log("默认序列号添加"); }
                            Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                            {
                                string name = obj.name.Trim();//去除头尾空白字符串
                                (obj as GameObject).name = $"{name}_{number}";
                                number++;
                            });
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************移除所有后缀******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("移除所有后缀", EditorStyles.largeLabel);
                    if (GUILayout.Button("移除所有后缀", EditorStyles.miniButtonMid))
                    {
                        if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                        Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                        {
                            if (obj.name.Contains("_")) { (obj as GameObject).name = obj.name.Split("_")[0]; }
                        });
                    }
                    //******************************重命名******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("重命名", EditorStyles.largeLabel);
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        ACHierarchyToolReName_REName = EditorGUILayout.TextField("输入新的名称", ACHierarchyToolReName_REName);
                        if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                        {
                            if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                            Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                            {
                                (obj as GameObject).name = ACHierarchyToolReName_REName;
                            });
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //******************************替换物体名称******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("替换物体名称", EditorStyles.largeLabel);
                    ACHierarchyToolReName_OldChangeName = EditorGUILayout.TextField("老的名称", ACHierarchyToolReName_OldChangeName);
                    if (GUILayout.Button("获取老的关键词(请手动删除不需要的)", EditorStyles.miniButtonMid))
                    {
                        ACHierarchyToolReName_OldChangeName = ACCoreExpansion_Find.ACGetObj().name;
                    }
                    ACHierarchyToolReName_NewChangeName = EditorGUILayout.TextField("输入新的名称", ACHierarchyToolReName_NewChangeName);
                    if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                    {
                        if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                        Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                        {
                            if (obj.name.Contains(ACHierarchyToolReName_OldChangeName))
                            {
                                (obj as GameObject).name = obj.name.Replace(ACHierarchyToolReName_OldChangeName, ACHierarchyToolReName_NewChangeName);
                            }
                        });
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }
    }
}