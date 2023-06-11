using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MyTools : EditorWindow
{
    static Object[] objs;
    static MyTools window;
    static bool isCreateFolder = false;
    static string folderName;


    Texture2D tex2d;
    public RawImage ri;

    int TexPixelLength = 256;
    Color[,] arrayColor;

    [MenuItem("Assets/批量创建子文件夹")]
    public static void CreateFolder()
    {
        window = (MyTools)GetWindow(typeof(MyTools));
        window.titleContent.text = "批量创建子文件夹";
        window.position = new Rect(PlayerSettings.defaultScreenWidth / 2, PlayerSettings.defaultScreenHeight / 2, 400, 120);
        window.Show();
        isCreateFolder = true;
    }

    private void OnGUI()
    {
        arrayColor = new Color[TexPixelLength, TexPixelLength];
        tex2d = new Texture2D(TexPixelLength, TexPixelLength, TextureFormat.RGB24, true);
        ri.texture = tex2d;
    }
}