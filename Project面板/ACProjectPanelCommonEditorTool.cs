using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ACTool
{
    /// <summary>
    /// 暗沉Unity编辑器Project面板通用功能
    /// </summary>
    public class ACProjectPanelCommonEditorTool : EditorWindow
    {

        private Vector2 scrollRoot { get; set; }

        public string InputGameObjectTransform { get; private set; }//输入物体的Transform，就是前缀
        public string InputGameObjectTransformSuffix { get; private set; }//输入物体的Transform，就是后缀
        public string InputGameObjectTransformReName { get; private set; }//输入物体的Transform，就是重命名
        public string InputGameObjectTransformSuffixNumber { get; private set; }//序列号
        public string InputGameObjectTransformClear { get; private set; }//清除


        [MenuItem("Assets/暗沉EditorTool/Project面板/Project面板通用功能")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACProjectPanelCommonEditorTool), false, "ACProject面板通用功能").Show();
        }


        //bool showPosition = true;
        //https://docs.unity.cn/cn/current/ScriptReference/EditorGUILayout.BeginFoldoutHeaderGroup.html
        //https://www.bing.com/search?q=GUILayout+%E6%A0%B7%E5%BC%8F&form=QBLH&sp=-1&lq=0&pq=guilayout+%E6%A0%B7%E5%BC%8F&sc=1-12&qs=n&sk=&cvid=ACDC31C23A7E4C6A83C73648C0D9012D&ghsh=0&ghacc=0&ghpl=
        //https://docs.unity.cn/cn/current/ScriptReference/EditorGUI.html
        //https://blog.csdn.net/e295166319/article/details/52370575
        /// <summary>
        /// 绘制面板
        /// </summary>
        private void OnGUI()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
            {
                //EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "2");
                //{
                //    if (GUILayout.Button("清空输入栏", EditorStyles.miniButtonMid))
                //    {
                //        InputGameObjectTransform = string.Empty;
                //    }

                //    if (!Selection.activeTransform)
                //    {
                //        //status = "Select a GameObject";
                //        showPosition = false;
                //    }
                //}
                //EditorGUILayout.EndFoldoutHeaderGroup();



                //EditorGUILayout.BeginFoldoutHeaderGroup(true, "1");
                //{
                //    if (GUILayout.Button("清空输入栏", EditorStyles.miniButtonMid))
                //    {
                //        InputGameObjectTransform = string.Empty;
                //    }
                //}
                //EditorGUILayout.EndFoldoutHeaderGroup();

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Unity编辑器Project面板通用功能", EditorStyles.boldLabel);
                    //******************************添加前缀******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);
                    InputGameObjectTransform = GUILayout.TextField(InputGameObjectTransform, "BoldTextField");
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("清空输入栏", EditorStyles.miniButtonMid))
                        {
                            InputGameObjectTransform = string.Empty;
                        }

                        if (GUILayout.Button("添加前缀", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] obj = Selection.objects;
                            ProjectObjAddPrefix(obj, InputGameObjectTransform);
                            obj.ACReAssets();
                        }

                        if (GUILayout.Button("移除前缀", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] obj = Selection.objects;
                            ProjectObjRemoePrefix(obj, InputGameObjectTransform);
                            obj.ACReAssets();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    //******************************添加后缀******************************
                    //GUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);

                    //GUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找后缀:", EditorStyles.largeLabel);
                    //InputGameObjectTransformSuffix = GUILayout.TextField(InputGameObjectTransformSuffix, "BoldTextField");

                    //******************************添加后缀(序列号)******************************
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("添加后缀(序列号)", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] objs = Selection.objects;
                            int number = int.Parse(InputGameObjectTransformSuffixNumber);
                            for (int i = 0; i < objs?.Length; i++)
                            {
                                UnityEngine.Object tempObj = objs[i];//单个物品
                                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                                //改后缀
                                //string nam11e = name.Replace(prefixName, "");//替换
                                AssetDatabase.RenameAsset(path_g, name + "_" + number);//改名API
                                number++;
                                //if (name.Contains(prefixName))
                                //{
                                //    string nam11e = name.Replace(prefixName, "");
                                //    AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                                //}
                            }
                            objs.ACReAssets();
                        }
                        InputGameObjectTransformSuffixNumber = GUILayout.TextField(InputGameObjectTransformSuffixNumber, "BoldTextField");
                    }
                    EditorGUILayout.EndHorizontal();

                    //******************************添加后缀******************************
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {

                        if (GUILayout.Button("添加后缀(自定义后缀，没有序列号)", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] objs = Selection.objects;

                            for (int i = 0; i < objs?.Length; i++)
                            {
                                UnityEngine.Object tempObj = objs[i];//单个物品
                                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                                //改后缀
                                //string nam11e = name.Replace(prefixName, "");//替换
                                AssetDatabase.RenameAsset(path_g, name + "_" + InputGameObjectTransformSuffix);//改名API
                                //if (name.Contains(prefixName))
                                //{
                                //    string nam11e = name.Replace(prefixName, "");
                                //    AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                                //}
                            }
                            objs.ACReAssets();
                        }



                        InputGameObjectTransformSuffix = GUILayout.TextField(InputGameObjectTransformSuffix, "BoldTextField");
                    }
                    EditorGUILayout.EndHorizontal();

                    //******************************重命名******************************
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("重命名", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] objs = Selection.objects;

                            for (int i = 0; i < objs?.Length; i++)
                            {
                                UnityEngine.Object tempObj = objs[i];//单个物品
                                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                                                                                    //改后缀
                                AssetDatabase.RenameAsset(path_g, InputGameObjectTransformReName + "_" + i);//改名API
                            }
                            objs.ACReAssets();
                        }
                        InputGameObjectTransformReName = GUILayout.TextField(InputGameObjectTransformReName, "BoldTextField");
                    }
                    EditorGUILayout.EndHorizontal();

                    //******************************清除******************************
                    EditorGUILayout.BeginHorizontal();//开始水平布局
                    {
                        if (GUILayout.Button("清除", EditorStyles.miniButtonMid))
                        {
                            //获取物体
                            UnityEngine.Object[] objs = Selection.objects;

                            for (int i = 0; i < objs?.Length; i++)
                            {
                                UnityEngine.Object tempObj = objs[i];//单个物品
                                string name = tempObj.name.Trim();//去除头尾空白字符串//物品名称
                                string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                                string nam11e = name.Replace(InputGameObjectTransformClear, "");//清除
                                AssetDatabase.RenameAsset(path_g, nam11e);//改名API
                            }
                            objs.ACReAssets();
                        }
                        InputGameObjectTransformClear = GUILayout.TextField(InputGameObjectTransformClear, "BoldTextField");
                    }
                    EditorGUILayout.EndHorizontal();

                    //******************************图片相关******************************
                    //EditorGUILayout.BeginHorizontal();//开始水平布局
                    //{
                    //    //if (GUILayout.Button("获取图片", EditorStyles.miniButtonMid))
                    //    //{
                    //    //    //TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath("Assets/Bundles/test/Armature_Arms_Normal.tif"); // 获取文件
                    //    //    //if (importer.textureType == TextureImporterType.NormalMap)
                    //    //    //{
                    //    //    //    Debug.Log(11);
                    //    //    //}
                    //    //    //importer.textureType = TextureImporterType.Sprite; // 修改属性
                    //    //    //importer.SaveAndReimport(); // 一定要记得写上这句
                    //    //    //return;
                    //    //    //List<Texture> textures = new List<Texture>();
                    //    //    //UnityEngine.Object[] obj = Selection.objects;
                    //    //    //for (int i = 0; i < obj?.Length; i++)
                    //    //    //{
                    //    //    //    if (obj[i].GetType() == typeof(Texture2D))
                    //    //    //    {
                    //    //    //        Texture2D texture2D = (Texture2D)obj[i];
                    //    //    //        TextureImporter.IsDefaultPlatformTextureFormatValid()
                    //    //    //        if (texture2D.text) TextureImporterType.NormalMap
                    //    //    //        {

                    //    //    //        }
                    //    //    //        textures.Add(obj[i] as Texture2D);
                    //    //    //    }
                    //    //    //}
                    //    //    //Debug.Log(111);
                    //    //}

                    //    //if (GUILayout.Button("添加前缀", EditorStyles.miniButtonMid))
                    //    //{
                    //    //    //获取物体
                    //    //    UnityEngine.Object[] obj = Selection.objects;
                    //    //    ProjectObjAddPrefix(obj, InputGameObjectTransform);
                    //    //    Re();
                    //    //}

                    //    //if (GUILayout.Button("移除前缀", EditorStyles.miniButtonMid))
                    //    //{
                    //    //    //获取物体
                    //    //    UnityEngine.Object[] obj = Selection.objects;
                    //    //    ProjectObjRemoePrefix(obj, InputGameObjectTransform);
                    //    //    Re();
                    //    //}
                    //}
                    //EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndVertical(); GUILayout.Space(5f);
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
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
        }

        /// <summary>
        /// 重命名
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
    }
}