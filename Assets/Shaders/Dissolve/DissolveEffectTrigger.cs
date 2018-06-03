using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffectTrigger : MonoBehaviour {

    public Material dissolveMaterial;
    public float speed;

    public KeyCode keyToPress;

    public bool dissolved;

    private float currentY, startTime;

	// Use this for initialization
	void Start () {
        dissolveMaterial.SetFloat("_Dissolved", 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (!dissolved)
        {
            if (currentY < 1)
            {
                dissolveMaterial.SetFloat("_Dissolved", currentY);
                currentY += Time.deltaTime * speed;
            }
        }
        else
        {
            if(currentY > 0)
            {
                dissolveMaterial.SetFloat("_Dissolved", currentY);
                currentY -= Time.deltaTime * speed;
            }
        }

        if (Input.GetKeyDown(keyToPress))
        {
            TriggerEffect();
        }
	}

    private void TriggerEffect()
    {
        startTime = Time.time;
        dissolved = !dissolved;
    }
}
