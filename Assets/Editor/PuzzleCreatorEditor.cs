using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PuzzleCreator))]
public class PuzzleCreatorEditor : Editor {

    PuzzleCreator creator;
    Vector3 puzzleDimensions;


    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        creator = (PuzzleCreator)target;
        if (!creator.PuzzleCreated){
            creator.transform.name = "Puzzle Creator Parent";
            creator.Cube = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", creator.Cube, typeof(GameObject), false);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
                puzzleDimensions = EditorGUILayout.Vector3Field("Puzzle Dimensions", puzzleDimensions);
                puzzleDimensions.x = (puzzleDimensions.x < 2) ? 2 : puzzleDimensions.x;
                puzzleDimensions.y = (puzzleDimensions.y < 2) ? 2 : puzzleDimensions.y;
                puzzleDimensions.z = (puzzleDimensions.z < 1) ? 1 : puzzleDimensions.z;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Puzzle")) {
                CreatePuzzleBase(puzzleDimensions);
            }
        }
        else {
            creator.PuzzleName = EditorGUILayout.TextField("Puzzle Name", creator.PuzzleName);
            creator.transform.name = creator.PuzzleName;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Puzzle")) {
                ResetPuzzle();
            }
            
        
        }


    }

    static void ResetPuzzle() {
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
                        GameObject gobject = Instantiate(creator.Cube, new Vector3(x, y, z), Quaternion.identity, creator.transform) as GameObject;
                        creator.Blocks.Add(gobject, new BlockProperties(new Vector3(x,y,z)));
                    }
                }
            }
        }
        else {
            Debug.Log("Cube Prefab cannot be null");
        }
    }
}

public class ResetConfirmWindow : EditorWindow {
    
    static ResetConfirmWindow window;
    public static void OnEnable() {
        window = (ResetConfirmWindow)GetWindow(typeof(ResetConfirmWindow), true, "Reset Puzzle Confirmation");
    }

    private void OnGUI() {

    }
}