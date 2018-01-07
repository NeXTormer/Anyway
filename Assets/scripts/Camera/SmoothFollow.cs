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


    private CameraInfo m_CamInfo;
    private PlayerInputManager m_InputManager;

    public void Start()
    {
        m_CamInfo = GetComponent<CameraInfo>();
        m_InputManager = this.transform.root.gameObject.GetComponent<PlayerInputManager>();
    }

    public void FixedUpdate()
    {
        if(m_CamInfo.isActive)
        {
            float zoom = (-8) * Input.GetAxis("Mouse ScrollWheel");
            
            height += zoom;
            distance += zoom * 2.4f; 

            if (height < minHeight) height = minHeight;
            if (height > maxHeight) height = maxHeight;

            if (distance < minDistance) distance = minDistance;
            if (distance > maxDistance) distance = maxDistance;

            this.transform.localPosition = new Vector3(0, height, distance);
            this.transform.position = new Vector3(this.transform.position.x, player.transform.position.y + height, this.transform.position.z);

            Vector3 targetPos = this.transform.position;
            
            m_CamInfo.cameraContainer.transform.position = Vector3.Lerp(m_CamInfo.cameraContainer.transform.position, this.transform.position, Time.deltaTime * damping);


            m_CamInfo.cameraContainer.transform.LookAt(player.transform);

        }
    }

}
