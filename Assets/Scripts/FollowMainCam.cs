using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCam : MonoBehaviour {

    private GameObject mainCamera;
    private GameObject mainCamParent;
    private GameObject mainCamParentParent;
    private GameObject mainCamParentParentParent;

    private GameObject myParent;
    private GameObject myParentParent;
    private GameObject myParentParentParent;


    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("CenterEyeAnchor");
        mainCamParent = mainCamera.transform.parent.gameObject;
        mainCamParentParent = mainCamParent.transform.parent.gameObject;
        mainCamParentParentParent = mainCamParentParent.transform.parent.gameObject;


        myParent = transform.parent.gameObject;
        myParentParent = transform.parent.parent.gameObject;
        myParentParentParent = transform.parent.parent.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        myParent.transform.localPosition = mainCamParent.transform.localPosition;
        myParent.transform.localRotation = mainCamParent.transform.localRotation;

        myParentParent.transform.localPosition = mainCamParentParent.transform.localPosition;
        myParentParent.transform.localRotation = mainCamParentParent.transform.localRotation;

        myParentParentParent.transform.localPosition = mainCamParentParentParent.transform.localPosition;
        myParentParentParent.transform.localRotation = mainCamParentParentParent.transform.localRotation;

        /*
        transform.localPosition = mainCamera.transform.localPosition;
        transform.localRotation = mainCamera.transform.localRotation;*/
    }
}
