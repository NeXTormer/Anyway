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
    [Tooltip("Checked if this axle should be powered by a motor")]
    public bool motorAttached;

    [Tooltip("Checked if the handbrake should work with this axle")]
    public bool handBrakeAttached = false;


    [Tooltip("1 -> Normal steering \n0 -> No steering\n-1 -> Inverted steering")]
    [Range(-1, 1)]
    public float steering = 1;

    [Tooltip("Modifies the direction and speed of the wheel rotation\n0 -> No rotation\n1 -> Normal rotation\n-1 -> Inverted rotaion\n2 -> Double speed normal rotation")]
    public float wheelRotaionModifier = 1;


    [HideInInspector]
    public float lastSteeringAngle = 0;
}



public class CarController : MonoBehaviour {

    public List<WheelPair> axles;

    [Header("Settings")]
    public float motorTorqueMax = 500;
    
    public float handbrakeTorqueMax = 2000;
    public float steeringAngleMax = 40;
    


	public void Start () {
		
	}
	
	public void Update () {
		
	}


    //Physics updates
    public void FixedUpdate()
    {
        float motorTorque = motorTorqueMax * Input.GetAxis("Vertical");
        float steeringAngle = steeringAngleMax * Input.GetAxis("Horizontal");
        float handBrakeTorque = handbrakeTorqueMax * Input.GetAxis("Jump");

        

        foreach (WheelPair wheelpair in axles)
        {
            wheelpair.lastSteeringAngle = steeringAngle;
            wheelpair.leftWheel.steerAngle = wheelpair.steering * steeringAngle;
            wheelpair.rightWheel.steerAngle = wheelpair.steering * steeringAngle;
            
            if(wheelpair.motorAttached)
            {
                wheelpair.rightWheel.motorTorque = motorTorque;
                wheelpair.leftWheel.motorTorque = motorTorque;
            }

            if(wheelpair.handBrakeAttached)
            {
                wheelpair.rightWheel.brakeTorque = handBrakeTorque;
                wheelpair.leftWheel.brakeTorque = handBrakeTorque;
            }

            //Update the rotation of the wheel meshes to show the wheel movements

            //motorrotation
            wheelpair.rightWheelMesh.transform.Rotate(Vector3.right, wheelpair.rightWheel.rpm / 60 * 360 * wheelpair.wheelRotaionModifier * Time.deltaTime, Space.Self);
            wheelpair.leftWheelMesh.transform.Rotate(Vector3.right, wheelpair.leftWheel.rpm / 60 * 360 * wheelpair.wheelRotaionModifier * Time.deltaTime, Space.Self);

            //steeringrotation
            wheelpair.rightWheelMesh.transform.localEulerAngles = new Vector3(wheelpair.rightWheelMesh.transform.localEulerAngles.x, wheelpair.rightWheel.steerAngle - wheelpair.rightWheelMesh.transform.localEulerAngles.z + 180, wheelpair.rightWheelMesh.transform.localEulerAngles.z);
            wheelpair.leftWheelMesh.transform.localEulerAngles = new Vector3(wheelpair.leftWheelMesh.transform.localEulerAngles.x, wheelpair.leftWheel.steerAngle - wheelpair.leftWheelMesh.transform.localEulerAngles.z + 180, wheelpair.leftWheelMesh.transform.localEulerAngles.z);

        }
    }

    
}
