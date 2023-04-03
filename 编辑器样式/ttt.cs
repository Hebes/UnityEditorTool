﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ACTool
{
    public sealed class GUIStyle : EditorWindow
    {
        [MenuItem("SKFramework/Editor/GUI/GUI Style")]
        private static void OpenGUIStyle()
        {
            GetWindow<GUIStyle>().Show();
        }

        private UnityEngine.GUIStyle[] styles;
        private Vector2 scroll = Vector2.zero;
        private string searchContent = "";

        private void OnGUI()
        {
            if (styles == null)
            {
                styles = GUI.skin.customStyles;
            }
            GUILayout.BeginHorizontal("Toolbar");
            {
                GUILayout.Label("Search:", GUILayout.Width(50));
                searchContent = GUILayout.TextField(searchContent, "SearchTextField");
            }
            GUILayout.EndHorizontal();

            scroll = GUILayout.BeginScrollView(scroll);
            {
                for (int i = 0; i < styles.Length; i++)
                {
                    if (styles[i].name.ToLower().Contains(searchContent.ToLower()))
                    {
                        GUILayout.BeginHorizontal("Badge");
                        {
                            if (GUILayout.Button("拷贝", "LargeButton", GUILayout.Width(40f)))
                            {
                                EditorGUIUtility.systemCopyBuffer = styles[i].name;
                                UnityEngine.Debug.Log($"拷贝名称: {styles[i].name}");
                            }
                            EditorGUILayout.SelectableLabel(styles[i].name, GUILayout.Width((position.width - 40f) * 0.3f));
                            GUILayout.Button(string.Empty, styles[i], GUILayout.Width((position.width - 40f) * 0.6f));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndScrollView();
        }
    }
}
