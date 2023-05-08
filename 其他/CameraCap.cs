using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraCap : Editor
{
    //[MenuItem("Assets/暗沉EditorTool/文件操作面板")]//#E
    [MenuItem("Tools/Screenshots/照相机截图测试 &`", false, 0)]
    //[MenuItem("Tools/照相机截图")]
    public static void ScreenShot()
    {
        Camera renderCamera = SceneView.lastActiveSceneView.camera;
        Debug.Log("相机截图");
        CameraCapture(renderCamera, new Rect(0, 0, 1920, 1080), GetFilePath());
    }

    /// <summary>
    /// 对相机拍摄区域进行截图，如果需要多个相机，可类比添加，可截取多个相机的叠加画面
    /// </summary>
    /// <param name="camera">待截图的相机</param>
    /// <param name="width">截取的图片宽度</param>
    /// <param name="height">截取的图片高度</param>
    /// <param name="fileName">文件名</param>
    /// <returns>返回Texture2D对象</returns>
    public static Texture2D CameraCapture(Camera camera, Rect rect, string fileName)
    {
        RenderTexture render = new RenderTexture((int)rect.width, (int)rect.height, -1);//创建一个RenderTexture对象 

        camera.gameObject.SetActive(true);//启用截图相机
        camera.targetTexture = render;//设置截图相机的targetTexture为render
        camera.Render();//手动开启截图相机的渲染

        RenderTexture.active = render;//激活RenderTexture
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象
        tex.ReadPixels(rect, 0, 0);//读取像素
        tex.Apply();//保存像素信息

        camera.targetTexture = null;//重置截图相机的targetTexture
        RenderTexture.active = null;//关闭RenderTexture的激活状态
        Object.Destroy(render);//删除RenderTexture对象

        byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片
        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif

        return tex;//返回Texture2D对象，方便游戏内展示和使用
    }

    /// <summary>
    /// 获取文件完整路径
    /// </summary>
    /// <returns></returns>
    public static string GetFilePath()
    {
        return Application.dataPath + "/截图" + Random.Range(0, 10000) + ".png";
    }
}
