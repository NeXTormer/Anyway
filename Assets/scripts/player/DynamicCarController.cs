using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axle
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
    public float steeringModifier = 0;

    [Tooltip("Modifies the direction and speed of the wheel rotation\n0 -> No rotation\n1 -> Normal rotation\n-1 -> Inverted rotaion\n2 -> Double torque normal rotation")]
    public float wheelRotaionModifier = 1;
}

[AddComponentMenu("Player/DynamicCarController")]
public class DynamicCarController : MonoBehaviour 
{
    public List<Axle> axles;

    [Header("Settings")]
    public float motorTorqueMax = 300;
    public float motorDirectionModifier = 1;
    public float topSpeed = 55;

    public float brakeTorqueMax = 30000;
    public float steeringAngleMax = 35;

    public float criticalSpeed = 5f;
    public int stepsBelow = 5;
    public int stepsAbove = 1;

    public Transform centerOfMass;


    [Header("Steering Wheel")]
    [Tooltip("Mesh of the steering wheel to move it when steering")]
    public GameObject steeringWheel;
    public float maxSteeringWheelAngle = 450;
    public float steeringWheelAngle;



    private Rigidbody m_Rigidbody;
    private PlayerInputManager m_InputManager;


	void Start () 
    {
        m_InputManager = GetComponent<PlayerInputManager>();
        m_Rigidbody = GetComponent<Rigidbody>();

        axles[0].rightWheel.attachedRigidbody.centerOfMass = centerOfMass.localPosition;
        //Debug.Log("CENTER OF MASS: " + m_Rigidbody.centerOfMass);
    }

    void FixedUpdate () 
    {
        float motorTorque = motorTorqueMax * m_InputManager.gas;
        if (m_InputManager.clutch > 0.5) motorTorque = -m_InputManager.clutch * motorTorqueMax;
        motorTorque *= motorDirectionModifier;
        
        float steeringAngle = steeringAngleMax * m_InputManager.steering;
        steeringWheelAngle = m_InputManager.steering * maxSteeringWheelAngle;
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, steeringWheelAngle);


        float brakeTorque = m_InputManager.brake * brakeTorqueMax;
        if (brakeTorque < 10.0f) brakeTorque = 0.0f;
        
        foreach (Axle a in axles)
        {
            a.leftWheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
            a.rightWheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

            a.leftWheel.brakeTorque = brakeTorque;
            a.rightWheel.brakeTorque = brakeTorque;

            if (a.handBrakeAttached)
            {
                if (m_InputManager.handBrake == 1)
                {
                    a.leftWheel.brakeTorque = brakeTorqueMax;
                    a.rightWheel.brakeTorque = brakeTorqueMax;
                }
            }
            if(a.motorAttached)
            {
                a.leftWheel.motorTorque = motorTorque * a.wheelRotaionModifier;
                a.rightWheel.motorTorque = motorTorque * a.wheelRotaionModifier;
            }
            a.leftWheel.steerAngle = steeringAngle * a.steeringModifier;
            a.rightWheel.steerAngle = steeringAngle * a.steeringModifier;
        }


        float velocity = Mathf.Abs(m_Rigidbody.velocity.magnitude * 3.6f);
       
        if(velocity < 25.0f)
        {
            float inverseVelocity = 25 - velocity;
            m_InputManager.PlayDamperForce(inverseVelocity * 3);
        }

        int springMagnitude = (int) (velocity * 0.55f);
        int springCoefficient = 55;
        if(velocity > 10)
        {
            m_InputManager.PlaySpringForce(0, (int)velocity, 50);
        }
        else
        {
            LogitechGSDK.LogiStopSpringForce(0);
        }
        
        foreach(Axle a in axles)
        {
            if(a.steeringModifier > 0.5f)
            {
                if(!a.rightWheel.isGrounded && !a.leftWheel.isGrounded)
                {
                    if(m_InputManager.forceFeedBackEnabled)
                    {
                        LogitechGSDK.LogiPlayCarAirborne(0);
                    }
                }
                else
                {
                    if(m_InputManager.forceFeedBackEnabled)
                    {
                        LogitechGSDK.LogiStopCarAirborne(0);
                    }
                }
            }
        }

    }
}
