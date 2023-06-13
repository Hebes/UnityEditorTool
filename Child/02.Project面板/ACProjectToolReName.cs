using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public class ACProjectToolReName : EditorWindow
    {
        private static string Prefix { get; set; }
        private static string SuffixNumber { get; set; }
        private static string Suffix { get; set; }
        private static string REName { get; set; }
        private static string OldChangeName { get; set; }
        private static string NewChangeName { get; set; }

        /// <summary>
        /// 重命名显示
        /// </summary>
        public static void OnShow()
        {
            ACProjectPrefix();
            ACProjectSuffix();
            ACProjectReName();
        }

        //*******************************前缀*******************************

        public static void ACProjectPrefix()
        {
            EditorGUILayout.Space(5f);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Project前缀工具", EditorStyles.boldLabel);
                EditorGUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                {
                    Prefix = EditorGUILayout.TextField("请输入组件查找前缀", Prefix);
                    if (GUILayout.Button("复制", EditorStyles.miniButtonMid)) { Prefix.ACCopyWord(); }
                    if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid)) { ACCoreExpansion_DateSave.ACReAssets(); }
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                    {
                        Prefix = ACCoreExpansion_Find.ACGetObj().ACGetPrefix();
                    }
                    if (GUILayout.Button("清空前缀", EditorStyles.miniButtonMid))
                    {
                        Prefix = string.Empty;
                    }
                    if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid))
                    {
                        ProjectObjAddPrefix(ACCoreExpansion_Find.ACGetObjs(), Prefix);

                    }
                    if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid))
                    {
                        ProjectObjRemoePrefix(ACCoreExpansion_Find.ACGetObjs(), Prefix);
                    }
                }
                EditorGUILayout.EndHorizontal();


                if (GUILayout.Button("去除空白", EditorStyles.miniButtonMid)) { ACCoreExpansion_Find.ACGetObjs().ACClearSpecificSymbolLoop("", " "); }

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("T_", EditorStyles.miniButtonMid)) { Prefix = "T_"; }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
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
                    SuffixNumber = EditorGUILayout.TextField("输入后缀", SuffixNumber);
                    if (GUILayout.Button("添加后缀(序列号)", EditorStyles.miniButtonMid))
                    {
                        int number = 0;
                        bool result = int.TryParse(SuffixNumber, out number); //i now = 108 是否是数字
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
                    REName = EditorGUILayout.TextField("输入新的名称", REName);
                    if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                    {
                        if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;

                        for (int i = 0; i < ACCoreExpansion_Find.ACGetObjs().Length; i++)
                        {
                            UnityEngine.Object obj = ACCoreExpansion_Find.ACGetObjs()[i];//单个物品
                            string name = obj.name.Trim();//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            AssetDatabase.RenameAsset(path_g, REName + i);//改名API
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                //******************************替换物体名称******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("替换物体名称", EditorStyles.largeLabel);
                OldChangeName = EditorGUILayout.TextField("老的名称", OldChangeName);
                if (GUILayout.Button("获取老的关键词(请手动删除不需要的)", EditorStyles.miniButtonMid))
                {
                    OldChangeName = ACCoreExpansion_Find.ACGetObj().name;
                }
                NewChangeName = EditorGUILayout.TextField("输入新的名称", NewChangeName);
                if (GUILayout.Button("物体重命名", EditorStyles.miniButtonMid))
                {
                    if (ACCoreExpansion_Find.ACGetObjs().Length == 0) return;
                    Array.ForEach(ACCoreExpansion_Find.ACGetObjs(), (obj) =>
                    {
                        if (obj.name.Contains(OldChangeName))
                        {
                            (obj as GameObject).name = obj.name.Replace(OldChangeName, NewChangeName);

                            string name = obj.name.Trim();//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(obj);//获得选中物的路径
                            string nam11e = name.Replace(OldChangeName, "");//清除
                            AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                        }
                    });
                }
                //******************************上一级文件夹名称变成物体名称******************************
                GUILayout.Space(5f); EditorGUILayout.LabelField("上一级文件夹名称变成物体名称", EditorStyles.largeLabel);
                EditorGUILayout.BeginHorizontal();//开始水平布局
                {
                    Suffix = EditorGUILayout.TextField("输入点后缀", Suffix);
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
                                if (files[i].Name.EndsWith(Suffix))
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
