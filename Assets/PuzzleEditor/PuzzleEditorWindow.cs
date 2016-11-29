using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

//Archived in case I need it later. Lots of hacks here. Sorry for not commenting, future self.

public class PuzzleEditorWindow : EditorWindow {
/*
    static PuzzleEditorWindow window;
    Vector3 handlePosition;
    RenderTexture texture;
    SceneSetup[] currentScene;
    bool startClick;
    GameObject blockPrefab;
    Material blockSelectedMaterial;
    Material blockNormalMaterial;
    Vector2 startPos;
    Dictionary<Vector3, BlockProperties> selectedBlocks = new Dictionary<Vector3, BlockProperties>();
    Dictionary<Vector3, BlockProperties> blocks = new Dictionary<Vector3, BlockProperties>();
    List<Vector3> blocksInScene = new List<Vector3>();

    [MenuItem ("Window/Puzzle Editor")]
	public static void ShowWindow()
    {
        window = EditorWindow.GetWindow<PuzzleEditorWindow>();
        window.Init();
        //To make it update the GUI when the scene changes. Live preview window.
        window.autoRepaintOnSceneChange = true;
        window.position = new Rect(0, 0, 1920, 1080);
        window.Show();
    }

    private void OnGUI()
    {
        //Don't think this actually does anything since I'm not using Handles in the rest of the code.
        Handles.BeginGUI();
        HandleMouse(Event.current);
        //DrawHandle();
        //Probably doesn't do anything
        Camera.main.Render();
        GUI.DrawTexture(new Rect(0, 0, 1440, 1080), texture);
        Handles.EndGUI();
    }

    void AddNewBlock(RaycastHit hit)
    {
        selectedBlocks.Clear();
        //....


        RenderBlocks();
    }

    void RenderBlocks()
    {
        foreach (KeyValuePair<Vector3, BlockProperties> block in blocks)
        {
             if (blocksInScene.Contains(block.Key))
            {
                return;
            }
             else
            {
                Instantiate(blockPrefab, block.Key, Quaternion.identity);
                blocksInScene.Add(block.Key);
            }
        }
    }

    void HandleMouse(Event e)
    {
        if (e.type == EventType.MouseDown)
        {
            if (!startClick)
            {
                startPos = e.mousePosition;
                startClick = true;
            }
        }

        if (e.type == EventType.MouseUp)
        {
            startClick = false;
            startPos = Vector3.zero;

            if (e.button == 0)
            {
                Vector2 windowPosition = new Vector2(window.position.x, window.position.y);
                Vector3 screenPosition = GUIUtility.GUIToScreenPoint(e.mousePosition - windowPosition);
                screenPosition.y = Screen.height - screenPosition.y;
                Ray screenRay = Camera.main.ScreenPointToRay(screenPosition);

                RaycastHit hit;
                if (Physics.Raycast(screenRay, out hit))
                {
                    if (e.modifiers == EventModifiers.Shift)
                    {
                        SelectBlock(hit, false);
                    }
                    else if (e.modifiers == EventModifiers.Control)
                    {
                        AddNewBlock(hit);
                    }
                    else
                    {
                        SelectBlock(hit);
                    }
                }
                else
                {
                    ClearSelectedBlocks();
                }

            }
        }

        if (e.button == 1 && startClick)
        {
            Vector3 rot = Camera.main.transform.parent.rotation.eulerAngles;
            rot.y += ((e.mousePosition.x - startPos.x));

            rot.x = (rot.x > 180) ? rot.x - 360 : rot.x;
            rot.x += (e.mousePosition.y - startPos.y);

            rot.x = (rot.x > 90f) ? 90f : rot.x;
            rot.x = (rot.x < -90f) ? -90f : rot.x;

            //rot *= Camera.main.orthographicSize / 10;

            Camera.main.transform.parent.rotation = Quaternion.Euler(rot);
            startPos = e.mousePosition;
        }

        if (e.type == EventType.ScrollWheel)
        {
            Camera.main.orthographicSize += e.delta.y / 9;
            Camera.main.orthographicSize = (Camera.main.orthographicSize < 0.1f) ? 0.1f : Camera.main.orthographicSize;
        }
    }

    void Init()
    {
        EditorSceneManager.SaveOpenScenes();
        currentScene = EditorSceneManager.GetSceneManagerSetup();
        EditorSceneManager.OpenScene("Assets/Editor/PuzzleEditorScene.unity");
        texture = new RenderTexture(1440, 1080, 16);
        blockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/EditorCubePrefab.prefab");
        blockNormalMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/PuzzleEditorMaterial.mat");
        blockSelectedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/PuzzleEditorSelectedMaterial.mat");
        Camera.main.targetTexture = texture;
        blocks.Add(Vector3.zero, new BlockProperties());
        blocks.Add(new Vector3(0,0,1), new BlockProperties());
        blocks.Add(Vector3.one, new BlockProperties());
        blocks.Add(new Vector3(0, 1, 1), new BlockProperties());
        blocks.Add(new Vector3(0, 2, 1), new BlockProperties());
        blocks.Add(new Vector3(0, 4, 1), new BlockProperties());
        blocks.Add(new Vector3(0, 5, 1), new BlockProperties());
        RenderBlocks();
    }

    void SelectBlock(RaycastHit hit, bool selectOneBlock = true)
    {
        if (selectOneBlock) {ClearSelectedBlocks();}

        Vector3 selectedPosition = hit.collider.transform.localPosition;

        if (selectedBlocks.ContainsKey(selectedPosition))
        {
            Collider[] colliders = Physics.OverlapSphere(selectedPosition, .1f);
            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<MeshRenderer>().material = blockNormalMaterial;
            }
            selectedBlocks.Remove(selectedPosition);
        }
        else
        {
            hit.collider.gameObject.GetComponent<MeshRenderer>().material = blockSelectedMaterial;
            selectedBlocks.Add(selectedPosition, blocks[selectedPosition]);
        }
    }

    Vector3 DrawHandle()
    {
        if (selectedBlocks.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;
        float longestDistance = 0f;

        foreach (Vector3 key in selectedBlocks.Keys)
        {
            foreach (Vector3 key2 in selectedBlocks.Keys)
            {
                if (Vector3.Distance(key, key2) > longestDistance)
                {
                    startPos = key;
                    endPos = key2;
                }
            }
        }
        handlePosition = Vector3.Lerp(startPos, endPos, 0.5f);
        return Vector3.zero;
        //return Handles.PositionHandle(handlePosition, Quaternion.identity);
    }



    void ClearSelectedBlocks()
    {
        foreach(KeyValuePair<Vector3, BlockProperties> block in selectedBlocks)
        {
            Collider[] colliders = Physics.OverlapSphere(block.Key, 1f);
            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<MeshRenderer>().material = blockNormalMaterial;
            }
        }
        selectedBlocks.Clear();
    }

    private void OnDestroy()
    {
        EditorSceneManager.RestoreSceneManagerSetup(currentScene);
    }
    */
}

public class BlockLayoutJSON
{
    public string Title;
    public int KeepTotal;
    public int BlockTotal;
    public Dictionary<Vector3, BlockProperties> Blocks;
}
