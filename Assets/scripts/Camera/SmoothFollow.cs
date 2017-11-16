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

    public float damping = 2;


    private CameraInfo caminfo;


    public void Start()
    {
        caminfo = GetComponent<CameraInfo>();
    }

    public void FixedUpdate()
    {
        if(caminfo.isActive)
        { 
            Vector3 targetPos = this.transform.position;
            
            caminfo.cameraContainer.transform.position = Vector3.Lerp(caminfo.cameraContainer.transform.position, this.transform.position, Time.deltaTime * damping);

            caminfo.cameraContainer.transform.LookAt(player.transform);

        }
    }

}