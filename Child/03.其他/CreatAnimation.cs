﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 创建动画
/// 参考连接：http://www.xuanyusong.com/archives/3243
/// https://blog.csdn.net/somnusand/article/details/72865091
/// </summary>
public class CreatAnimation : Editor
{
    //生成出的Prefab的路径
    //private static string PrefabPath = "Assets/Test/Prefabs";
    //生成出的AnimationController的路径
    //private static string AnimationControllerPath = "Assets/Test/AnimationController";
    //生成出的Animation的路径
    private static string AnimationPath = "Assets/Test/Animation";
    //private static string AnimationPath = "";
    //美术给的原始图片路径
    private static string ImagePath = Application.dataPath + "/Image/SkillImage";
    //private static string ImagePath = Application.dataPath + "";


    [MenuItem("Assets/暗沉EditorTool/其他/批量生成动画")]
    public static void BuildAniamtion()
    {
        DirectoryInfo raw = new DirectoryInfo(ImagePath);
        List<AnimationClip> clips = new List<AnimationClip>();
        foreach (DirectoryInfo dictorys in raw.GetDirectories())//获取每个文件夹的路劲
        {
            clips.Add(BuildAnimationClip(dictorys));
        }
        ////把所有的动画文件生成在一个AnimationController里
        //AnimatorController controller = BuildAnimationController(clips, directories.Name);
        ////最后生成程序用的Prefab文件
        //BuildPrefab(directories, controller);
    }

    private static AnimationClip BuildAnimationClip(DirectoryInfo dictorys)
    {
        string animationName = dictorys.Name;
        //查找所有图片，因为我找的测试动画是.png 
        FileInfo[] images = dictorys.GetFiles("*.png");
        //创建AnimationClip
        AnimationClip _clipTemp = new AnimationClip();
        //动画帧率，30比较合适
        _clipTemp.frameRate = 30;
        EditorCurveBinding _curveBinding = new EditorCurveBinding();
        _curveBinding.type = typeof(SpriteRenderer);
        _curveBinding.path = "";
        _curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] _keyFrames = new ObjectReferenceKeyframe[images.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float _frameTime = 1 / 10f;
        for (int i = 0; i < images.Length; i++)
        {
            Sprite _sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i].FullName));
            _keyFrames[i] = new ObjectReferenceKeyframe();
            _keyFrames[i].time = _frameTime * i;
            _keyFrames[i].value = _sprite;
        }
        //设置idle文件为循环动画
        //设置idle文件为循环动画
        SerializedObject serializedClip = new SerializedObject(_clipTemp);
        AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
        clipSettings.loopTime = true;
        serializedClip.ApplyModifiedProperties();

        string parentName = System.IO.Directory.GetParent(dictorys.FullName).Name;
        System.IO.Directory.CreateDirectory(AnimationPath + "/" + parentName);
        AnimationUtility.SetObjectReferenceCurve(_clipTemp, _curveBinding, _keyFrames);
        AssetDatabase.CreateAsset(_clipTemp, AnimationPath + "/" + parentName + "/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        return _clipTemp;
    }

    private static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return path.Substring(path.IndexOf("Assets\\"));
        }
        else
        {
            return path.Substring(path.IndexOf("Assets/"));
        }
    }

    class AnimationClipSettings
    {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }
}
