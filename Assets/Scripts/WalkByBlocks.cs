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
    private bool stopMemory;

    public bool flyController;

    private GameObject currentWorld;

    private GameObject player;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        player = GameObject.Find("OVRPlayerController");

        currentWorld = GameObject.Find("CurrentWorld");

        playMemory = false;
        currentBlock = 0;
        
        blocks = new Transform[moveBlocks.transform.childCount];

        for (int i = 0; i < moveBlocks.transform.childCount; i++)
        {
            blocks[i] = moveBlocks.transform.GetChild(i);
        }

        //int tempnr = 0;
        /* foreach(Transform child in moveBlocks.transform)
        {
            blocks[tempnr] = child;
            tempnr++;
        } */        
    }
	
	// Update is called once per frame
	void Update () {
        if (playMemory == true && !stopMemory)
        {
            if(blocks.Length <= currentBlock)
            {
                stopMemory = true;
                StopMemory(true);
            }
            else{
                MoveToNextBlock();
            }
        }
	}

    public void MoveToNextBlock()
    {
        if (Time.time > timerMovement)
        {
            camMemory.transform.position = Vector3.MoveTowards(camMemory.transform.position, blocks[currentBlock].transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(camMemory.transform.position, blocks[currentBlock].transform.position) < 0.1f)
            {
                currentBlock++;
                timerMovement = Time.time + secondsBetweenMovement;
            }
        }
    }

    public void StartMemory()
    {
        worldPositionBeforeMemory = currentWorld.transform.position;
        locationBeforeMemory = player.transform.position;
        rotationBeforeMemory = player.transform.rotation;
        StartCoroutine(TriggerShader());
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
        if (playMemory)
        {
            StartCoroutine(TriggerShaderOff());
        }
    }


    IEnumerator TriggerShader()
    {
        camMemory.enabled = true;

        cam.gameObject.GetComponent<AudioListener>().enabled = false;
        camMemory.gameObject.GetComponent<AudioListener>().enabled = true;

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

        /*currentWorld.transform.parent = player.transform;
        player.transform.position = blocks[0].transform.position;

        currentWorld.transform.parent = null;*/


        diffuseScript.StartDissolve(player.transform.position);
        
        yield return new WaitForSeconds(3);
        

        //cam.GetComponent<PostProcessingBehaviour>().profile = profileMemory;

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
    }

    IEnumerator TriggerShaderOff()
    {
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
        //cam.GetComponent<PostProcessingBehaviour>().profile = profileCurrent;
        /*
        player.transform.position = locationBeforeMemory;
        currentWorld.transform.position = worldPositionBeforeMemory; */
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

        camMemory.enabled = false;
    }

}
