using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace ACTool
{
    /// <summary>
    /// UI组件查找
    /// </summary>
    public class UIFindComponent : Editor
    {
        /// <summary>
        /// 查找组件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Component>> FindComponents1(GameObject obj, string keyValue)
        {
            Dictionary<string, List<Component>> controlDic = new Dictionary<string, List<Component>>();

            //查找组件
            FindChildrenControl1<Button>(obj, controlDic, keyValue);
            FindChildrenControl1<Image>(obj, controlDic, keyValue);
            FindChildrenControl1<Text>(obj, controlDic, keyValue);
            FindChildrenControl1<Toggle>(obj, controlDic, keyValue);
            FindChildrenControl1<Slider>(obj, controlDic, keyValue);
            FindChildrenControl1<ScrollRect>(obj, controlDic, keyValue);
            FindChildrenControl1<InputField>(obj, controlDic, keyValue);
            FindChildrenControl1<Transform>(obj, controlDic, keyValue);
            FindChildrenControl1<ToggleGroup>(obj, controlDic, keyValue);
            FindChildrenControl1<Dropdown>(obj, controlDic, keyValue);
            FindChildrenControl1<CanvasGroup>(obj, controlDic, keyValue);
            return controlDic;
        }

        /// <summary>
        /// 找到子对象的对应控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void FindChildrenControl1<T>(GameObject gameObject, Dictionary<string, List<Component>> controlDic, string keyValue) where T : Component
        {
            T[] controls = gameObject.GetComponentsInChildren<T>();
            string objType = controls.Length == 0 ? string.Empty : controls[0].GetType().Name;//获取组件类型字符串
            for (int i = 0; i < controls?.Length; i++)
            {
                string objName = controls[i].name;//获取组件的名称
                if (!objName.StartsWith(keyValue)) //keyValue = V_
                    continue;
                if (controlDic.ContainsKey(objType))//字典里面有这个组件
                    controlDic[objType].Add(controls[i]);
                else
                    controlDic.Add(objType, new List<Component>() { controls[i] });
            }
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        public static string ClearSpecificSymbol(string str)
        {
            return str.Replace(" ", "").Replace("(", "").Replace(")", "").Trim();//组件名称,順便出去空白
        }
    }
}