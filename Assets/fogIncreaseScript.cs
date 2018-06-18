using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogIncreaseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.fogColor = Color.blue;
        RenderSettings.fogDensity += 0.0001f;
    }
}
