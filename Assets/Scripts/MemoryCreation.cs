using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip[] audioClips;
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

    public int currentMaxMemory;

    public AudioClip endGame;

    public GameObject endGameObject;

    public AudioClip startGame;


    public GameObject waterLine;

    public GameObject player;

    // Use this for initialization
    void Start()
    {
        //memoryObjects[0].walkByBlocks.StartMemory();
        startMemory = new bool[memoryObjects.Length];
        currentMaxMemory = 1;
        AudioManager.PlayAudio(startGame);
        endGameObject.SetActive(false);

        for (int i = 0; i < memoryObjects.Length - currentMaxMemory; i++)
        {
            memoryObjects[memoryObjects.Length -1 - i].memoryObject.GetComponent<Collider>().enabled = false;
            memoryObjects[memoryObjects.Length - 1 - i].memoryObject.GetComponent<Rigidbody>().useGravity = false;
        }
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
                Debug.Log("Dropped Left");
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
                Debug.Log("Dropped Right");
                CheckIfMemoryClose(itemHoldingRight);
                holdingRight = false;
                itemHoldingRight = null;
            }
        }
        

        if(startMemory[2] == true) {
            if(waterLine.transform.position.y > 2.0f)
            {
                waterLine.transform.Translate(Vector3.down * Time.deltaTime * 0.05f, Space.World);
            }
            if(waterLine.transform.position.y <= player.transform.position.y)
            {
                RenderSettings.fogColor = Color.blue;
                RenderSettings.fogDensity = 0.06f;
            }
        }
        if(startMemory[4] == true)
        {
            if(!endGameObject.activeSelf)
            {
                endGameObject.SetActive(true);
            }
        } 
    }

    void CheckIfMemory(GameObject go)
    {
        foreach (ObjectToMemory otm in memoryObjects)
        {
            if (otm.memoryObject.name == go.name)
            {
                int memInt = System.Array.IndexOf(memoryObjects, otm);
                if (memInt < currentMaxMemory)
                {
                    if(memInt + 1 == currentMaxMemory)
                    {
                        currentMaxMemory++;
                        memoryObjects[currentMaxMemory - 1].memoryObject.GetComponent<Collider>().enabled = true;
                        memoryObjects[currentMaxMemory - 1].memoryObject.GetComponent<Rigidbody>().useGravity = true;
                    }
                    Debug.Log("Start memory");
                    for (int i = 0; i < memoryObjects.Length; i++)
                    {
                        memoryObjects[i].memoryObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        memoryObjects[i].memoryObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    otm.walkByBlocks.StartMemory();
                    
                }                
                MemoryStarted(System.Array.IndexOf(memoryObjects, otm));
            }
        }
    }

    public void EndMemory()
    {
        for (int i = 0; i < memoryObjects.Length; i++)
        {
            memoryObjects[i].memoryObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            memoryObjects[i].memoryObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void CheckIfMemoryClose(GameObject go)
    {
        foreach (ObjectToMemory otm in memoryObjects)
        {
            if (otm.memoryObject.name == go.name)
            {
                Debug.Log("Stop memory");
                otm.walkByBlocks.stopMemory = true;
                otm.walkByBlocks.forcedStop = true;
            }
        }
    }

    public void CheckIfOneLiner(GameObject go)
    {
        Debug.Log("Checking for oneliner for " + go.name);

        foreach(ObjectToAudio ota in audioObjects)
        {
            if(ota.objectToTrigger.name == go.name)
            {
                Debug.Log("Start audio");
                AudioManager.PlayAudio(ota.audioClips[UnityEngine.Random.Range(0, ota.audioClips.Length)]);
            }
        }
    }

    void MemoryStarted(int index)
    {
        startMemory[index] = true;
    }

    public IEnumerator EndGame()
    {
        player.GetComponent<OVRScreenFade>().FadeIn();
        AudioManager.audioSelf.clip = endGame;
        AudioManager.audioSelf.Play();
        yield return new WaitForSeconds(endGame.length);
        SceneManager.LoadScene(1);
    }
}
