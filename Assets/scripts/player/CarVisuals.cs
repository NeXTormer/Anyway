using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

class CarVisuals : NetworkBehaviour
{
    public WheelCollider WheelFR;
    public WheelCollider WheelFL;
    public WheelCollider WheelBR;
    public WheelCollider WheelBL;

    [Space]

    public GameObject WheelFRMesh;
    public GameObject WheelFLMesh;
    public GameObject WheelBRMesh;
    public GameObject WheelBLMesh;

    [Space]

    public float wheelRotationModifier = -1;


    public void FixedUpdate()
    {
        /* Front */
        /* Motor */
        WheelFRMesh.transform.Rotate(Vector3.right, WheelFR.rpm / 60 * 360 * wheelRotationModifier * Time.deltaTime, Space.Self);
        WheelFLMesh.transform.Rotate(Vector3.right, WheelFL.rpm / 60 * 360 * wheelRotationModifier * Time.deltaTime, Space.Self);

        /* Steering wheel */
        WheelFRMesh.transform.localEulerAngles = new Vector3(WheelFRMesh.transform.localEulerAngles.x, WheelFR.steerAngle - WheelFRMesh.transform.localEulerAngles.z + 180, WheelFRMesh.transform.localEulerAngles.z);
        WheelFLMesh.transform.localEulerAngles = new Vector3(WheelFLMesh.transform.localEulerAngles.x, WheelFL.steerAngle - WheelFLMesh.transform.localEulerAngles.z + 180, WheelFLMesh.transform.localEulerAngles.z);

        /* Back */
        /* Motor */
        WheelBRMesh.transform.Rotate(Vector3.right, WheelBR.rpm / 60 * 360 * wheelRotationModifier * Time.deltaTime, Space.Self);
        WheelBLMesh.transform.Rotate(Vector3.right, WheelBL.rpm / 60 * 360 * wheelRotationModifier * Time.deltaTime, Space.Self);

        /* Steering wheel */
        WheelBRMesh.transform.localEulerAngles = new Vector3(WheelBRMesh.transform.localEulerAngles.x, WheelBR.steerAngle - WheelBRMesh.transform.localEulerAngles.z + 180, WheelBRMesh.transform.localEulerAngles.z);
        WheelBLMesh.transform.localEulerAngles = new Vector3(WheelBLMesh.transform.localEulerAngles.x, WheelBL.steerAngle - WheelBLMesh.transform.localEulerAngles.z + 180, WheelBLMesh.transform.localEulerAngles.z);


    }
}
