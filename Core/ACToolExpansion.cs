using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Progress;

namespace ACTool
{
    /// <summary>
    /// 暗沉工具拓展
    /// </summary>
    public static class ACToolExpansion
    {
        #region 查找物体
        /// <summary>
        /// 获取返回的
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] ACGetSelection()
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取返回的(一个)
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object ACGetSelectionOne()
        {
            return Selection.objects.First();
        }

        /// <summary>
        /// 获取返回的
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> ACGetSelectionGos()
        {
            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < Selection.objects?.Length; i++)
                gos.Add((GameObject)Selection.objects[i]);
            return gos;
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static void ACLoopGetKeyValueGO(this Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).ACLoopGetKeyValueGO(keyValue, ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="goList"></param>
        public static void ACLoopGetAllGO(this Transform transform, ref List<GameObject> goList)
        {
            goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).ACLoopGetAllGO(ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="transformPrefix"></param>
        /// <param name="gameObjects"></param>
        public static void LoopGetAllGO(this Transform transform, ref List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).LoopGetAllGO(ref transforms);
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板)
        /// </summary>
        /// <returns></returns>
        public static GameObject[] GetAllGameObject()
        {
            GameObject[] all1 = UnityEngine.Object.FindObjectsOfType<GameObject>();//查找物体
            return all1;
        }

        /// <summary>
        /// 查找物体(PS：所有,Hierarchy面板)
        /// </summary>
        /// <returns></returns>
        public static Transform[] GetAllTransform()
        {
            Transform[] all1 = UnityEngine.Object.FindObjectsOfType<Transform>();//查找物体
            return all1;
        }

        /// <summary>
        /// 查找子物体.(PS:适用于单个,代码中可直接用,可找到隐藏物体)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childName">物体名称</param>
        /// <returns></returns>
        public static Transform LoopGetOne(this Transform transform, string childName)
        {
            //递归:方法内部又调用自身的过程。
            //1.在子物体中查找
            Transform childTF = transform.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = transform.GetChild(i).LoopGetOne(childName); // 2.将任务交给子物体
                if (childTF != null)
                    return childTF;
            }
            return null;
        }

        /// <summary>
        /// 找到子对象的对应控件 返回一本字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="keyValue"></param>
        /// <param name="controlDic"></param>
        public static void FindChildrenControl<T>(GameObject gameObject, string keyValue, ref Dictionary<string, List<Component>> controlDic) where T : Component
        {
            //返回所有keyValue开头的物品
            List<GameObject> transforms = new List<GameObject>();
            ACLoopGetKeyValueGO(gameObject.transform, keyValue, ref transforms);

            //添加进字典
            for (int i = 0; i < transforms?.Count; i++)
            {
                GameObject tempGo = transforms[i];//组件的物体
                T go = tempGo.GetComponent<T>();
                if (go == null) continue;//如果获取组件空 跳过
                string objType = go.GetType().Name;//组件的类型
                if (controlDic.ContainsKey(objType))
                    controlDic[objType].Add(go);
                else
                    controlDic.Add(objType, new List<Component>() { go });
            }
        }
        #endregion

        #region 增删改查前缀
        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void AddPrefix(this UnityEngine.Object[] objs, string prefix)
        {
            if (objs.Length == 0) Debug.Log("没有物体");
            for (int i = 0; i < objs?.Length; i++)
            {
                GameObject go = objs[i] as GameObject;
                if (!go.name.StartsWith(prefix))
                    go.name = $"{prefix}{go.name}";
            }
        }

        /// <summary>
        /// 删除前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void RemovePrefix(this UnityEngine.Object[] objs, string prefix)
        {
            if (objs.Length == 0) Debug.Log("没有物体");
            for (int i = 0; i < objs?.Length; i++)
            {
                GameObject go = objs[i] as GameObject;
                if (go.name.Contains(prefix))
                    go.name = go.name.Replace(prefix, "");
            }
        }



        /// <summary>
        /// 移除脚本(PS:自定义版本,一个物体里面)
        /// </summary>
        public static void RemoveScript(this GameObject gameObject, String InputCustom)
        {
            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component comp in components)//正常来说肯定有一个
            {
                Type type = comp.GetType();
                if (type.Name == InputCustom)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject.GetComponent(type));
                    Debug.Log($"{gameObject.name} 移除 {InputCustom}脚本成功");
                }
            }
        }

        /// <summary>
        /// 去除空白
        /// </summary>
        public static void ClearTrim(this UnityEngine.Object[] objs)
        {
            if (objs.Length == 0) Debug.Log("没有物体");
            for (int i = 0; i < objs?.Length; i++)
            {
                GameObject go = objs[i] as GameObject;
                go.name = ClearSpecificSymbol(go.name);
            }
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearSpecificSymbol(string str)
        {
            return str.Replace(" ", "").Replace("(", "").Replace(")", "").Trim();//组件名称,順便出去空白
        }
        #endregion

        #region 保存修改
        /// <summary>
        /// 保存修改
        /// </summary>
        public static void SaveModification(UnityEngine.Object[] objs)
        {
            for (int i = 0; i < objs?.Length; i++)
            {
                Undo.RecordObject(objs[i], objs[i].name);
                EditorUtility.SetDirty(objs[i]);
            }
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public static void Save()
        {
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ReAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        #region 检查文件或文件夹路径
        /// <summary>
        /// 文件以追加写入的方式
        /// https://wenku.baidu.com/view/a8fdb767fd4733687e21af45b307e87100f6f85b.html
        /// 显示IO异常请在创建文件的时候Close下
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        private static void FileWriteContent(string path, string content)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
            using (FileStream fsWrite = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            }
        }

        /// <summary>
        /// 通过路径检文件夹是否存在，如果不存在则创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static void ChackFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))//是否存在这个文件
            {
                Debug.Log("文件夹不存在,正在创建...");
                Directory.CreateDirectory(folderPath);//创建
                AssetDatabase.Refresh();//刷新编辑器
                Debug.Log("创建成功!");
            }
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatCSharpScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            string filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using (StreamWriter writer = File.CreateText(filePath))//生成文件
                {
                    writer.Write(content);
                    Debug.Log("内容写入成功!");
                }
            }
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static string TypeChange(string typeStr)
        {
            switch (typeStr)
            {
                case "RectTransform": return "Transform";
                default: return typeStr;
            }
        }
        #endregion

        #region Copy到剪切板
        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void CopyWord(string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void UnityCopyWord(string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }
        #endregion

        #region 移除丢失脚本
        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void RemoveMissScript(this GameObject gameObject)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            //获取所有的物体
            gameObject.transform.ACLoopGetAllGO(ref gameObjects);
            //移除所有的物体的丢失脚本
            for (int i = 0; i < gameObjects?.Count; i++)
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObjects[i]);
            AssetDatabase.Refresh();
            Debug.Log("清理完成!");
        }
        #endregion

        #region 批量更换预设物体（未完成）
        //public static void BatchPrefab()
        //{

        //    Transform tParent = ((GameObject)Selection.activeObject).transform;
        //    Object tempPrefab;
        //    GameObject tt = GameObject.Find("XXX");
        //    int i = 0;
        //    foreach (Transform child in tParent)
        //    {
        //        tempPrefab = EditorUtility.CreateEmptyPrefab("Assets/Prefab" + child.name + ".prefab");
        //        tt.transform.position = child.position;
        //        tt.transform.rotation = child.rotation;
        //        tempPrefab = EditorUtility.ReplacePrefab(tt, tempPrefab);

        //        i++;
        //    }

        //}
        #endregion

        #region 通过字符串反射类型
        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass(string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }
        #endregion
    }
}
