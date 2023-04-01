using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACTool
{
    public static  class ACToolExpansionCopy
    {
        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void CopyWord(this string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void CopyWord(this StringBuilder str)
        {
            TextEditor te = new TextEditor();
            te.text = str.ToString();
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void UnityCopyWord(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void UnityCopyWord(this StringBuilder str)
        {
            GUIUtility.systemCopyBuffer = str.ToString();
        }
    }
}
