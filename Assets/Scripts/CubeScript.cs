using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {

    public BlockProperties BlockProperties;

    public Material KeepMaterial;
    public Material CubeMaterial;
    public Material KeepMaterialFail;
    public GameObject ParticlePrefab;

    MeshRenderer mrenderer;
    Animator animator;
    bool playerMarked = false;
    bool forceMarked = false;

	void Start ()
    {
        mrenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Selected(string e)
    {
        string message = e;
        if (message == "Break")
        {
            Break();
        }
        
        else if (message == "Mark")
        {
            Mark();
        }
    }

    void Break()
    {
      
       /* if (Keep && !playerMarked && !forceMarked)
        {
            ForceMarked();
        }
        
        else if (playerMarked || forceMarked)
        {
            return;
        }
        else
        {
            GameObject particles = Instantiate(ParticlePrefab, transform.position, transform.rotation) as GameObject;
            Destroy(particles, .5f);
            gameObject.SetActive(false);
        }
        */
    }

    void Mark()
    {
        if (playerMarked)
        {
            mrenderer.material = CubeMaterial;
            playerMarked = false;
        }
        else if (forceMarked)
        {
            return;
        }
        else
        {
            animator.SetTrigger("Mark");
            mrenderer.material = KeepMaterial;
            playerMarked = true;
        }
    }

    void ForceMarked()
    {
        animator.SetTrigger("Fail");
        
    }
    
    void TriggerFailMaterialSwitch()
    {
        transform.parent.GetComponent<PuzzleTracker>().BreakFail(transform);
    }
}
