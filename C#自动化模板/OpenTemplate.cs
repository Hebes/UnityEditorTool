/******************************************
	作者：暗沉
	邮箱：空
	日期：2022-12-06 10:49:09
	功能：

	//===============================\
				空
	\===============================//
******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

/// <summary>
/// 是否启用注释头模板文件
/// 参考：https://blog.csdn.net/aaa27987/article/details/119755938
/// 参考：https://www.bilibili.com/read/cv15039402
/// </summary>
public class OpenTemplate : Editor
{
    public static bool isOpenTemplate;
	[MenuItem("Assets/是否启用注释头模板文件(F1) _F1")]
	[MenuItem("Tool/是否启用注释头模板文件")]
	public static void SetOpenTemplate()
    {
		//取反
		isOpenTemplate = !isOpenTemplate; 
		//Unity提供的API，可设置菜单的勾选状态
		Menu.SetChecked("Tool/是否启用注释头模板文件", isOpenTemplate);
		Menu.SetChecked("Assets/是否启用注释头模板文件(F1) _F1", isOpenTemplate);
	}
}


