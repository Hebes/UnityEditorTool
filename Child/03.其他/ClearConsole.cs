using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ClearConsole
{
    //private void toClearConsole()
    //{
    //	//获取UnityEditor程序集里面的UnityEditorInternal.LogEntries类型，也就是把关于Console的类提出来
    //	var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
    //	//在logEntries类里面找到名为Clear的方法，且其属性必须是public static的，等同于得到了Console控制台左上角的clear，然后通过Invoke进行点击实现
    //	var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    //	clearMethod.Invoke(null, null);
    //}

    [MenuItem("Assets/暗沉EditorTool/其他/Clear Console(键盘左上角[`]) _`")] // Ctrl + ALT + C 避免与唤出控制台的快捷方式冲突
    public static void ClearConsole1()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
    }
}