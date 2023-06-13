using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACProjectToolReName : EditorWindow
    {
        public static string ACProjectReName_Prefix { get; private set; }
        public static Vector2 ACProjectReName_ScrollRoot { get; private set; }
        public static string InputSuffixNumber { get; private set; }
        public static string ACProjectReName_SuffixNumber { get; private set; }
        public static string ACProjectReName_Suffix { get; private set; }
        public static string ACProjectReName_REName { get; private set; }
        public static string ACProjectReName_OldChangeName { get; private set; }
        public static string ACProjectReName_NewChangeName { get; private set; }


        //*******************************前缀*******************************

        public static void ACProjectPrefix()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("Project前缀工具", EditorStyles.boldLabel);
            //******************************添加前缀******************************
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);

            EditorGUILayout.BeginHorizontal();
            {
                ACProjectReName_Prefix = EditorGUILayout.TextField("请输入组件查找前缀", ACProjectReName_Prefix);
                if (GUILayout.Button("复制", EditorStyles.miniButtonMid)) { ACProjectReName_Prefix.ACCopyWord(); }
                if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid)) { ACCoreExpansion_DateSave.ACReAssets(); }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                {
                    ACProjectReName_Prefix = ACCoreExpansion_Find.ACGetObj().ACGetPrefix();
                }
                if (GUILayout.Button("清空前缀", EditorStyles.miniButtonMid))
                {
                    ACProjectReName_Prefix = string.Empty;
                }
                if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid))
                {
                    ProjectObjAddPrefix(ACCoreExpansion_Find.ACGetObjs(), ACProjectReName_Prefix);

                }
                if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid))
                {
                    ProjectObjRemoePrefix(ACCoreExpansion_Find.ACGetObjs(), ACProjectReName_Prefix);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("去除空白", EditorStyles.miniButtonMid)) { ACCoreExpansion_Find.ACGetObjs().ACClearSpecificSymbolLoop("", " "); }

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("T_", EditorStyles.miniButtonMid)) { ACProjectReName_Prefix = "T_"; }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        public static void ProjectObjAddPrefix(UnityEngine.Object[] objs, string prefixName)
        {
            for (int i = 0; i < objs?.Length; i++)
            {
                UnityEngine.Object tempObj = objs[i];
                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                if (name.StartsWith(prefixName))
                    continue;
                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                AssetDatabase.RenameAsset(path_g, prefixName + name);//改名API
            }
            ACCoreExpansion_DateSave.ACReAssets();
        }

        /// <summary>
        /// 移除前缀
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="prefixName"></param>
        public static void ProjectObjRemoePrefix(UnityEngine.Object[] objs, string prefixName)
        {
            for (int i = 0; i < objs?.Length; i++)
            {
                UnityEngine.Object tempObj = objs[i];
                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                if (name.Contains(prefixName))
                {
                    string nam11e = name.Replace(prefixName, "");
                    AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                }
            }
        }

        //*******************************后缀*******************************
        /// <summary>
        /// 添加后缀
        /// </summary>
        public static void ACProjectSuffix()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Unity编辑器Project后缀功能", EditorStyles.boldLabel);
                GUILayout.Space(5f); EditorGUILayout.LabelField("添加后缀(序列号)", EditorStyles.largeLabel);
                //******************************添加后缀(序列号)******************************
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    ACProjectReName_SuffixNumber = EditorGUILayout.TextField("输入后缀", ACProjectReName_SuffixNumber);
                    if (GUILayout.Button("添加后缀(序列号)", EditorStyles.miniButtonMid))
                    {
                        int number = 0;
                        bool result = int.TryParse(ACProjectReName_SuffixNumber, out number); //i now = 108 是否是数字
                        if (!result) { Debug.Log("默认序列号添加"); }
                        Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                        {
                            string name = obj.name.Trim();//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            AssetDatabase.RenameAsset(path_g, name + "_" + number);//改名API
                            number++;
                        });
                        ACCoreExpansion_DateSave.ACReAssets();
                    }
                }
                EditorGUILayout.EndHorizontal();
                //******************************移除后缀******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("移除所有后缀", EditorStyles.largeLabel);
                if (GUILayout.Button("移除所有后缀", EditorStyles.miniButtonMid))
                {
                    if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                    Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                    {
                        string name = obj.name.Trim().Split("_")[0];
                        string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                        AssetDatabase.RenameAsset(path_g, name);//改名API
                    });

                    ACCoreExpansion_DateSave.ACReAssets();
                }
            }
            GUILayout.EndVertical();
        }

        //*******************************修改名称*******************************

        /// <summary>
        /// 修改名称
        /// </summary>
        public static void ACProjectReName()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Unity编辑器Project后缀功能", EditorStyles.boldLabel);
                //******************************重命名******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("重命名", EditorStyles.largeLabel);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    ACProjectReName_REName = EditorGUILayout.TextField("输入新的名称", ACProjectReName_REName);
                    if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                    {
                        if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;

                        for (int i = 0; i < ACCoreExpansion_Find.ACGetObjs().Length; i++)
                        {
                            UnityEngine.Object obj = ACCoreExpansion_Find.ACGetObjs()[i];//单个物品
                            string name = obj.name.Trim();//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            AssetDatabase.RenameAsset(path_g, ACProjectReName_REName + i);//改名API
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                //******************************替换物体名称******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("替换物体名称", EditorStyles.largeLabel);
                ACProjectReName_OldChangeName = EditorGUILayout.TextField("老的名称", ACProjectReName_OldChangeName);
                if (GUILayout.Button("获取老的关键词(请手动删除不需要的)", EditorStyles.miniButtonMid))
                {
                    ACProjectReName_OldChangeName = ACCoreExpansion_Find.ACGetObj().name;
                }
                ACProjectReName_NewChangeName = EditorGUILayout.TextField("输入新的名称", ACProjectReName_NewChangeName);
                if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                {
                    if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                    Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                    {
                        if (obj.name.Contains(ACProjectReName_OldChangeName))
                        {
                            (obj as GameObject).name = obj.name.Replace(ACProjectReName_OldChangeName, ACProjectReName_NewChangeName);

                            string name = obj.name.Trim();//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            string nam11e = name.Replace(ACProjectReName_OldChangeName, "");//清除
                            AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                        }
                    });
                }
                //******************************上一级文件夹名称变成物体名称******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("上一级文件夹名称变成物体名称", EditorStyles.largeLabel);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    ACProjectReName_Suffix = EditorGUILayout.TextField("输入点后缀", ACProjectReName_Suffix);
                    if (GUILayout.Button("上一级文件夹名称变成物体名称", EditorStyles.miniButtonMid))
                    {
                        if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                        Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                        {
                            //单个文件的修改
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            DirectoryInfo direction = new DirectoryInfo(path_g);
                            FileInfo[] files = direction.GetFiles("*");
                            //FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);

                            for (int i = 0; i < files.Length; i++)
                            {
                                //忽略关联文件
                                if (files[i].Name.EndsWith(ACProjectReName_Suffix))
                                {
                                    //Debug.Log("文件名:" + files[i].Name);
                                    //Debug.Log("文件绝对路径:" + files[i].FullName);
                                    //Debug.Log("文件相对路径:" + path_g +"/"+ files[i].Name);
                                    //Debug.Log("文件所在目录:" + files[i].DirectoryName);
                                    AssetDatabase.RenameAsset(path_g + "/" + files[i].Name, obj.name);//改名API
                                }
                            }
                        });
                        ACCoreExpansion_DateSave.ACReAssets();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
