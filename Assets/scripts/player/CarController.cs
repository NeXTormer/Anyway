using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

//TODO: add curve field instead of class (https://docs.unity3d.com/ScriptReference/EditorGUILayout.CurveField.html)
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

[AddComponentMenu("Player/CarController")]
public class CarController : NetworkBehaviour
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

    [Range(0, 1)]
    [Tooltip("A higher value means less brake helping.")]
    public float brakeHelper = 0.94f;

    public AccelerationHelperSpeeds accelerationHelperSettings;

    [Header("Steering Wheel")]

    [Tooltip("Mesh of the steering wheel to move it when steering")]
    public GameObject steeringWheel;

    [Tooltip("Multiplier between the rotation of the wheels and the rotation of the steering wheel")]
    public float steeringWheelModifier = 10;

    [Header("Info")]
    public float speed = 0;
    public Vector3 speedDirection = new Vector3();
    public float forwardSlip = 0;

    public float currentTorque = 0;

    private float m_OldSteeringAngle = 0;
    private Rigidbody m_Body;
    private float m_OldRotationY = 0;
    private PlayerInputManager m_InputManager;
    private Vector3 m_OldVelocity = new Vector3(0, 0, 0);


	public void Start ()
    {
        m_OldSteeringAngle = steeringWheel.transform.localEulerAngles.z;
        m_Body = GetComponent<Rigidbody>();
        axles[0].rightWheel.attachedRigidbody.centerOfMass = centerOfMass.localPosition;
        m_InputManager = GetComponent<PlayerInputManager>();
        currentTorque = motorTorqueMax;
        
    }
	

    public void FixedUpdate()
    {
        float motorTorque = currentTorque * m_InputManager.gas;
        if (m_InputManager.clutch > 0.5) motorTorque = -m_InputManager.clutch * currentTorque;
        float steeringAngle = steeringAngleMax * m_InputManager.steering;

        float handBrakeTorque;
        if (m_InputManager.HandBrakeDown())
        {
            handBrakeTorque = float.MaxValue;
        }
        else if (m_InputManager.brake > 0.3)
        {
            handBrakeTorque = handbrakeTorqueMax * m_InputManager.brake;
        }
        else
        {
            handBrakeTorque = 0;
        }

        speedDirection = this.transform.InverseTransformDirection(m_Body.velocity.normalized);


        //apply an extra brake force when driving backwards with a forward velocity
        if (motorTorque < -1)
        {
            if (speedDirection.z < -0.1)
            {
                if(IsOnGround())
                {
                    //m_Body.velocity = m_Body.velocity.magnitude * brakeHelper * m_Body.velocity.normalized;
                    //handBrakeTorque = handbrakeTorqueMax;
                }
            }
        }
        else if (motorTorque > 1)
        {
            if (speedDirection.z > 0.1)
            {
                if(IsOnGround())
                {
                    //m_Body.velocity = m_Body.velocity.magnitude * brakeHelper * m_Body.velocity.normalized;
                    //handBrakeTorque = handbrakeTorqueMax;
                }
            }
        }
        

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

        }

        //rotate steering wheel
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, (steeringAngle - m_OldSteeringAngle) * steeringWheelModifier);



        //add downforce relative to speed
        m_Body.AddForce(this.transform.up * -1 * downwardsForce * speed);

        //TractionControl();
        //AccelerationHelper();

        if(handBrakeTorque > 10)
        {
            if(IsOnGround())
            {
                m_Body.velocity = m_Body.velocity.magnitude * brakeHelper * m_Body.velocity.normalized;
            }
        }

       

        if(m_Body.velocity.magnitude > maxSpeed)
        {
            m_Body.velocity = maxSpeed * m_Body.velocity.normalized;
        }

        
        speed = m_Body.velocity.magnitude;

        m_InputManager.PlayLEDs((int) speed, 3, (int) (maxSpeed - 1));
        //m_InputManager.PlayDamperForce(m_Body.velocity.magnitude * 0.4f); damper should be inverted: slow speed -> high damper, high speed -> low damper but high spring force


        Vector3 diff = m_OldVelocity - m_Body.velocity;
        if (diff.z < 0.1 && diff.z > 0) diff.z = 0;
        if (diff.z > -0.1 && diff.z < 0) diff.z = 0;

        m_InputManager.PlayConstantForce((int)(diff.z * 88));
        Debug.Log("Velocity DIff: " + diff.z * 88);

        m_OldVelocity = m_Body.velocity;
    }

    /// <summary>
    ///     Assists with steering by changing the direction of the velocity vector.
    /// </summary>
    private void SteerHelper()
    {

        if (!IsOnGround()) return;

        //avoid gimbal lock
        if(Mathf.Abs(m_OldRotationY - transform.eulerAngles.y) < 10.0f)
        {
            float adjust = ((transform.eulerAngles.y - m_OldRotationY)) * steerHelper;
            Quaternion rotation = Quaternion.AngleAxis(adjust, Vector3.up);
            m_Body.velocity = rotation * m_Body.velocity;
            
        }
        m_OldRotationY = transform.eulerAngles.y;
    }

    private bool IsOnGround()
    {
        WheelHit wheelhit;
        axles[0].rightWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return false;
        }

        axles[0].leftWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return false;
        }

        axles[1].rightWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return false;
        }

        axles[1].leftWheel.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Adjusts the torque for each wheel based on the current slip in order to prevent wheelspin.
    /// </summary>
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

    /// <summary>
    ///     Applies some extra force to the car when the slip nears 1 to speed up the acceleration of the car.
    /// </summary>
    private void AccelerationHelper()
    {
        if (m_InputManager.gas > 0.5)
        {
            if (speed > accelerationHelperSettings.speed6)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration6);
            }
            else if (speed > accelerationHelperSettings.speed5)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration5);
            }
            else if (speed > accelerationHelperSettings.speed4)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration4);
            }
            else if (speed > accelerationHelperSettings.speed3)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration3);
            }
            else if (speed > accelerationHelperSettings.speed2)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration2);
            }
            else if (speed > accelerationHelperSettings.speed1)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration1);
            }
            else if (speed < accelerationHelperSettings.speed1)
            {
                m_Body.velocity = m_Body.velocity.normalized * (m_Body.velocity.magnitude + accelerationHelperSettings.acceleration0);
            }
        }
    }

    /// <summary>
    ///     Adjusts the torque of the motor based on the current slip of a wheel.
    /// </summary>
    /// <param name="slip">
    ///     Current wheel slip.
    /// </param>
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