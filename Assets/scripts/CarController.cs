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
}



public class CarController : MonoBehaviour
{

    public List<WheelPair> axles;

    [Header("Settings")]
    public float motorTorqueMax = 500;
    
    public float handbrakeTorqueMax = 2000;
    public float steeringAngleMax = 40;

    [Tooltip("The force applied to the vehicle while moving. Protortional to the speed")]
    public float downwardsForce = 100;

    [Tooltip("A lower center of mass makes the vehicle more stable")]
    public Transform centerOfMass;

    [Header("Steering Wheel")]
    [Tooltip("Mesh of the steering wheel to move it when steering")]
    public GameObject steeringWheel;

    [Tooltip("Multiplier between the rotation of the wheels and the rotation of the steering wheel")]
    public float steeringWheelModifier = 10;

    [Header("Info")]
    public float speed = 0;

    private float steeringAngleOld = 0;
    private Rigidbody body;


	public void Start ()
    {
        steeringAngleOld = steeringWheel.transform.localEulerAngles.z;
        body = GetComponent<Rigidbody>();
        axles[0].rightWheel.attachedRigidbody.centerOfMass = centerOfMass.localPosition;
    }
	
	public void Update ()
    {
		
	}


    //Physics updates
    public void FixedUpdate()
    {
        float motorTorque = motorTorqueMax * Input.GetAxis("Vertical");
        float steeringAngle = steeringAngleMax * Input.GetAxis("Horizontal");
        float handBrakeTorque = handbrakeTorqueMax * Input.GetAxis("Jump");

        if(speed > 100)
        {
            motorTorque *= 3;
        }
        else if(speed > 80)
        {
            motorTorque *= 2.6f;
        }
        else if (speed > 60)
        {
            motorTorque *= 2.2f;
        }
        else if (speed > 50)
        {
            motorTorque *= 1.6f;
        }
        else if (speed > 40)
        {
            motorTorque *= 1.4f;
        }
        else if (speed > 20)
        {
            motorTorque *= 1.2f;
        }


        foreach (WheelPair wheelpair in axles)
        {
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

        //rotate steering wheel
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, (steeringAngle - steeringAngleOld) * steeringWheelModifier);

        //speed variable for display in the inspector
        speed = body.velocity.magnitude;


        axles[0].rightWheel.attachedRigidbody.AddForce(this.transform.up * -1 * downwardsForce * speed);
    }



}
