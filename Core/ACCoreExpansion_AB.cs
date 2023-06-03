using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 设置Ab包名称
    /// </summary>
    public static class ACCoreExpansion_AB
    {
        /// <summary>
        /// 设置ab包名称
        /// </summary>
        public static void ACSetAssetBundleName(string path, string assetBundleName)
        {
            // 设置ab包
            AssetImporter assetImporter1 = AssetImporter.GetAtPath(path);
            assetImporter1.assetBundleName = assetBundleName;
            AssetDatabase.Refresh();
            Debug.Log("set assetbundle success!");
        }

        /// <summary>
        /// 设置单个资源的ABName
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="path">资源路径</param>
        public static string ACSetABName(this string path, string abName)
        {
            AssetImporter ai = AssetImporter.GetAtPath(path);
            if (ai != null)
                ai.assetBundleName = abName;
            return abName;
        }
    }
}
