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

[System.Serializable]
public struct AccelerationHelperSpeeds
{
    public float acceleration0;

    public float speed1;
    public float acceleration1;

    public float speed2;
    public float acceleration2;

    public float speed3;
    public float acceleration3;

    public float speed4;
    public float acceleration4;

    public float speed5;
    public float acceleration5;

    public float speed6;
    public float acceleration6;
}


public class CarController : MonoBehaviour
{

    public List<WheelPair> axles;

    [Header("Settings")]
    public float motorTorqueMax = 3000;
    public float motorDirectionModifier = 1;
    public float maxSpeed = 55;

    public float handbrakeTorqueMax = 100000000;
    public float steeringAngleMax = 30;

    [Tooltip("The force applied to the vehicle while moving. Protortional to the speed")]
    public float downwardsForce = 100;

    [Tooltip("A lower center of mass makes the vehicle more stable")]
    public Transform centerOfMass;

    [Header("Car Assists")]
    [Range(0, 1)]
    public float steerHelper = 0.6f;

    [Range(0, 1)]
    public float tractionControl = 0.5f;

    [Range(0, 1)]
    public float slipLimit = 0.3f;

    [Range(0, 1)]
    public float hardSlipLimit = 0.87f;
    public float hardSlipTorqueModifier = 25;


    public AccelerationHelperSpeeds accelerationHelperSettings;

    [Header("Steering Wheel")]
    [Tooltip("Mesh of the steering wheel to move it when steering")]
    public GameObject steeringWheel;

    [Tooltip("Multiplier between the rotation of the wheels and the rotation of the steering wheel")]
    public float steeringWheelModifier = 10;

    [Header("Info")]
    public float speed = 0;
    public float forwardSlip = 0;

    public float currentTorque = 0;

    private float steeringAngleOld = 0;
    private Rigidbody body;
    private float oldRotationY = 0;


	public void Start ()
    {
        steeringAngleOld = steeringWheel.transform.localEulerAngles.z;
        body = GetComponent<Rigidbody>();
        axles[0].rightWheel.attachedRigidbody.centerOfMass = centerOfMass.localPosition;

        currentTorque = motorTorqueMax;
    }
	

    public void FixedUpdate()
    { 
        float motorTorque = currentTorque * Input.GetAxis("Vertical");
        float steeringAngle = steeringAngleMax * Input.GetAxis("Horizontal");
        float handBrakeTorque = handbrakeTorqueMax * Input.GetAxis("Jump");

        motorTorque *= motorDirectionModifier;

        SteerHelper();

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


        //add downforce relative to speed
        axles[0].rightWheel.attachedRigidbody.AddForce(this.transform.up * -1 * downwardsForce * speed);

        TractionControl();
        AccelerationHelper();

        if(body.velocity.magnitude > maxSpeed)
        {
            body.velocity = maxSpeed * body.velocity.normalized;
        }

    }

    //TODO: add support for more than two axles
    private void SteerHelper()
    {
        WheelHit wheelhit;
        axles[0].rightWheel.GetGroundHit(out wheelhit);
        if(wheelhit.normal == Vector3.zero)
        {
            return;
        }

        axles[0].leftWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return;
        }

        axles[1].rightWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return;
        }

        axles[1].leftWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return;
        }

        //avoid gimbal lock
        if(Mathf.Abs(oldRotationY - transform.eulerAngles.y) < 10.0f)
        {
            float adjust = ((transform.eulerAngles.y - oldRotationY)) * steerHelper;
            Quaternion rotation = Quaternion.AngleAxis(adjust, Vector3.up);
            body.velocity = rotation * body.velocity;
            
            

        }
        oldRotationY = transform.eulerAngles.y;
    }

    //TODO: add support for more than two axles
    private void TractionControl()
    {
        WheelHit wheelhit;

        if(axles[0].motorAttached)
        {
            axles[0].rightWheel.GetGroundHit(out wheelhit);
            AdjustTorque(wheelhit.forwardSlip);

            axles[0].leftWheel.GetGroundHit(out wheelhit);
            AdjustTorque(wheelhit.forwardSlip);
        }
        if (axles[1].motorAttached)
        {
            axles[1].rightWheel.GetGroundHit(out wheelhit);
            AdjustTorque(wheelhit.forwardSlip);

            axles[1].leftWheel.GetGroundHit(out wheelhit);
            AdjustTorque(wheelhit.forwardSlip);
        }
    }

    //when the forward slip nears 1 when accelerating the car moves very slowly. this adds some exta acceleration force for faster acceleration
    private void AccelerationHelper()
    {
        if (Input.GetAxis("Vertical") > 0.5)
        {
            if (speed > accelerationHelperSettings.speed6)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration6);
                Debug.Log(Time.time + "| " + "Speed6");
            }
            else if (speed > accelerationHelperSettings.speed5)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration5);
                Debug.Log(Time.time + "| " + "Speed5");
            }
            else if (speed > accelerationHelperSettings.speed4)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration4);
                Debug.Log(Time.time + "| " + "Speed4");
            }
            else if (speed > accelerationHelperSettings.speed3)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration3);
                Debug.Log(Time.time + "| " + "Speed3");
            }
            else if (speed > accelerationHelperSettings.speed2)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration2);
                Debug.Log(Time.time + "| " + "Speed2");
            }
            else if (speed > accelerationHelperSettings.speed1)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration1);
                Debug.Log(Time.time + "| " + "Speed1");
            }
            else if (speed < accelerationHelperSettings.speed1)
            {
                body.velocity = body.velocity.normalized * (body.velocity.magnitude + accelerationHelperSettings.acceleration0);
                Debug.Log(Time.time + "| " + "Speed0");
            }
        }
    }

    private void AdjustTorque(float slip)
    {
        slip *= motorDirectionModifier;
        forwardSlip = slip;

        if(slip >= slipLimit && currentTorque >= 0)
        {
            if(slip >= hardSlipLimit)
            {
                currentTorque -= hardSlipTorqueModifier * tractionControl;
            }
            else
            {
                currentTorque -= 10 * tractionControl;
            }

            if(currentTorque < 0)
            {
                currentTorque = 0;
            }
            
        }
        else
        {
            currentTorque += 10 * tractionControl;

            if(currentTorque > motorTorqueMax)
            {
                currentTorque = motorTorqueMax;
            }
        }
    }
}