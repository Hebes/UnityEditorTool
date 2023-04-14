using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public static class ACToolExpansionPrefix
    {
        /// <summary>
        /// 选择的物体添加前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void ACAddPrefixLoop(this UnityEngine.Object[] objs, string prefix)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) => { ACAddPrefixOne(obj as GameObject, prefix); });
        }

        /// <summary>
        /// 添加一个物体的名字前缀
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="prefix"></param>
        public static void ACAddPrefixOne(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name : $"{prefix}{gameObject.name}";
        }

        /// <summary>
        /// 删除前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void ACRemovePrefix(this UnityEngine.Object[] objs, string prefix)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) => { ACRemovePrefixOne(obj as GameObject, prefix); });
        }

        /// <summary>
        /// 删除一个前缀
        /// </summary>
        public static void ACRemovePrefixOne(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name.Replace(prefix, "") : gameObject.name;
        }

        /// <summary>
        /// 获取选择的物品的前缀
        /// </summary>
        public static string ACGetPrefix(this UnityEngine.Object obj)
        {
            return $"{obj.name}";
        }

    }
}
