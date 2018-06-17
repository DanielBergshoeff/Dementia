using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.PostProcessing;

public class WalkByBlocks : MonoBehaviour {
    public PostProcessingProfile profileCurrent;
    public PostProcessingProfile profileMemory;

    public GameObject moveBlocks;

    public AudioClip soundOutside;
    public AudioClip soundLivingRoom;
    public AudioClip soundOffice;
    public AudioClip soundBedRoom;
    public AudioClip soundHall;
    public AudioClip soundBathroom;
    public AudioClip soundKitchen;
    public AudioClip soundRadio;
    public AudioClip soundSelf;
    public AudioClip soundPhone;

    public DiffuseScript diffuseScript;

    public float secondsBetweenMovement;
    private float timerMovement;


    private Vector3 locationBeforeMemory;
    private Quaternion rotationBeforeMemory;

    private Vector3 worldPositionBeforeMemory;

    private Transform[] blocks;
    private Transform[] lookAtBlocks;

    public float speed;
    public float lookRotationSpeed;

    public float timeMemory;

    private int currentBlock;

    private Camera cam;

    public Camera camMemory;

    private bool playMemory;
    public bool stopMemory;

    public bool flyController;

    public GameObject currentWorld;
    public GameObject memoryWorld;

    private GameObject player;

    private MemoryCreation memCreation;

    private bool currentlyTriggering;
    private bool currentlyStopping;
    public bool forcedStop;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        player = GameObject.Find("OVRPlayerController");
        memCreation = GameObject.Find("MemoryManager").GetComponent<MemoryCreation>();

        memoryWorld.SetActive(false);

        playMemory = false;
        currentBlock = 0;
        
        blocks = new Transform[moveBlocks.transform.childCount];

        for (int i = 0; i < moveBlocks.transform.childCount; i++)
        {
            blocks[i] = moveBlocks.transform.GetChild(i);
        }        
    }
	
	// Update is called once per frame
	void Update () {
        if (playMemory == true && !stopMemory)
        {
            if(blocks.Length <= currentBlock)
            {
                stopMemory = true;
            }
            else{
                MoveToNextBlock();
            }
        }

        if(stopMemory && !currentlyStopping && !currentlyTriggering)
        {
            if (forcedStop)
            {
                StopMemory(false);
                forcedStop = false;
            }
            else
            {
                StopMemory(true);
            }
        }
	}

    public void MoveToNextBlock()
    {
        if (Time.time > timerMovement)
        {
            camMemory.transform.parent.position = Vector3.MoveTowards(camMemory.transform.parent.position, blocks[currentBlock].transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(camMemory.transform.parent.position, blocks[currentBlock].transform.position) < 0.1f)
            {
                currentBlock++;
                timerMovement = Time.time + secondsBetweenMovement;
            }
        }
    }

    public void StartMemory()
    {
        if (!currentlyTriggering && !currentlyStopping)
        {
            worldPositionBeforeMemory = currentWorld.transform.position;
            locationBeforeMemory = player.transform.position;
            rotationBeforeMemory = player.transform.rotation;
            StartCoroutine(TriggerShader());
        }
    }

    public void StopMemory(bool afterDuration)
    {
        if (playMemory)
        {
            if (afterDuration)
            {
                StartCoroutine(MemoryDuration(timeMemory));
            }
            else
            {
                StartCoroutine(TriggerShaderOff());
            }
        }
    }

    IEnumerator MemoryDuration(float amtOfTime)
    {
        yield return new WaitForSeconds(amtOfTime);
        if (playMemory && stopMemory && !currentlyStopping)
        {
            StartCoroutine(TriggerShaderOff());
        }
    }


    IEnumerator TriggerShader()
    {
        currentlyTriggering = true;

        memoryWorld.SetActive(true);
        camMemory.enabled = true;

        cam.gameObject.GetComponent<AudioListener>().enabled = false;
        camMemory.gameObject.GetComponent<AudioListener>().enabled = true;
        camMemory.GetComponent<PostProcessingBehaviour>().profile = profileMemory;
        camMemory.transform.parent.transform.position = blocks[0].transform.position;

        AudioManager.audioSelf = camMemory.GetComponent<AudioSource>();

        playMemory = true;
        currentBlock = 0;

        if (flyController)
        {
            player.GetComponent<FlyController>().enabled = false;
        }
        else
        {
            player.GetComponent<CharacterController>().enabled = false;
        }
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        player.GetComponent<Rigidbody>().isKinematic = true;

        diffuseScript.enabled = true;

        Debug.Log("Start memory by dissolve");
        diffuseScript.StartDissolve(player.transform.position);
        
        yield return new WaitForSeconds(3);

        //PLAY AUDIO
        if(soundOutside != null)
        {
            AudioManager.audioOutside.clip = soundOutside;
            AudioManager.audioOutside.Play();
        }

        if (soundLivingRoom != null)
        {
            AudioManager.audioLivingRoom.clip = soundLivingRoom;
            AudioManager.audioLivingRoom.Play();
        }

        if(soundOffice != null)
        {
            AudioManager.audioOffice.clip = soundOffice;
            AudioManager.audioOffice.Play();
        }

        if(soundBedRoom != null)
        {
            AudioManager.audioBedRoom.clip = soundBedRoom;
            AudioManager.audioBedRoom.Play();
        }

        if (soundHall != null)
        {
            AudioManager.audioHall.clip = soundHall;
            AudioManager.audioHall.Play();
        }

        if (soundBathroom != null)
        {
            AudioManager.audioBathRoom.clip = soundBathroom;
            AudioManager.audioBathRoom.Play();
        }

        if (soundKitchen != null)
        {
            AudioManager.audioKitchen.clip = soundKitchen;
            AudioManager.audioKitchen.Play();
        }

        if (soundRadio != null)
        {
            AudioManager.audioRadio.clip = soundRadio;
            AudioManager.audioRadio.Play();
        }

        if (soundSelf != null)
        {
            AudioManager.audioSelf.clip = soundSelf;
            AudioManager.audioSelf.Play();
        }

        if(soundPhone != null)
        {
            AudioManager.audioPhone.clip = soundPhone;
            AudioManager.audioPhone.Play();
        }

        cam.gameObject.SetActive(false);
        currentWorld.SetActive(false);

        currentlyTriggering = false;
    }

    IEnumerator TriggerShaderOff()
    {
        stopMemory = false;

        currentlyStopping = true;

        currentWorld.SetActive(true);
        cam.gameObject.SetActive(true);

        playMemory = false;

        //TURN AUDIO OFF
        if (soundOutside != null)
        {
            AudioManager.audioOutside.Stop();
            AudioManager.audioOutside.clip = null;

        }

        if (soundLivingRoom != null)
        {
            AudioManager.audioLivingRoom.Stop();
            AudioManager.audioLivingRoom.clip = null;
        }

        if (soundOffice != null)
        {
            AudioManager.audioOffice.Stop();
            AudioManager.audioOffice.clip = null;
        }

        if (soundBedRoom != null)
        {
            AudioManager.audioBedRoom.Stop();
            AudioManager.audioBedRoom.clip = null;
        }

        if (soundHall != null)
        {
            AudioManager.audioHall.Stop();
            AudioManager.audioHall.clip = null;
        }

        if (soundBathroom != null)
        {
            AudioManager.audioBathRoom.Stop();
            AudioManager.audioBathRoom.clip = null;
        }

        if (soundKitchen != null)
        {
            AudioManager.audioKitchen.Stop();
            AudioManager.audioKitchen.clip = null;
        }

        if (soundRadio != null)
        {
            AudioManager.audioRadio.Stop();
            AudioManager.audioRadio.clip = null;
        }

        if (soundSelf != null)
        {
            AudioManager.audioSelf.Stop();
            AudioManager.audioSelf.clip = null;
        }

        if(soundPhone != null)
        {
            AudioManager.audioPhone.Stop();
            AudioManager.audioPhone.clip = null;
        }

        Debug.Log("Remove memory by dissolve");

        diffuseScript.StopDissolve();
        yield return new WaitForSeconds(3);

        player.GetComponent<Rigidbody>().isKinematic = false;
        if (flyController)
        {
            player.GetComponent<FlyController>().enabled = true;
        }
        else
        {
            player.GetComponent<CharacterController>().enabled = true;
        }

        cam.gameObject.GetComponent<AudioListener>().enabled = true;
        camMemory.gameObject.GetComponent<AudioListener>().enabled = false;

        AudioManager.audioSelf = cam.GetComponent<AudioSource>();

        yield return new WaitForSeconds(2);
        camMemory.enabled = false;
        memoryWorld.SetActive(false);
        diffuseScript.enabled = false;

        memCreation.EndMemory();
        
        currentlyStopping = false;
    }

}
