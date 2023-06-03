using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACTool
{
    /// <summary>
    /// 字符串修改
    /// </summary>
    public static class ACCoreExpansion_String
    {
        /// <summary>
        /// 添加一个物体名字前缀
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="prefix">前缀</param>
        private static void ACAddPrefixOne(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name : $"{prefix}{gameObject.name}";
        }

        /// <summary>
        /// 删除一个物体名字前缀
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="prefix">前缀</param>
        private static void ACRemovePrefixOne(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name.Replace(prefix, "") : gameObject.name;
        }


        /// <summary>
        /// 循环删除或者添加物体前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void ACChangePrefixLoop(this UnityEngine.Object[] objs, string prefix, bool isAdd = true)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) =>
            {
                if (isAdd)
                    ACAddPrefixOne(obj as GameObject, prefix);
                else
                    ACRemovePrefixOne(obj as GameObject, prefix);
            });
        }

        /// <summary>
        /// 获取选择的物品的名称
        /// </summary>
        public static string ACGetPrefix(this UnityEngine.Object obj)
        {
            return $"{obj.name}";
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyWord1"></param>
        /// <param name="keyWord2"></param>
        /// <returns></returns>
        public static string ACClearSpecificSymbolOne(this string str, string newString, params string[] strings)
        {
            if (strings == null || strings.Length == 0) return str;
            Array.ForEach(strings, (s) =>
            {
                str = str.Replace(s, newString);
            });
            return str.Trim();
        }

        /// <summary>
        /// 去除指定内容
        /// </summary>
        public static void ACClearSpecificSymbolLoop(this UnityEngine.Object[] objs, string newString, params string[] strings)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) => { obj.name = obj.name.ACClearSpecificSymbolOne(newString, strings); });
        }
    }
}
