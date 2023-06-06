using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [MenuItem("Assets/暗沉EditorTool/其他/切换到01.Init场景(F4) _F4")]
    public static void Chanage()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/01.Init.unity");
    }
}
