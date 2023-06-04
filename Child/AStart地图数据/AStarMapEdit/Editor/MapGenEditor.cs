using System;
using UnityEditor;
using UnityEngine;

namespace ACTool
{

    public class MapGenEditor : EditorWindow
    {
        private int mapX = 64;
        private int mapZ = 64;
        private int blockSize = 8;
        int blockHeight = 8;
        private string blockPath = "Assets/UnityEditorTool/Child/AStart��ͼ����/AStarMapEdit/Res/Cube.prefab";

        [MenuItem("Assets/����EditorTool/AStart/��ͼ��������MapEidtorGen")]
        static void run()
        {
            EditorWindow.GetWindow<MapGenEditor>();
        }

        public void OnGUI()
        {
            GUILayout.Label("��ͼX�������");
            this.mapX = Convert.ToInt32(GUILayout.TextField(this.mapX.ToString()));

            GUILayout.Label("��ͼz�������");
            this.mapZ = Convert.ToInt32(GUILayout.TextField(this.mapZ.ToString()));

            GUILayout.Label("��ͼ���С");
            this.blockSize = Convert.ToInt32(GUILayout.TextField(this.blockSize.ToString()));
            GUILayout.Label("��ͼ��߶�");
            this.blockHeight = Convert.ToInt32(GUILayout.TextField(this.blockHeight.ToString()));

            GUILayout.Label("ѡ���ͼԭ��");
            if (Selection.activeGameObject != null)
            {
                GUILayout.Label(Selection.activeGameObject.name);
            }
            else
            {
                GUILayout.Label("û��ѡ�е�ͼԭ��!!!!");
            }

            if (GUILayout.Button("��ԭ�������ɵ�ͼ��"))
            {
                if (Selection.activeGameObject != null)
                {
                    Debug.Log("��ʼ����...");
                    this.CreateBlocksAt(Selection.activeGameObject);
                    Debug.Log("���ɽ���");
                }
            }

            if (GUILayout.Button("���õ�ͼ��"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ResetBlocks(Selection.activeGameObject);
                }
            }

            if (GUILayout.Button("�����ͼ��"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ClearBlocksAt(Selection.activeGameObject);
                }
            }
        }

        private void ResetBlocks(GameObject org)
        {
            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(i).gameObject;
                cube.GetComponent<BlockData>().isGo = 0;
                cube.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void ClearBlocksAt(GameObject org)
        {

            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(0).gameObject;
                GameObject.DestroyImmediate(cube);
            }
        }
        private void CreateBlocksAt(GameObject org)
        {
            MapEditorMgr mgr = org.GetComponent<MapEditorMgr>();
            if (!mgr)
            {
                mgr = org.AddComponent<MapEditorMgr>();
            }
            mgr.mapX = this.mapX;
            mgr.mapZ = this.mapZ;
            mgr.blockSize = this.blockSize;

            this.ClearBlocksAt(org);

            Vector3 startPos = new Vector3(this.blockSize * 0.5f, 0, this.blockSize * 0.5f);
            GameObject cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(blockPath);
            for (int i = 0; i < this.mapZ; i++)
            {
                Vector3 pos = startPos;
                for (int j = 0; j < this.mapX; j++)
                {
                    GameObject cube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
                    cube.name = "block";
                    cube.transform.SetParent(org.transform, false);
                    cube.transform.localPosition = pos;
                    cube.transform.localScale = new Vector3(this.blockSize, this.blockHeight, this.blockSize);
                    BlockData block = cube.AddComponent<BlockData>();
                    block.mapX = j;
                    block.mapZ = i;
                    block.isGo = 0;

                    pos.x += this.blockSize;
                }
                startPos.z += this.blockSize;
            }
        }

        void OnSelectionChange()
        {
            this.Repaint();
        }
    }
}
