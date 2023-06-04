using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LogOpen : Editor
{
    public static bool isLogPrint;
    //[MenuItem("Assets/是否启用Log日志打印(F2) _F2")]
    //[MenuItem("Tool/是否启用Log日志打印(F2) _F2")]
    public static void SetOpenTemplate()
    {
        //取反
        isLogPrint = !isLogPrint;
        int numberBool = isLogPrint == true ? 0 : 1;//0是开启1是关闭
        PlayerPrefs.SetInt("设置日志开启", numberBool);

        //Unity提供的API，可设置菜单的勾选状态
        Menu.SetChecked("Assets/是否启用Log日志打印(F2) _F2", isLogPrint);
        Menu.SetChecked("Tool/是否启用Log日志打印(F2) _F2", isLogPrint);
    }
}
