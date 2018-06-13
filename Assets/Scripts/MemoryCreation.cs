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

[System.Serializable]
public class ObjectToAudio
{
    public GameObject objectToTrigger;
    public AudioClip audioClip;
}


public class MemoryCreation : MonoBehaviour {

    public ObjectToMemory[] memoryObjects;
    public ObjectToAudio[] audioObjects;

    public GameObject handLeft;
    public GameObject handRight;

    public bool holdingLeft = false;
    public bool holdingRight = false;

    public int memoryTestInt;

    private GameObject itemHoldingLeft;
    private GameObject itemHoldingRight;

    public bool memoryTestBool;
    public bool turnMemoryOff;

    public bool[] startMemory;


    public GameObject waterLine;

    // Use this for initialization
    void Start() {
        //memoryObjects[0].walkByBlocks.StartMemory();
        startMemory = new bool[memoryObjects.Length];
    }

    // Update is called once per frame
    void Update () {
        if(memoryTestBool)
        {
            memoryObjects[memoryTestInt].walkByBlocks.StartMemory();
            MemoryStarted(memoryTestInt);
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
                CheckIfOneLiner(itemHoldingLeft);
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
                CheckIfOneLiner(itemHoldingRight);
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

        if(startMemory[2] == true) {
            if(waterLine.transform.position.y > -0.7f)
            {
                waterLine.transform.Translate(Vector3.down * Time.deltaTime * 0.05f, Space.World);
            }
        }
    }

    void CheckIfMemory(GameObject go)
    {
        foreach (ObjectToMemory otm in memoryObjects)
        {
            if (otm.memoryObject.name == go.name)
            {
                Debug.Log("Start memory");
                otm.walkByBlocks.StartMemory();
                MemoryStarted(System.Array.IndexOf(memoryObjects, otm.memoryObject));
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

    void CheckIfOneLiner(GameObject go)
    {
        foreach(ObjectToAudio ota in audioObjects)
        {
            if(ota.objectToTrigger.name == go.name)
            {
                Debug.Log("Start audio");
                AudioManager.PlayAudio(ota.audioClip);
            }
        }
    }

    void MemoryStarted(int index)
    {
        startMemory[index] = true;
    }
}
