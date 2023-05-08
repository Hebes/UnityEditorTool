using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraCap : Editor
{
    //[MenuItem("Assets/����EditorTool/�ļ��������")]//#E
    [MenuItem("Tools/Screenshots/�������ͼ���� &`", false, 0)]
    //[MenuItem("Tools/�������ͼ")]
    public static void ScreenShot()
    {
        Camera renderCamera = SceneView.lastActiveSceneView.camera;
        Debug.Log("�����ͼ");
        CameraCapture(renderCamera, new Rect(0, 0, 1920, 1080), GetFilePath());
    }

    /// <summary>
    /// ���������������н�ͼ�������Ҫ���������������ӣ��ɽ�ȡ�������ĵ��ӻ���
    /// </summary>
    /// <param name="camera">����ͼ�����</param>
    /// <param name="width">��ȡ��ͼƬ���</param>
    /// <param name="height">��ȡ��ͼƬ�߶�</param>
    /// <param name="fileName">�ļ���</param>
    /// <returns>����Texture2D����</returns>
    public static Texture2D CameraCapture(Camera camera, Rect rect, string fileName)
    {
        RenderTexture render = new RenderTexture((int)rect.width, (int)rect.height, -1);//����һ��RenderTexture���� 

        camera.gameObject.SetActive(true);//���ý�ͼ���
        camera.targetTexture = render;//���ý�ͼ�����targetTextureΪrender
        camera.Render();//�ֶ�������ͼ�������Ⱦ

        RenderTexture.active = render;//����RenderTexture
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//�½�һ��Texture2D����
        tex.ReadPixels(rect, 0, 0);//��ȡ����
        tex.Apply();//����������Ϣ

        camera.targetTexture = null;//���ý�ͼ�����targetTexture
        RenderTexture.active = null;//�ر�RenderTexture�ļ���״̬
        Object.Destroy(render);//ɾ��RenderTexture����

        byte[] bytes = tex.EncodeToPNG();//���������ݣ�ת����һ��pngͼƬ
        System.IO.File.WriteAllBytes(fileName, bytes);//д������
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif

        return tex;//����Texture2D���󣬷�����Ϸ��չʾ��ʹ��
    }

    /// <summary>
    /// ��ȡ�ļ�����·��
    /// </summary>
    /// <returns></returns>
    public static string GetFilePath()
    {
        return Application.dataPath + "/��ͼ" + Random.Range(0, 10000) + ".png";
    }
}
