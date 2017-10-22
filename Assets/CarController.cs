using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelPair
{
    public string name;

    [Header("WheelColliders")]
    public WheelCollider rightWheel;
    public WheelCollider leftWheel;

    [Header("Meshes")]
    public GameObject rightWheelMesh;
    public GameObject leftWheelMesh;

    [Header("Settings")]
    [Tooltip("True if the axis should be powered by a motor")]
    public bool motorAttached;

    [Tooltip("1 -> Normal steering \n0 -> No steering\n-1 -> Inverted steering")]
    [Range(-1, 1)]
    public float steering;
}



public class CarController : MonoBehaviour {

    public List<WheelPair> axles;

    [Header("Settings")]
    public float motorTorqueMax = 10;
    public float steeringAngleMax = 70;


	public void Start () {
		
	}
	
	public void Update () {
		
	}


    //Physics updates
    public void FixedUpdate()
    {
        float motorTorque = motorTorqueMax * Input.GetAxis("Vertical");
        float steeringAngle = steeringAngleMax * Input.GetAxis("Horizontal");

        foreach (WheelPair wheelpair in axles)
        {
            wheelpair.leftWheel.steerAngle = wheelpair.steering * steeringAngle;
            wheelpair.rightWheel.steerAngle = wheelpair.steering * steeringAngle;

            if(wheelpair.motorAttached)
            {
                wheelpair.rightWheel.motorTorque = motorTorque;
                wheelpair.leftWheel.motorTorque = motorTorque;
            }

            UpdateWeelMeshTransform(wheelpair);
        }
    }

    public void UpdateWeelMeshTransform(WheelPair wheelpair)
    {
        wheelpair.rightWheelMesh.transform.Rotate(Vector3.right, wheelpair.rightWheel.rpm * 10 * Time.deltaTime, Space.Self);
        wheelpair.leftWheelMesh.transform.Rotate(Vector3.right, wheelpair.leftWheel.rpm * 10 * Time.deltaTime, Space.Self);
    }
}
