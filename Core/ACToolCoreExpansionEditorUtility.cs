using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace ACTool
{
    public static class ACToolCoreExpansionEditorUtility
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
