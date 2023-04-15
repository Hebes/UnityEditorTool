using System;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 去除空白
    /// </summary>
    public static  class ACToolCoreExpansionTrim
    {
        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyWord1"></param>
        /// <param name="keyWord2"></param>
        /// <returns></returns>
        public static string ClearSpecificSymbol(this string str, params string[] strings)
        {
            if (strings == null || strings.Length == 0) return str;
            Array.ForEach(strings, (s) => 
            {
                str = str.Replace(s, "");
            });
            return str.Trim();
        }

        /// <summary>
        /// 去除指定内容
        /// </summary>
        public static void ClearTrims(this UnityEngine.Object[] objs, params string[] strings)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) =>
            {
                obj.name = obj.name.ClearSpecificSymbol(strings);
            });
        }
    }
}
