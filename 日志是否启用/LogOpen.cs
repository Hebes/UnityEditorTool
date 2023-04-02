using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LogOpen : Editor
{
    public static bool isLogPrint;
    [MenuItem("Assets/�Ƿ�����Log��־��ӡ(F2) _F2")]
    [MenuItem("Tool/�Ƿ�����Log��־��ӡ(F2) _F2")]
    public static void SetOpenTemplate()
    {
        //ȡ��
        isLogPrint = !isLogPrint;
        int numberBool = isLogPrint == true ? 0 : 1;//0�ǿ���1�ǹر�
        PlayerPrefs.SetInt("������־����", numberBool);

        //Unity�ṩ��API�������ò˵��Ĺ�ѡ״̬
        Menu.SetChecked("Assets/�Ƿ�����Log��־��ӡ(F2) _F2", isLogPrint);
        Menu.SetChecked("Tool/�Ƿ�����Log��־��ӡ(F2) _F2", isLogPrint);
    }
}
