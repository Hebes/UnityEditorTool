using ACTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AC
{
    /// <summary>
    /// 建模专用
    /// </summary>
    public class ACModelingEditorTool : EditorWindow
    {
        [MenuItem("Assets/暗沉EditorTool/建模专用/建模专用")]//#E
        public static void GeneratorFindComponentTool()
        {
            GetWindow(typeof(ACModelingEditorTool), false, "建模专用").Show();
        }
        private Vector2 scrollRoot { get; set; }


        private void OnGUI()
        {
            scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
            {
                EditorGUILayout.BeginVertical("box");
                {
                    //******************************移除丢失脚本******************************
                    GUILayout.Space(5f); EditorGUILayout.LabelField("添加到Model文件夹里面:", EditorStyles.largeLabel);
                    if (GUILayout.Button("添加到Model文件夹里面", EditorStyles.miniButtonMid))
                    {
                        UnityEngine.Object[] objs = Selection.objects;
                        //获取当前文件的路径
                        for (int i = 0; i < objs?.Length; i++)
                        {
                            UnityEngine.Object tempObj = objs[i];//单个物品
                            string name = tempObj.name;//去除头尾空白字符串//物品名称
                            string path_g = AssetDatabase.GetAssetPath(tempObj);//获得选中物的路径
                            //拆分路径
                            string[] str = path_g.Split('/');
                            string strName = str[str.Length - 1];//物体的名称
                            string newPath = $"{path_g.Split('.')[1]}/Model";//新的文件夹路径
                            //创建文件夹
                            newPath.ACFolderCreat();
                            //移动物体
                            string strMoveInfo = AssetDatabase.MoveAsset(path_g, $"{newPath}/{strName}");
                            Debug.Log(strMoveInfo);
                            //刷新
                            this.ACReAssets();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView(); //结束滚动视图
        }
    }
}
