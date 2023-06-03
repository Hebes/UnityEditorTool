using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACTool
{
    public static class ACCoreExpansion_Type
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static string ACTypeChange(this string typeStr)
        {
            switch (typeStr)
            {
                case "RectTransform": return "Transform";
                default: return typeStr;
            }
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass(this string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this string className, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this UnityEngine.Object obj, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this string namespaceName) where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }
    }
}
