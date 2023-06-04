using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Unity 5.6   
/// </summary>
public class EditorGUILayoutOtherExample : EditorWindow
{
    //PropertyField  GetControlRect  PrefixLabel
    [MenuItem("Tools/EditorGUILayout模板/参考样例1")]
    static void Init()
    {
        EditorGUILayoutOtherExample window = (EditorGUILayoutOtherExample)EditorWindow.GetWindow(typeof(EditorGUILayoutOtherExample));
        window.Show();
    }

    #region  DropdownButton
    private string m_itemString = "";
    #endregion

    #region EnumPopup
    public enum OPTIONS
    {
        CUBE = 0,
        SPHERE = 1,
        PLANE = 2
    }
    public OPTIONS op;
    #endregion

    #region IntPopup
    int selectedSize = 1;
    string[] names = new string[] { "Normal", "Double", "Quadruple" };
    int[] sizes = new int[] { 1, 2, 4 };
    #endregion

    #region Popup
    public string[] options = new string[] { "Cube", "Sphere", "Plane" };
    public int index = 0;
    #endregion

    #region EnumMaskPopup
    public enum Options
    {
        CUBE = 0,
        SPHERE = 1,
        PLANE = 2
    }
    public Options m_options;
    #endregion

    #region InspectorTitlebar
    bool fold = true;
    bool fold2 = true;
    Transform selectedTransform;
    GameObject selectedGameObject;
    #endregion

    #region IntSlider
    int m_intSlider = 1;
    #endregion
    #region IntSlider
    float scale = 0.0f;
    #endregion

    #region MinMaxSlider
    float minVal = -10;
    float maxVal = 10;

    float minLimit = -20;
    float maxLimit = 20;
    #endregion

    #region PasswordField
    string m_passwordField = "";
    #endregion

    void OnGUI()
    {
        #region  DropdownButton
        //比较麻烦
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("DropdownButton:");
        if (EditorGUILayout.DropdownButton(new GUIContent(m_itemString), FocusType.Keyboard))
        {
            var alls = new string[4] { "A", "B", "C", "D" };
            GenericMenu _menu = new GenericMenu();
            foreach (var item in alls)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                //添加菜单
                _menu.AddItem(new GUIContent(item), m_itemString.Equals(item), OnValueSelected, item);
            }
            _menu.ShowAsContext();//显示菜单
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region EnumPopup
        op = (OPTIONS)EditorGUILayout.EnumPopup("EnumPopup:", op);
        #endregion

        #region IntPopup
        selectedSize = EditorGUILayout.IntPopup("IntPopup: ", selectedSize, names, sizes);
        #endregion

        #region Popup
        index = EditorGUILayout.Popup("Popup:", index, options);
        #endregion

        #region HelpBox
        EditorGUILayout.HelpBox("HelpBox Error:", MessageType.Error);
        EditorGUILayout.HelpBox("HelpBox Info:", MessageType.Info);
        EditorGUILayout.HelpBox("HelpBox None:", MessageType.None);
        EditorGUILayout.HelpBox("HelpBox Warning:", MessageType.Warning);
        #endregion

        #region InspectorTitlebar
        selectedTransform = Selection.activeGameObject.transform;
        selectedGameObject = Selection.activeGameObject;
        fold = EditorGUILayout.InspectorTitlebar(fold, selectedTransform);
        fold2 = EditorGUILayout.InspectorTitlebar(fold2, selectedGameObject);
        #endregion

        #region IntSlider
        //包括最大最小值
        m_intSlider = EditorGUILayout.IntSlider("IntSlider:", m_intSlider, 1, 10);
        #endregion

        #region MinMaxSlider
        //取值范围
        EditorGUILayout.LabelField("Min Val:", minVal.ToString());
        EditorGUILayout.LabelField("Max Val:", maxVal.ToString());
        EditorGUILayout.MinMaxSlider("MinMaxSlider", ref minVal, ref maxVal, minLimit, maxLimit);
        #endregion
        EditorGUILayout.Space();

        #region PasswordField
        m_passwordField = EditorGUILayout.PasswordField("PasswordField:", m_passwordField);
        EditorGUILayout.LabelField("输入的文本:", m_passwordField);
        #endregion

        #region SelectableLabel
        //可以选择，复制粘贴
        EditorGUILayout.SelectableLabel("SelectableLabel");
        #endregion

        scale = EditorGUILayout.Slider("Slider:", scale, 1, 100);
        //自适应高，不能自适应宽
        m_textArea = EditorGUILayout.TextArea(m_textArea);

        m_vector2 = EditorGUILayout.Vector2Field("Vector2:", m_vector2);
        m_vector3 = EditorGUILayout.Vector3Field("Vector3:", m_vector3);
        m_vector4 = EditorGUILayout.Vector4Field("Vector4:", m_vector4);

    }

    string m_textArea = "";
    Vector2 m_vector2;
    Vector3 m_vector3;
    Vector4 m_vector4;
    #region  DropdownButton
    void OnValueSelected(object value)
    {
        m_itemString = value.ToString();
    }
    #endregion
}
