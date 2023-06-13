using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    /// <summary> ��������Ŀ¼ </summary>
    public class ACCreatBaseDirectory : EditorWindow
    {
        //https://community.uwa4d.com/blog/detail?id=1589882891219140610&entrance=0
        private static List<string> BaseDirectoryList = new List<string>()
        {
            "Animations:������صĲ���",
            "Audio:��Ч��صĲ���",
            "Music:������صĲ���",
            "SFX:��Ч������صĲ���",
            "Materials:������صĲ���",
            "Models:ģ����صĲ���",
            "Plugins:���",
            "ThirdLibrary:���������",
            "Prefabs:Ԥ�Ƽ�",
            "Resources:��Դ����Ҫ��̬���ص���Դ��������,���ʱ������ļ�����������ļ�(������û��ʹ��)����ȫ����������Բ���Ҫ���ļ���Ҫ������.���Ҵ��ʱ��ѹ���ļ���С���",
            "Textures:������صĲ���",
            "Images:����ԭʼͼƬ",
            "Scenes:����",
            "Other:����",
            "Scripts:�ű�",
            "Editor:�༭����ص�����,��������",
            "Shaders:��ɫ��",
            "Fonts:����",
            "StreamingAssets:StreamingBander�ŵ�Ŀ¼,ѹ���ļ�,��ʽ��ȡ�ļ�,����app,StreamingAssets����ļ����µ���ԴҲ��ȫ�������.apk����.ipa ����Resources�������ǣ�Resources��ѹ���ļ�������������ѹ��ԭ�ⲻ���Ĵ����ȥ����������һ��ֻ�����ļ��У����ǳ�������ʱֻ�ܶ� ����д",
        };

        public static Vector2 V2Scroll { get; private set; }

        /// <summary> ���ɻ���Ŀ¼ </summary>
        public static void OnShow()
        {
            GUILayout.Space(5);
            //V2Scroll = GUILayout.BeginScrollView(V2Scroll);
            if (GUILayout.Button("��������", GUILayout.Width(150)))
            {
                BaseDirectoryList?.ForEach(baseDirectory =>
                {
                    baseDirectory.ACFileCreat();
                });
            }

            EditorGUILayout.LabelField("�������");
            BaseDirectoryList?.ForEach(baseDirectory =>
            {
                string[] strs = baseDirectory.Split(':');
                EditorGUILayout.BeginHorizontal();
                string path = $"{Application.dataPath}/{strs[0]}";
                if (GUILayout.Button(strs[0], GUILayout.Width(150)))
                    path.ACFolderCreat();
                EditorGUILayout.LabelField(strs[1]);
                EditorGUILayout.EndHorizontal();

            });
            //GUILayout.EndScrollView();
        }
    }
}
