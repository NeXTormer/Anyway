using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera Control/Static Camera")]
[RequireComponent(typeof(CameraInfo))]
public class StaticCamera : MonoBehaviour
{
    public GameObject player;

    private CameraInfo m_CameraInfo;

	void Start()
    {
        m_CameraInfo = GetComponent<CameraInfo>();
	}

    void LateUpdate()
    {
        if (m_CameraInfo.isActive)
        {
            m_CameraInfo.cameraContainer.transform.position = this.transform.position;
            m_CameraInfo.cameraContainer.transform.rotation = this.transform.rotation;
        }
	}
}
