using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour {

    public float speed;

    public static bool allowMovement;

    private GameObject cam;

    // Use this for initialization
    void Start () {
        cam = GameObject.Find("CenterEyeAnchor");
	}
	
	// Update is called once per frame
	void Update () {
        if ((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0 || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0) && allowMovement)
        {
            WalkForward();
        }
    }

    public void WalkForward()
    {
        float x = cam.transform.forward.x;
        float z = cam.transform.forward.z;
        transform.Translate(new Vector3(x, 0, z) * Time.deltaTime * speed);
    }
}
