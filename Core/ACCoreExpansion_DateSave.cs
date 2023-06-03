using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ACTool
{
    public static class ACCoreExpansion_DateSave
    {
        /// <summary>
        /// 保存修改
        /// </summary>
        public static void ACSaveModification(this UnityEngine.Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                Undo.RecordObject(obj, obj.name);
                EditorUtility.SetDirty(obj);
            });
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public static void ACSave()
        {
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ACReAssets(this UnityEngine.Object obj)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ACReAssets(this string obj)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ACReAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ACReAssets(this UnityEngine.Object[] objects)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void ACAssetDatabaseRefresh()
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void ACAssetDatabaseRefresh(this UnityEngine.Object obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void ACAssetDatabaseRefresh(this UnityEngine.Object[] obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void ACAssetDatabaseRefresh(this GameObject obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void ACAssetDatabaseRefresh(this string folderPath)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }
    }
}
