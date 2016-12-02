using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class PuzzleCreatorEditor : EditorWindow {

    PuzzleCreator creator;
    Vector3 puzzleDimensions;
    static PuzzleCreatorEditor puzzleCreatorEditor;
    bool showPosition;
    Vector2 scrollPos;
    Transform parent;
    bool keep;
    Material material;
    SceneSetup[] currentScene;
    int numberCount;

    [MenuItem("Window/Puzzle Creator")]
    static void Enable()
    {
        puzzleCreatorEditor = (PuzzleCreatorEditor)EditorWindow.GetWindow<PuzzleCreatorEditor>();
        puzzleCreatorEditor.Init();
        puzzleCreatorEditor.name = "Puzzle Creator Editor";
        puzzleCreatorEditor.autoRepaintOnSceneChange = true;
    }

    void Init()
    {
        EditorSceneManager.SaveOpenScenes();
        currentScene = EditorSceneManager.GetSceneManagerSetup();
        EditorSceneManager.OpenScene("Assets/PuzzleEditor/PuzzleEditorScene.unity");
        parent = GameObject.Find("Puzzle Creator Parent").transform;
    }

    private void OnEnable()
    {
        creator = new PuzzleCreator();
        parent = GameObject.Find("Puzzle Creator Parent").transform;
    }

    public void OnGUI()
    {
        
        if (!creator.PuzzleCreated){
            
            creator.Cube = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", creator.Cube, typeof(GameObject), false);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
                puzzleDimensions = EditorGUILayout.Vector3Field("Puzzle Dimensions", puzzleDimensions);
                puzzleDimensions.x = (puzzleDimensions.x < 2) ? 2 : puzzleDimensions.x;
                puzzleDimensions.y = (puzzleDimensions.y < 2) ? 2 : puzzleDimensions.y;
                puzzleDimensions.z = (puzzleDimensions.z < 1) ? 1 : puzzleDimensions.z;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Puzzle")) 
            {
                CreatePuzzleBase(puzzleDimensions);
            }
        }
        else {
            creator.PuzzleName = EditorGUILayout.TextField("Puzzle Name", creator.PuzzleName);
            parent.name = creator.PuzzleName;

            EditorGUILayout.Space();


            showPosition = EditorGUILayout.Foldout(showPosition, "Selection");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            if (showPosition)
            {
                foreach (Transform t in Selection.transforms)
                {
                    EditorGUILayout.ObjectField("Pos: " + t.position, t.gameObject, typeof(GameObject), false);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();



            keep = EditorGUILayout.Toggle("Keep", CheckSelectionKeep());
            material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material), false);

            foreach (Transform t in Selection.transforms)
            {
                creator.Blocks[t.gameObject].Keep = keep;
                creator.Blocks[t.gameObject].Material = material;
            }

            keep = false;
            material = null;


            EditorGUILayout.Space();


            if (numberCount > 0) 
            {
                int number;
                for (int i = 0; i < numberCount; i++) 
                {
                    number = EditorGUILayout.IntField()
                }
            }


            if (GUILayout.Button("Add Number")) 
            {
                numberCount++;
                foreach (Transform t in Selection.transforms) 
                {
                    creator.Numbers.Add(creator.Blocks[t.gameObject], new NumberProperties());
                }
            }

            if (GUILayout.Button("Reset Puzzle")) 
            {
                EditorWindow.GetWindow<ResetConfirmWindow>();
                parent.name = "Puzzle Creator Parent";
            }

        }


    }


    private void OnDestroy()
    {
        EditorSceneManager.RestoreSceneManagerSetup(currentScene);
    }

    public static void ResetPuzzleConfirm()
    {
        puzzleCreatorEditor.ResetPuzzle();
    }

    bool CheckSelectionKeep()
    {
        foreach (Transform t in Selection.transforms)
        {
            if (!creator.Blocks[t.gameObject].Keep)
            {
                return false;
            }
        }
        return true;
    }

    int CheckSelectionNumber() 
    {
        int number = creator.Numbers.
    }

    void ResetPuzzle() {
        foreach (GameObject gobject in creator.Blocks.Keys) {
            DestroyImmediate(gobject);
        }
        creator.Blocks.Clear();
        creator.PuzzleCreated = false;
    }
    
    void CreatePuzzleBase(Vector3 dimensions)
    {
        if (creator.Cube != null)
        {
            creator.PuzzleCreated = true;
            dimensions.x = (dimensions.x / 2);
            dimensions.y = (dimensions.y / 2);
            dimensions.z = (dimensions.z / 2);
            

            for (int x = -(int)dimensions.x; x < dimensions.x; x++)
            {
                for (int y = -(int)dimensions.y; y < dimensions.y; y++)
                {
                    for (int z = -(int)dimensions.z; z < dimensions.z; z++)
                    {
                        
                        GameObject gobject = Instantiate(creator.Cube, new Vector3(x, y, z), Quaternion.identity, parent) as GameObject;
                        gobject.name = "Cube: " + new Vector3(x, y, z);
                        creator.Blocks.Add(gobject, new BlockProperties(new Vector3(x,y,z)));
                    }
                }
            }
        }
        else {
            Debug.LogError("Cube Prefab cannot be null");
        }
    }
}

public class ResetConfirmWindow : EditorWindow {

    void OnGUI()
    {
        GUILayout.Label("Are you sure you want to reset the puzzle?");
        if (GUILayout.Button("Yes"))
        {
            PuzzleCreatorEditor.ResetPuzzleConfirm();
            this.Close();
        }
    }
}

public class PuzzleCreator
{

    public Dictionary<GameObject, BlockProperties> Blocks = new Dictionary<GameObject, BlockProperties>();
    public Dictionary<BlockProperties, NumberProperties> Numbers = new Dictionary<BlockProperties, NumberProperties>();
    public GameObject Cube;
    public bool PuzzleCreated;
    public string PuzzleName;
}