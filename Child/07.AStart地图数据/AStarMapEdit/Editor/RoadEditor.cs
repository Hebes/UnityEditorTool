using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ACTool
{
    [CustomEditor(typeof(MapEditorMgr))]
    public class RoadEditor : Editor
    {
        MapEditorMgr script;
        private bool placing = false;
        private bool enterPlacingBatMode = false;

        //private string mapPath = "Assets/AssetsPackage/Maps/SGYD/mapTex.asset";
        private string mapPath = "Assets/UnityEditorTool/Child/07.AStart地图数据/AStarMapEdit/SGYD/mapTex.asset";

        void changeMapValue(ref RaycastHit hitInfo)
        {
            BlockData data = hitInfo.collider.gameObject.GetComponent<BlockData>();
            data.isGo = (data.isGo == 1) ? 0 : 1;
            if (data.isGo == 1)
            {
                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        public void OnSceneGUI()
        {
            if (this.placing == false) return;

            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Space)
                { // 连续模式
                    Event.current.Use();
                    this.enterPlacingBatMode = !this.enterPlacingBatMode;
                }
                else if (Event.current.keyCode == KeyCode.C)
                { // 单个模式
                    Event.current.Use();

                    this.enterPlacingBatMode = false;
                    Ray worldRay1 = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    RaycastHit hitInfo1;
                    if (!Physics.Raycast(worldRay1, out hitInfo1))
                    {
                        return;
                    }
                    if (hitInfo1.collider.gameObject.name != "block")
                    {
                        return;
                    }

                    this.changeMapValue(ref hitInfo1);
                    return;
                }
            }
            // end 

            if (this.enterPlacingBatMode == false) return;

            // 连续模式
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            if (!Physics.Raycast(worldRay, out hitInfo)) return;
            if (hitInfo.collider.gameObject.name != "block") return;
            // end 

            this.setMapValue(ref hitInfo, 1);
        }

        void setMapValue(ref RaycastHit hitInfo, int value)
        {
            BlockData data = hitInfo.collider.gameObject.GetComponent<BlockData>();
            if (data == null) return;
            data.isGo = value;
            if (data.isGo == 1)
                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            else
                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        public static SceneView GetSceneView()
        {
            SceneView view = SceneView.lastActiveSceneView;
            if (view == null)
                view = EditorWindow.GetWindow<SceneView>();

            return view;
        }

        void OnDisable()
        {
            this.enterPlacingBatMode = false;
            this.placing = false;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Label("设置配置数据文件的生成路径");
            this.mapPath = GUILayout.TextField(mapPath);

            this.script = (MapEditorMgr)this.target;
            SceneView view = GetSceneView();

            if (!this.placing && GUILayout.Button("Start Editing", GUILayout.Height(40)))
            {
                //we passed all prior checks, toggle waypoint placement
                this.placing = true;
                this.enterPlacingBatMode = false;
                view.Focus();
            }
            GUI.backgroundColor = Color.yellow;

            if (this.placing && GUILayout.Button("Finish Editing", GUILayout.Height(40)))
            {
                this.placing = false;
                this.enterPlacingBatMode = false;
                this.ExportMapBitMap();
            }

            GUILayout.Label("空格键按下连续模式");
            GUILayout.Label("C键单点模式");
            GUILayout.Label("当前地图是512*512,block大小为8,按照一个方块8的大小,8*8=64");
        }

        private void ExportMapBitMap()
        {
            Texture2D mapTex = new Texture2D(this.script.mapX, this.script.mapZ, TextureFormat.Alpha8, false);
            byte[] rawData = mapTex.GetRawTextureData();
            for (int i = 0; i < rawData.Length; i++)
            {
                rawData[i] = 0;
            }

            for (int i = 0; i < this.script.gameObject.transform.childCount; i++)
            {
                BlockData block = this.script.gameObject.transform.GetChild(i).GetComponent<BlockData>();
                rawData[i] = (byte)((block.isGo == 0) ? 0 : 255);
            }

            mapTex.LoadRawTextureData(rawData);
            AssetDatabase.DeleteAsset(this.mapPath);
            AssetDatabase.CreateAsset(mapTex, this.mapPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
