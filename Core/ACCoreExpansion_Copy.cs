using System.Text;
using UnityEngine;

namespace ACTool
{
    public static  class ACCoreExpansion_Copy
    {
        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void ACCopyWord(this string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void ACCopyWord(this StringBuilder str)
        {
            TextEditor te = new TextEditor();
            te.text = str.ToString();
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void ACUnityCopyWord(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void ACUnityCopyWord(this StringBuilder str)
        {
            GUIUtility.systemCopyBuffer = str.ToString();
        }
    }
}
