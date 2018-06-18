using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public GameObject audioOutsideObject;
    public GameObject audioLivingRoomObject;
    public GameObject audioOfficeObject;
    public GameObject audioBedRoomObject;
    public GameObject audioHallObject;
    public GameObject audioBathRoomObject;
    public GameObject audioKitchenObject;
    public GameObject audioRadioObject;
    public GameObject audioPhoneObject;

    public GameObject audioSelfObject;

    public static AudioSource audioOutside;
    public static AudioSource audioLivingRoom;
    public static AudioSource audioOffice;
    public static AudioSource audioBedRoom;
    public static AudioSource audioHall;
    public static AudioSource audioBathRoom;
    public static AudioSource audioKitchen;
    public static AudioSource audioRadio;
    public static AudioSource audioPhone;

    public static AudioSource audioSelf;

    private static bool selfAudio;

    private static AudioManager audioManager;

    void Awake()
    {
        audioManager = this;
    }

	// Use this for initialization
	void Start () {
        audioOutside = audioOutsideObject.GetComponent<AudioSource>();
        audioLivingRoom = audioLivingRoomObject.GetComponent<AudioSource>();
        audioOffice = audioOfficeObject.GetComponent<AudioSource>();
        audioBedRoom = audioBedRoomObject.GetComponent<AudioSource>();
        audioHall = audioHallObject.GetComponent<AudioSource>();
        audioBathRoom = audioBathRoomObject.GetComponent<AudioSource>();
        audioKitchen = audioKitchenObject.GetComponent<AudioSource>();
        audioRadio = audioRadioObject.GetComponent<AudioSource>();
        audioPhone = audioPhoneObject.GetComponent<AudioSource>();
        audioSelf = audioSelfObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlayAudio(AudioClip clip)
    {
        if (!selfAudio)
        {
            Debug.Log("Sending to enumerator");
            audioManager.StartCoroutine(PlayAudioSelf(clip));
        }
    }

    static IEnumerator PlayAudioSelf(AudioClip clip)
    {
        selfAudio = true;
        audioSelf.clip = clip;
        audioSelf.Play();

        Debug.Log("Playing " + audioSelf.clip.name);

        yield return new WaitForSeconds(clip.length);

        selfAudio = false;
    }
}
