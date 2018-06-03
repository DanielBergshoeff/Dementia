using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishState
{
    SWIM,
    FLEE,
    CHASE
}

public class Flock : MonoBehaviour {

    public float speed = 0.1f;

    public float minSpeed = 0.5f;
    public float maxSpeed = 1.0f;

    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neightbourDistance = 10.0f;
    public float avoidDistance = 0.3f;

    public GlobalFlock globalFlock;

    public Vector3 newGoalPos;

    private FishState currentState;

    public float fleeDistance = 3.0f;
    public GameObject scaryFish;

    bool turning = false;
    

	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        currentState = FishState.SWIM;
	}
	
	// Update is called once per frame
	void Update () {

        if (currentState == FishState.SWIM)
        {
            if (turning)
            {
                Vector3 direction = newGoalPos - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

                speed = Random.Range(minSpeed, maxSpeed);
            }

            else
            {
                if (Random.Range(0, 3) < 1)
                {
                    ApplyRules();
                }
            }
        }

        else if(currentState == FishState.FLEE)
        {
            Vector3 direction = scaryFish.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-direction), rotationSpeed * Time.deltaTime);

            speed = Random.Range(maxSpeed * 1.5f, maxSpeed * 3f);

            if(Vector3.Distance(scaryFish.transform.position, transform.position) >= fleeDistance)
            {
                currentState = FishState.SWIM;
            }
        }
        
        transform.Translate(0, 0, Time.deltaTime * speed);
	}

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neightbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(dist < avoidDistance)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!turning)
        {
            if (other.gameObject.tag == "BigFish" || other.gameObject.tag == "Player")
            {
                currentState = FishState.FLEE;
                scaryFish = other.gameObject;
            }
            else
            {
                newGoalPos = this.transform.position - other.gameObject.transform.position;
            }

            turning = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        turning = false;
    }
}
