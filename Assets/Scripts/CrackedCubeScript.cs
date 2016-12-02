using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedCubeScript : MonoBehaviour {

    Vector3 scale = new Vector3(.95f, .95f, .95f);


	void Start () {
        for (int i = 0; i < 3; i++)
        {
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            while (child.GetComponent<NumberPosition>() != null)
            {
                child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            }
            child.localScale = scale;
        }
    }
}
