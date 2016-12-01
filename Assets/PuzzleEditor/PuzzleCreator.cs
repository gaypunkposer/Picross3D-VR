using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleCreator : MonoBehaviour {

    public Dictionary<GameObject, BlockProperties> Blocks = new Dictionary<GameObject, BlockProperties>();
    public GameObject Cube;
    public bool PuzzleCreated;
    public string PuzzleName;
}
