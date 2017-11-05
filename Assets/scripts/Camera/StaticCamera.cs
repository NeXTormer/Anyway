using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraInfo))]
public class StaticCamera : MonoBehaviour
{
    public GameObject player;

    private CameraInfo camerainfo;


	void Start()
    {
        camerainfo = GetComponent<CameraInfo>();
	}

    void LateUpdate()
    {
        if (camerainfo.isActive)
        {
            camerainfo.cameraContainer.transform.position = this.transform.position;
            camerainfo.cameraContainer.transform.rotation = this.transform.rotation;
        }
	}
}
