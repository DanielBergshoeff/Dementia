using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.PostProcessing;

public class WalkByBlocks : MonoBehaviour {
    public PostProcessingProfile profileCurrent;
    public PostProcessingProfile profileMemory;

    public GameObject moveBlocks;
    public GameObject lookDirectionBlocks;

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

    private AudioManager audioManager;


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

    private bool playMemory;
    private bool stopMemory;

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

        audioManager = GameObject.Find("SoundSystems").GetComponent<AudioManager>();
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
            player.transform.position = Vector3.MoveTowards(player.transform.position, blocks[currentBlock].transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(player.transform.position, blocks[currentBlock].transform.position) < 0.1f)
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
        playMemory = true;
        currentBlock = 0;

        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<FlyController>().enabled = false;

        currentWorld.transform.parent = player.transform;
        player.transform.position = blocks[0].transform.position;

        currentWorld.transform.parent = null;


        diffuseScript.StopDissolve();
        
        yield return new WaitForSeconds(3);
        
        cam.GetComponent<PostProcessingBehaviour>().profile = profileMemory;

        //PLAY AUDIO
        if(soundOutside != null)
        {
            audioManager.audioOutside.clip = soundOutside;
            audioManager.audioOutside.Play();
        }

        if (soundLivingRoom != null)
        {
            audioManager.audioLivingRoom.clip = soundLivingRoom;
            audioManager.audioLivingRoom.Play();
        }

        if(soundOffice != null)
        {
            audioManager.audioOffice.clip = soundOffice;
            audioManager.audioOffice.Play();
        }

        if(soundBedRoom != null)
        {
            audioManager.audioBedRoom.clip = soundBedRoom;
            audioManager.audioBedRoom.Play();
        }

        if (soundHall != null)
        {
            audioManager.audioHall.clip = soundHall;
            audioManager.audioHall.Play();
        }

        if (soundBathroom != null)
        {
            audioManager.audioBathRoom.clip = soundBathroom;
            audioManager.audioBathRoom.Play();
        }

        if (soundKitchen != null)
        {
            audioManager.audioKitchen.clip = soundKitchen;
            audioManager.audioKitchen.Play();
        }

        if (soundRadio != null)
        {
            audioManager.audioRadio.clip = soundRadio;
            audioManager.audioRadio.Play();
        }

        if (soundSelf != null)
        {
            audioManager.audioSelf.clip = soundSelf;
            audioManager.audioSelf.Play();
        }

        if(soundPhone != null)
        {
            audioManager.audioPhone.clip = soundPhone;
            audioManager.audioPhone.Play();
        }
    }

    IEnumerator TriggerShaderOff()
    {
        playMemory = false;

        //TURN AUDIO OFF
        if (soundOutside != null)
        {
            audioManager.audioOutside.Stop();
            audioManager.audioOutside.clip = null;

        }

        if (soundLivingRoom != null)
        {
            audioManager.audioLivingRoom.Stop();
            audioManager.audioLivingRoom.clip = null;
        }

        if (soundOffice != null)
        {
            audioManager.audioOffice.Stop();
            audioManager.audioOffice.clip = null;
        }

        if (soundBedRoom != null)
        {
            audioManager.audioBedRoom.Stop();
            audioManager.audioBedRoom.clip = null;
        }

        if (soundHall != null)
        {
            audioManager.audioHall.Stop();
            audioManager.audioHall.clip = null;
        }

        if (soundBathroom != null)
        {
            audioManager.audioBathRoom.Stop();
            audioManager.audioBathRoom.clip = null;
        }

        if (soundKitchen != null)
        {
            audioManager.audioKitchen.Stop();
            audioManager.audioKitchen.clip = null;
        }

        if (soundRadio != null)
        {
            audioManager.audioRadio.Stop();
            audioManager.audioRadio.clip = null;
        }

        if (soundSelf != null)
        {
            audioManager.audioSelf.Stop();
            audioManager.audioSelf.clip = null;
        }

        if(soundPhone != null)
        {
            audioManager.audioPhone.Stop();
            audioManager.audioPhone.clip = null;
        }

        Debug.Log("Remove memory by dissolve");

        diffuseScript.StartDissolve(player.transform.position);
        yield return new WaitForSeconds(3);
        cam.GetComponent<PostProcessingBehaviour>().profile = profileCurrent;
        player.transform.position = locationBeforeMemory;
        Debug.Log(player.transform.position);
        currentWorld.transform.position = worldPositionBeforeMemory;

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<FlyController>().enabled = true;


    }

}
