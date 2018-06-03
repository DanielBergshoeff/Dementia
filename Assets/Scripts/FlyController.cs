using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {
    public float force = 1000.0f;

    public GameObject leftCube;
    public GameObject rightCube;
    private GameObject cam;

    private Rigidbody rigidbody;

    private Vector3 localPosLeftCube;
    private Vector3 localPosRightCube;

    private float distanceLeftX;
    private float distanceLeftY;
    private float distanceLeftZ;

    private float distanceRightX;
    private float distanceRightY;
    private float distanceRightZ;

    private float nextFly;

    private int amtTestedRight = 0;
    private int amtTestedLeft = 0;
    private float[] previousYPosRight = new float[3];
    private float[] previousYPosLeft = new float[3];
    private float testRange = 10.0f;
    private float[] totalTestRangeLeft = new float[3];
    private float[] totalTestRangeRight = new float[3];
    private float[] averageRight = new float[3];
    private float[] averageLeft = new float[3];

    private float maxDistanceWings = 0.5f;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("CenterEyeAnchor");
        rigidbody = GetComponent<Rigidbody>();

        localPosLeftCube = leftCube.transform.parent.localPosition;
        localPosRightCube = rightCube.transform.parent.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        localPosLeftCube = leftCube.transform.parent.localPosition;
        localPosRightCube = rightCube.transform.parent.localPosition;

        distanceLeftX = leftCube.transform.parent.localPosition.x - cam.transform.localPosition.x;
        distanceLeftY = leftCube.transform.parent.localPosition.y - cam.transform.localPosition.y;
        distanceLeftZ = leftCube.transform.parent.localPosition.z - cam.transform.localPosition.z;

        distanceRightX = rightCube.transform.parent.localPosition.x - cam.transform.localPosition.x;
        distanceRightY = rightCube.transform.parent.localPosition.y - cam.transform.localPosition.y;
        distanceRightZ = rightCube.transform.parent.localPosition.z - cam.transform.localPosition.z;




        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0)
        {
            FlyRight();
        }
        else
        {
            amtTestedRight = 0;
            totalTestRangeRight[0] = 0;
            totalTestRangeRight[1] = 0;
            totalTestRangeRight[2] = 0;
        }

        if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0)
        {
            FlyLeft();
        }
        else
        {
            amtTestedLeft = 0;
            totalTestRangeLeft[0] = 0;
            totalTestRangeLeft[1] = 0;
            totalTestRangeLeft[2] = 0;
        }

        /*
        if (GetComponent<Rigidbody>().velocity.y < 0.1)
        {
            float totalDistanceX = Mathf.Abs(distanceLeftX) + Mathf.Abs(distanceRightX);
            float totalDistanceZ = Mathf.Abs(distanceLeftZ) + Mathf.Abs(distanceRightZ);

            float newVelocityY;

            Vector3 newVelocity;

            if (totalDistanceX > totalDistanceZ)
            {
                if(totalDistanceX < maxDistanceWings * 2)
                {
                    newVelocityY = rigidbody.velocity.y * (totalDistanceX / (maxDistanceWings * 2));                    
                }
                else
                {
                    newVelocityY = 0;
                }                
            }
            else
            {
                if (totalDistanceZ < maxDistanceWings * 2)
                {
                    newVelocityY = rigidbody.velocity.y * (totalDistanceZ / (maxDistanceWings * 2));                    
                }
                else
                {
                    newVelocityY = 0;
                }
            }

            newVelocity = new Vector3(rigidbody.velocity.x, newVelocityY, rigidbody.velocity.z);
            newVelocity += cam.transform.right * (rigidbody.velocity.y - newVelocityY);

            rigidbody.velocity = newVelocity;      
        } */
    }

    void FlyRight()
    {
        if(amtTestedRight < testRange)
        {
            //Debug.Log(rightCube.transform.position.y);
            if(amtTestedRight == 0)
            {
                previousYPosRight[0] = localPosRightCube.y;
                previousYPosRight[1] = localPosRightCube.x;
                previousYPosRight[2] = localPosRightCube.z;
            }
            else
            {
                if (Mathf.Abs(distanceRightX) > Mathf.Abs(distanceRightZ) && distanceRightX < maxDistanceWings)
                {
                    totalTestRangeRight[0] += ((previousYPosRight[0] - localPosRightCube.y) * (Mathf.Abs(distanceRightX) / maxDistanceWings));
                }
                else if (Mathf.Abs(distanceRightX) <= Mathf.Abs(distanceRightZ) && distanceRightZ < maxDistanceWings)
                {
                    totalTestRangeRight[0] += ((previousYPosRight[0] - localPosRightCube.y) * (Mathf.Abs(distanceRightZ) / maxDistanceWings));
                }
                else
                {
                    totalTestRangeRight[0] += (previousYPosRight[0] - localPosRightCube.y);
                }

                totalTestRangeRight[0] += (previousYPosRight[0] - localPosRightCube.y);
                totalTestRangeRight[1] += (previousYPosRight[1] - localPosRightCube.x);
                totalTestRangeRight[2] += (previousYPosRight[2] - localPosRightCube.z);
                previousYPosRight[0] = localPosRightCube.y;
                previousYPosRight[1] = localPosRightCube.x;
                previousYPosRight[2] = localPosRightCube.z;
            }

            amtTestedRight++;
        }
        else
        {
            averageRight[0] = totalTestRangeRight[0] / amtTestedRight;
            averageRight[1] = totalTestRangeRight[1] / amtTestedRight;
            averageRight[2] = totalTestRangeRight[2] / amtTestedRight;
            amtTestedRight = 0;
            totalTestRangeRight[0] = 0;
            totalTestRangeRight[1] = 0;
            totalTestRangeRight[2] = 0;
            
            if(Mathf.Abs(distanceRightX) > Mathf.Abs(distanceRightZ))
            {                    
                if(distanceRightX > 0.1)
                {
                    averageRight[1] -= 0.5f * averageRight[0];
                }
                else if (distanceRightX < -0.1)
                {
                    averageRight[1] += 0.5f * averageRight[0];
                } 
            }   else
            {
                if (distanceRightZ > 0.1)
                {
                    averageRight[2] -= 0.5f * averageRight[0];
                }
                else if (distanceRightZ < -0.1)
                {
                    averageRight[2] += 0.5f * averageRight[0];
                }
            } 

            GetComponent<Rigidbody>().AddForce(new Vector3(averageRight[1], averageRight[0], averageRight[2]) * force);
        }
        
    }

    void FlyLeft()
    {
        if (amtTestedLeft < testRange)
        {
            //Debug.Log(leftCube.transform.position.y);
            if (amtTestedLeft == 0)
            {
                previousYPosLeft[0] = localPosLeftCube.y;
                previousYPosLeft[1] = localPosLeftCube.x;
                previousYPosLeft[2] = localPosLeftCube.z;
            }
            else
            {
                if (Mathf.Abs(distanceLeftX) > Mathf.Abs(distanceLeftZ) && distanceLeftX < maxDistanceWings)
                {
                    totalTestRangeLeft[0] += ((previousYPosLeft[0] - localPosLeftCube.y) * (Mathf.Abs(distanceLeftX) / maxDistanceWings));
                }
                else if(Mathf.Abs(distanceLeftX) <= Mathf.Abs(distanceLeftZ) && distanceLeftZ < maxDistanceWings)
                {
                    totalTestRangeLeft[0] += ((previousYPosLeft[0] - localPosLeftCube.y) * (Mathf.Abs(distanceLeftZ) / maxDistanceWings));
                }
                else
                {
                    totalTestRangeLeft[0] += (previousYPosLeft[0] - localPosLeftCube.y);
                }

                totalTestRangeLeft[1] += (previousYPosLeft[1] - localPosLeftCube.x);
                totalTestRangeLeft[2] += (previousYPosLeft[2] - localPosLeftCube.z);
                previousYPosLeft[0] = localPosLeftCube.y;
                previousYPosLeft[1] = localPosLeftCube.x;
                previousYPosLeft[2] = localPosLeftCube.z;
            }

            amtTestedLeft++;
        }
        else
        {
            averageLeft[0] = totalTestRangeLeft[0] / amtTestedLeft;
            averageLeft[1] = totalTestRangeLeft[1] / amtTestedLeft;
            averageLeft[2] = totalTestRangeLeft[2] / amtTestedLeft;
           
            amtTestedLeft = 0;
            totalTestRangeLeft[0] = 0;
            totalTestRangeLeft[1] = 0;
            totalTestRangeLeft[2] = 0;

            if (Mathf.Abs(distanceLeftX) > Mathf.Abs(distanceLeftZ))
            {

                if (distanceLeftX > 0.1)
                {
                    averageLeft[1] -= 0.5f * averageLeft[0];
                }
                else if (distanceLeftX < -0.1)
                {
                    averageLeft[1] += 0.5f * averageLeft[0];
                } 
            }
             else
            {

                if (distanceLeftZ > 0.1)
                {
                    averageLeft[2] -= 0.5f * averageLeft[0];
                }
                else if (distanceLeftZ < -0.1)
                {
                    averageLeft[2] += 0.5f * averageLeft[0];
                }
            } 

            GetComponent<Rigidbody>().AddForce(new Vector3(averageLeft[1], averageLeft[0], averageLeft[2]) * force);
        }
    }
}
