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

    [Header("Maximum Values")]
    public float minDistance = 4;
    public float maxDistance = 50;

    

    public float minHeight = 1;
    public float maxHeight = 50;

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
            float zoom = (-8) * Input.GetAxis("Mouse ScrollWheel");
            
            height += zoom;
            distance += zoom * 2.4f; 

            if (height < minHeight) height = minHeight;
            if (height > maxHeight) height = maxHeight;

            if (distance < minDistance) distance = minDistance;
            if (distance > maxDistance) distance = maxDistance;
            
            

            this.transform.localPosition = new Vector3(0, height, distance);
            this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);

            Vector3 targetPos = this.transform.position;
            
            caminfo.cameraContainer.transform.position = Vector3.Lerp(caminfo.cameraContainer.transform.position, this.transform.position, Time.deltaTime * damping);


            caminfo.cameraContainer.transform.LookAt(player.transform);

        }
    }

}
