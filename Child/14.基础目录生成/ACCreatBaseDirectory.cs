using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{

    /// <summary> 创建基础目录 </summary>
    public class ACCreatBaseDirectory : EditorWindow
    {
        //https://community.uwa4d.com/blog/detail?id=1589882891219140610&entrance=0
        List<string> BaseDirectoryList = new List<string>()
        {
            "Animations",//动画相关的部分
            "Audio",//音效相关的部分
            "Music",//音乐相关的部分
            "SFX",//特效音乐相关的部分
            "Materials",//材质相关的部分
            "Models",//模型相关的部分
            "Plugins",//插件
            "ThirdLibrary",//第三方插件
            "Prefabs",//预制件
            "Resources",// 资源，需要动态加载的资源放在这里,打包时在这个文件夹里的所有文件(不管有没有使用)都会全部打包。所以不需要的文件不要放里面.并且打包时会压缩文件减小体积
            "Textures",//纹理相关的部分
            "Images",//美术原始图片
            "Scenes",//场景
            "Other",//其他
            "Scripts",//脚本
            "Editor",//编辑器相关的内容,不参与打包
            "Shaders",//着色器
            "Fonts",//字体
            "StreamingAssets",//StreamingBander放的目录,压缩文件,流式读取文件,生成app,StreamingAssets这个文件夹下的资源也会全都打包在.apk或者.ipa 它和Resources的区别是，Resources会压缩文件，但是它不会压缩原封不动的打包进去。并且它是一个只读的文件夹，就是程序运行时只能读 不能写。
        };

        /// <summary> 生成基础目录 </summary>
        public void OnCreatBaseDirectory()
        {
            BaseDirectoryList?.ForEach(baseDirectory =>
            {
                baseDirectory.ACFileCreat();
            });
        }
    }
}
