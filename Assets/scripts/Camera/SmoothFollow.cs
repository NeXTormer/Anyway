using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[AddComponentMenu("Camera Control/SmoothFollow")]
[RequireComponent(typeof(CameraInfo))]
class SmoothFollow : MonoBehaviour
{
    public GameObject player;

    [Header("Camera Settings")]
    [Tooltip("Horizontal distance to the target")]
    public float distance = 10;

    [Tooltip("Vertical distance to the target")]
    public float height = 5;

    public float heightDamping = 2;
    public float rotationDamping = 3;


    private CameraInfo caminfo;
    private Transform cameraTransform;
    private Transform targetTransform;

    public void Start()
    {
        caminfo = GetComponent<CameraInfo>();
        cameraTransform = caminfo.cameraContainer.transform;
        targetTransform = player.transform;
    }

    public void LateUpdate()
    {
        if(caminfo.isActive)
        {
            float targetRotation = targetTransform.eulerAngles.y;
            float targetHeight = targetTransform.position.y + height;

            float currentRotation = cameraTransform.eulerAngles.y;
            float currentHeight = targetTransform.position.y;

            currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotationQ = Quaternion.Euler(0, currentRotation, 0);

            cameraTransform.position = targetTransform.position;
            cameraTransform.position -= currentRotationQ * -Vector3.forward * distance;

            cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);

            cameraTransform.LookAt(targetTransform);

        }
    }

}
