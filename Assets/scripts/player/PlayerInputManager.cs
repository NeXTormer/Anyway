using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    STEERINGWHEEL, KEYBOARD
}

[RequireComponent(typeof(NetworkPlayerData))]
[AddComponentMenu("Player/Player Input Manager")]
public class PlayerInputManager : MonoBehaviour
{
    [Header("Settings")]
    public InputType inputType = InputType.KEYBOARD;

    [Space]

    [Tooltip("how long [s] the reset button has to be pressed in order to reset.")]
    public float carResetPressTime = 0.5f;

    [Header("Axes")]
    public string AXIS_GAS = "Vertical";
    public string AXIS_STEERING = "Horizontal";
    public string AXIS_HANDBRAKE = "Jump";
    public string AXIS_CAMERAZOOM = "Mouse ScrollWheel";

    [Header("Keys")]
    public KeyCode KEY_CHANGEPOV = KeyCode.V;
    public KeyCode KEY_HANDBRAKE = KeyCode.Space;
    public KeyCode KEY_PAUSEMENU = KeyCode.Escape;
    public KeyCode KEY_RESETCAR = KeyCode.R;

    [Header("Values")]
    public float gas;
    public float steering;
    public float brake;
    public float clutch;
    public float handBrake;

    [Header("CameraZoom")]
    public float cameraZoom;

    public float cameraZoomValue = 6;
    public float cameraZoomMax = 20;
    public float cameraZoomMin = 1;

    private LogitechGSDK.DIJOYSTATE2ENGINES m_State;

    private bool m_NewChangePOV = false;
    private bool m_OldChangePOV = false;
    private bool m_ChangePOV = false;
    private bool m_ResetCarTempBool = false;
    private bool m_ResetCarValue;

    private float m_ResetCarTimer = 0;

    private NetworkPlayerData m_Playerdata;
    private PlayerDataTransfer m_PlayerDataTransfer;

    public void Start()
    {
        m_Playerdata = GetComponent<NetworkPlayerData>();
        m_PlayerDataTransfer = GameObject.FindGameObjectWithTag("PlayerDataTransfer").GetComponent<PlayerDataTransfer>();

        inputType = m_PlayerDataTransfer.useSteeringWheel ? InputType.STEERINGWHEEL : InputType.KEYBOARD;

        Debug.Log("Logitech Steering Wheel: " + LogitechGSDK.LogiSteeringInitialize(false));
    }

    public void FixedUpdate()
    {

        if (inputType == InputType.KEYBOARD)
        {
            /* movement is disabled when debugMode is off and the race is actve */
            if(!(!m_Playerdata.debugMode && !m_Playerdata.raceActive))
            {
                gas = Input.GetAxis(AXIS_GAS);
                //Debug.Log(gas);
            }
            else
            {
                gas = 0;
            }
            
            steering = Input.GetAxis(AXIS_STEERING);
            handBrake = Input.GetAxis(AXIS_HANDBRAKE);
            brake = 0;

            cameraZoom = Input.GetAxis(AXIS_CAMERAZOOM);
            
        }
        else
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                m_State = LogitechGSDK.LogiGetStateUnity(0);

                /* movement is disabled when debugMode is off and the race is actve */
                if (!(!m_Playerdata.debugMode && !m_Playerdata.raceActive))
                {
                    gas = -((m_State.lY - 32768.0f) / (32768.0f * 2f));
                    clutch = -((m_State.rglSlider[0] - 32768.0f) / (32768.0f * 2f));
                }
                else
                {
                    gas = 0;
                    clutch = 0;
                }

                steering = m_State.lX / 32768.0f;
                brake = -((m_State.lRz - 32768.0f) / (32768.0f * 2f));

                cameraZoomValue = m_State.rgbButtons[22] == 128 ? cameraZoomValue + 0.1f : cameraZoomValue;
                cameraZoomValue = m_State.rgbButtons[21] == 128 ? cameraZoomValue - 0.1f : cameraZoomValue;
                if (cameraZoomValue < cameraZoomMin) cameraZoomValue = cameraZoomMin;
                if (cameraZoomValue > cameraZoomMax) cameraZoomValue = cameraZoomMax;

                m_OldChangePOV = m_NewChangePOV;
                m_NewChangePOV = m_State.rgbButtons[2] == 128;

                m_ChangePOV = !m_OldChangePOV == m_NewChangePOV;

            }
        }

        if(Input.GetKeyDown(KEY_RESETCAR))
        {
            m_ResetCarTempBool = true;
            m_ResetCarTimer = Time.time;
        }

        if(Input.GetKey(KEY_RESETCAR))
        {
            if (m_ResetCarTempBool)
            {
                if (Time.time - m_ResetCarTimer > carResetPressTime)
                {
                    m_ResetCarValue = true;
                    m_ResetCarTempBool = false;
                }
            }
        }
        else
        {
            m_ResetCarValue = false;
        }
    }

    public bool ResetCar()
    {
        bool tmp = m_ResetCarValue;
        m_ResetCarValue = false;
        
        return tmp;
    }

    public bool ChangeCameraPosition()
    {
        return Input.GetKeyDown(KEY_CHANGEPOV);
    }

    public bool TogglePauseMenu()
    {
        return Input.GetKeyDown(KEY_PAUSEMENU);
    }

    public bool HandBrakeDown()
    {
        return Input.GetKeyDown(KEY_HANDBRAKE);
    }

    public void PlayLEDs(int currentValue, int firstLEDValue, int almostmaxLEDValue)
    {
        if(inputType == InputType.STEERINGWHEEL)
        {
            LogitechGSDK.LogiPlayLeds(0, currentValue, firstLEDValue, almostmaxLEDValue);
        }
    }
}
