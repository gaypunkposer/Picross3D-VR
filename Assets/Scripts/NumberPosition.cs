using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPosition : MonoBehaviour {

    public Transform PuzzleParent;

	void Start () {
        UpdatePosition();
	}

	public void UpdatePosition () {
        transform.SetParent(PuzzleParent);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            transform.position = hit.point - (transform.forward * .001f);
            transform.SetParent(hit.collider.transform);
        }
        else
        {
            gameObject.SetActive(false);
        }
	}


}
