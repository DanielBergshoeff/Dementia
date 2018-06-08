using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {

    public GameObject fishPrefab;

    public Vector3 tankSize = Vector3.zero;

    public int numFish = 75;

    public GameObject[] allFish;

    public Vector3 goalPos = Vector3.zero;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(tankSize.x * 2, tankSize.y * 2, tankSize.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(goalPos, 0.1f);
    }

    // Use this for initialization
    void Start() {

        goalPos = transform.position;
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(gameObject.transform.position.x - tankSize.x, gameObject.transform.position.x + tankSize.x),
                                      Random.Range(gameObject.transform.position.y - tankSize.y, gameObject.transform.position.y + tankSize.y),
                                      Random.Range(gameObject.transform.position.z - tankSize.z, gameObject.transform.position.z + tankSize.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().globalFlock = this;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = new Vector3(Random.Range(gameObject.transform.position.x - tankSize.x, gameObject.transform.position.x + tankSize.x),
                                        Random.Range(gameObject.transform.position.y - tankSize.y, gameObject.transform.position.y + tankSize.y),
                                        Random.Range(gameObject.transform.position.z - tankSize.z, gameObject.transform.position.z + tankSize.z));
        }
    }
}

