using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class ControllerMessageSender : MonoBehaviour {

    VRTK_ControllerEvents events;
    bool triggerStart = false;

	void Start () {
        GetComponent<VRTK_SimplePointer>().DestinationMarkerEnter += ControllerMessageSender_DestinationMarkerSet;        
        events = GetComponent<VRTK_ControllerEvents>();
        events.TriggerTouchStart += Events_TriggerTouchStart;
    }

    private void Events_TriggerTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        triggerStart = true;
    }

    private void ControllerMessageSender_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
    {
        
        if (triggerStart)
        {
            triggerStart = false;
            if (transform.name == "Controller (left)")
            {
                e.target.GetComponent<CubeScript>().Selected("Mark");
            }
            else if (transform.name == "Controller (right)")
            {
                e.target.GetComponent<CubeScript>().Selected("Break");
                e.target.parent.GetComponent<PuzzleTracker>().BlockBroken();

            }
            
            
        }
    }



    // Update is called once per frame
    void Update () {
		
	}
}
