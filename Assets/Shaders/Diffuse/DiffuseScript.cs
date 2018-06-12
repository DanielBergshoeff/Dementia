using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffuseScript : MonoBehaviour {

    public float sizeDissolve = 0f;
    public float sizeSlice = 0f;
    public float maxDissolve = 4.0f;
    public float timesFactor = 3.0f;
    public bool startDissolve;

    public Texture textureDissolve;

    public Vector3 startPosition;

    public float speedDissolve = 0.01f;

    public GameObject parentOfAllMaterials;

    public List<Material> materials;

    private Renderer[] renderers;

    public Shader shader;

    // Use this for initialization
    void Start () {
        renderers = parentOfAllMaterials.GetComponentsInChildren<Renderer>();
        foreach(Renderer ren in renderers)
        {
            /*for (int i = 0; i < ren.materials.Length; i++)
            {
                Material newMat = new Material(ren.materials[i]);
                newMat.name += "DIFFUSE";
                newMat.shader = shader;

                Debug.Log(ren.materials[i].name);

                ren.materials[i] = (Material)Instantiate(newMat);

                Debug.Log(ren.materials[i].name);

                if (!materials.Contains(newMat))
                {
                    materials.Add(newMat);
                }
            }*/
            for (int i = 0; i < ren.materials.Length; i++)
            {
                ren.materials[i].shader = shader;
            }
        }      
        
        foreach (Renderer ren in renderers)
        {
            for (int i = 0; i < ren.materials.Length; i++)
            {
                ren.materials[i].SetVector("_StartingVector", startPosition);
                ren.materials[i].SetTexture("_DissolveTexture", textureDissolve);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (startDissolve && sizeSlice < maxDissolve)
        {
            sizeSlice += speedDissolve;                     
        }
        else if(!startDissolve && sizeSlice > 0.0f)
        {
            sizeSlice -= speedDissolve;
        }
        else
        {
            if(!startDissolve)
            {
                sizeSlice = 0.0f;
            }
            else
            {
                sizeSlice = maxDissolve;
            }
        }

        sizeDissolve = (timesFactor * sizeSlice);
        foreach (Renderer ren in renderers)
        {
            for (int i = 0; i < ren.materials.Length; i++)
            {
                ren.materials[i].SetFloat("_DissolveSize", sizeDissolve);
                ren.materials[i].SetFloat("_SliceAmount", sizeSlice);
            }
        }
    }

    public void StartDissolve(Vector3 startVector)
    {
        //myMaterial.SetVector("_StartingVector", startVector);
        startDissolve = true;
    }

    public void StopDissolve()
    {
        startDissolve = false;
    }
}
