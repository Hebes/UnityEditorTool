using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

namespace ACTool
{
    public static class ACToolExpansionUnityComponent
    {
        /// <summary>
        /// 去除组件RayCast Target
        /// </summary>
        public static void ClearRayCastTarget(this UnityEngine.Object[] obj)
        {
            foreach (var item in obj)
            {
                GameObject go = item as GameObject;
                if (go.GetComponent<Text>() != null)
                {
                    go.GetComponent<Text>().raycastTarget = false;
                    continue;
                }
                else if (go.GetComponent<Image>())
                {
                    go.GetComponent<Image>().raycastTarget = false;
                    continue;
                }
                else if (go.GetComponent<RawImage>())
                {
                    go.GetComponent<RawImage>().raycastTarget = false;
                    continue;
                }
                if (EditorUtility.DisplayDialog("消息提示", go.name + "没有找到需要去除的RayCast Target选项", "确定")) { }
            }
        }
    }
}
