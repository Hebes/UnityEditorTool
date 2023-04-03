using System;
using UnityEngine;

namespace ACTool
{
    public static  class ACToolExpansionTrim
    {
        /// <summary>
        /// 去除空白
        /// </summary>
        public static void ClearTrim(this UnityEngine.Object[] objs)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) => 
            {
                GameObject go = obj as GameObject;
                go.name = ClearSpecificSymbol(go.name);
            });
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearSpecificSymbol(this string str)
        {
            return str.Replace(" ", "").Replace("(", "").Replace(")", "").Trim();//组件名称,順便出去空白
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyWord1"></param>
        /// <returns></returns>
        public static string ClearSpecificSymbol(this string str, string keyWord1)
        {
            return str.Replace(" ", "").Replace(keyWord1, "").Trim();//组件名称,順便出去空白
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyWord1"></param>
        /// <param name="keyWord2"></param>
        /// <returns></returns>
        public static string ClearSpecificSymbol(this string str,string keyWord1, string keyWord2)
        {
            return str.Replace(" ", "").Replace(keyWord1, "").Replace(keyWord2, "").Trim();//组件名称,順便出去空白
        }
    }
}
