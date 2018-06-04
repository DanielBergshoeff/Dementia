using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectToMemory
{
    public GameObject memoryObject;
    public WalkByBlocks walkByBlocks;
}


public class MemoryCreation : MonoBehaviour {

    public ObjectToMemory[] memoryObjects;

    public GameObject handLeft;
    public GameObject handRight;

    public bool holdingLeft = false;
    public bool holdingRight = false;

    public int memoryTestInt;

    private GameObject itemHoldingLeft;
    private GameObject itemHoldingRight;

    private Vector3 lastPosition;

    public bool memoryTestBool;
    public bool turnMemoryOff;

    // Use this for initialization
    void Start() {
        //memoryObjects[0].walkByBlocks.StartMemory();
    }

    // Update is called once per frame
    void Update () {
        if(memoryTestBool)
        {
            memoryObjects[memoryTestInt].walkByBlocks.StartMemory();
            memoryTestBool = false;
        }

        if(turnMemoryOff)
        {
            memoryObjects[memoryTestInt].walkByBlocks.StopMemory(false);
            turnMemoryOff = false;
        }


		if(!holdingLeft)
        {
            if(handLeft.GetComponent<OVRGrabber>().grabbedObject != null)
            {
                itemHoldingLeft = handLeft.GetComponent<OVRGrabber>().grabbedObject.gameObject;
                CheckIfMemory(itemHoldingLeft);                
                holdingLeft = true;
            }
        }
        else
        {
            if(handLeft.GetComponent<OVRGrabber>().grabbedObject == null)
            {
                CheckIfMemoryClose(itemHoldingLeft);
                holdingLeft = false;
                itemHoldingLeft = null;
            }
        }

        if(!holdingRight)
        {
            if (handRight.GetComponent<OVRGrabber>().grabbedObject != null)
            {
                itemHoldingRight = handRight.GetComponent<OVRGrabber>().grabbedObject.gameObject;
                CheckIfMemory(itemHoldingRight);
                holdingRight = true;
            }
        }
        else
        {
            if (handRight.GetComponent<OVRGrabber>().grabbedObject == null)
            {
                CheckIfMemoryClose(itemHoldingRight);
                holdingRight = false;
                itemHoldingRight = null;
            }
        }
    }

    void CheckIfMemory(GameObject go)
    {
        lastPosition = transform.position;
        foreach (ObjectToMemory otm in memoryObjects)
        {
            if (otm.memoryObject.name == go.name)
            {
                Debug.Log("Start memory");
                otm.walkByBlocks.StartMemory();
            }
        }
    }

    void CheckIfMemoryClose(GameObject go)
    {
        foreach (ObjectToMemory otm in memoryObjects)
        {
            if (otm.memoryObject.name == go.name)
            {
                Debug.Log("Stop memory");
                otm.walkByBlocks.StopMemory(false);
            }
        }
    }
}
