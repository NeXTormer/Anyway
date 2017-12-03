using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    STEERINGWHEEL, KEYBOARD
}

public class PlayerInputManager : MonoBehaviour
{
    [Header("Settings")]
    public InputType inputType = InputType.KEYBOARD;

    [Header("Axes")]
    public string AXIS_GAS = "Vertical";
    public string AXIS_STEERING = "Horizontal";
    public string AXIS_HANDBRAKE = "Jump";
    public string AXIS_CAMERAZOOM = "Mouse ScrollWheel";

    [Header("Keys")]
    public KeyCode KEY_CHANGEPOV = KeyCode.V;

    [Header("Values")]
    public float gas;
    public float steering;
    public float brake;
    public float handBrake;

    [Header("CameraZoom")]
    public float cameraZoom;

    public float cameraZoomValue = 6;
    public float cameraZoomMax = 20;
    public float cameraZoomMin = 1;

    private LogitechGSDK.DIJOYSTATE2ENGINES state;

    private bool m_NewChangePOV = false;
    private bool m_OldChangePOV = false;
    private bool m_ChangePOV = false;

    public void FixedUpdate()
    {
        if(inputType == InputType.KEYBOARD)
        {
            gas = Input.GetAxis(AXIS_GAS);
            steering = Input.GetAxis(AXIS_STEERING);
            handBrake = Input.GetAxis(AXIS_HANDBRAKE);
            brake = 0;

            cameraZoom = Input.GetAxis(AXIS_CAMERAZOOM);
            
        }
        else
        {
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                state = LogitechGSDK.LogiGetStateUnity(0);

                steering = state.lX / 32768;
                gas = -((state.lY - 32768) / 32768);
                brake = -((state.lRz - 32768) / 32768);

                cameraZoomValue = state.rgbButtons[22] == 128 ? cameraZoomValue + 0.1f : cameraZoomValue;
                cameraZoomValue = state.rgbButtons[21] == 128 ? cameraZoomValue - 0.1f : cameraZoomValue;
                if (cameraZoomValue < cameraZoomMin) cameraZoomValue = cameraZoomMin;
                if (cameraZoomValue > cameraZoomMax) cameraZoomValue = cameraZoomMax;

                m_OldChangePOV = m_NewChangePOV;
                m_NewChangePOV = state.rgbButtons[2] == 128;

                m_ChangePOV = !m_OldChangePOV == m_NewChangePOV;

            }
        }

    }

    public bool ChangeCameraPosition()
    {
        if(inputType == InputType.KEYBOARD)
        {
            return Input.GetKeyDown(KEY_CHANGEPOV);
        }
        else
        {
            return m_ChangePOV;
        }
    }
}
