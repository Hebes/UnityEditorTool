using System.IO;
using UnityEditor;

namespace ACTool
{
    public static class ACCoreExpansion_EditorUtility
    {
        /// <summary>
        /// 打开路径
        /// </summary>
        /// <param name="folderPath">路径</param>
        public static void ACOpenPath(this string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            EditorUtility.RevealInFinder(folderPath);
        }
    }
}
