using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NumberSprite : MonoBehaviour {

    public Sprite[] NumberTextures;
    public int Number;

    void Start () {
		
	}

    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = NumberTextures[Number];
    }

}
