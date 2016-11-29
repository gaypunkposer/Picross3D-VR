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
        creator.Cube = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", creator.Cube, typeof(GameObject), false);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
            puzzleDimensions = EditorGUILayout.Vector3Field("Puzzle Dimensions", puzzleDimensions);
            puzzleDimensions.x = (puzzleDimensions.x < 2) ? 2 : puzzleDimensions.x;
            puzzleDimensions.y = (puzzleDimensions.y < 2) ? 2 : puzzleDimensions.y;
        EditorGUILayout.EndHorizontal();
    }

    void CreatePuzzleBase(Vector3 dimensions)
    {
        if (creator.Cube != null)
        {
            dimensions.x = Mathf.Floor(dimensions.x / 2);
            dimensions.y = Mathf.Floor(dimensions.y / 2);
            dimensions.z = Mathf.Floor(dimensions.z / 2);

            for (int x = -(int)dimensions.x; x <= dimensions.x; x++)
            {
                for (int y = -(int)dimensions.y; y <= dimensions.y; y++)
                {
                    for (int z = -(int)dimensions.z; z <= dimensions.z; z++)
                    {
                        Instantiate(creator.Cube, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }
}
