using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTracker : MonoBehaviour {

    public List<NumberPosition> NumberPositionTracker;
    public GameObject[] CrackedBlocks;
    public UnityEngine.UI.Text DebugText;

    public GameObject FinalMesh;
    public int MaxFail;

    int failCount;
    int cubeCount;
    int keepCount;


	void Start () {
        
        NumberPositionTracker = new List<NumberPosition>();
        foreach (NumberPosition p in transform.GetComponentsInChildren<NumberPosition>())
        {
            NumberPositionTracker.Add(p);
        }

        foreach (CubeScript c in transform.GetComponentsInChildren<CubeScript>())
        {
            cubeCount++;
        //    keepCount += (c.Keep ? 1 : 0);
        }

        Debug.Log(cubeCount + ":" + keepCount);

    }

    public void BlockBroken()
    {
        //TODO: Optimize this bit. It's really bad on performance. Lots of raycasts that aren't necessary
       foreach (NumberPosition n in NumberPositionTracker)
        {
            n.UpdatePosition();
        }


        cubeCount--;
        DebugText.text = cubeCount.ToString();
        if (keepCount <= cubeCount)
        {
            PuzzleComplete();
        }
    }

    public void BreakFail(Transform failedBlock)
    {
        failCount++;

        Instantiate(CrackedBlocks[Random.Range(0, 4)], failedBlock.position, failedBlock.rotation, transform);
        failedBlock.gameObject.SetActive(false);
        foreach (NumberPosition n in NumberPositionTracker)
        {
            n.UpdatePosition();
        }


        if ((failCount >= MaxFail) && MaxFail >= 0)
        {
            PuzzleFail();
        }
    }

    void PuzzleComplete()
    {
        //TODO Implement Puzzle Complete. Involve final mesh, and animate that shit.
        gameObject.SetActive(false);
        throw new System.NotImplementedException();
    }

    void PuzzleFail()
    {
        //TODO Implement Puzzle Fail
        throw new System.NotImplementedException();
    }
}
