using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmootherFollow : MonoBehaviour {

    public GameObject player;
    public float smoothing = 6.0f;

    private CameraInfo m_CamInfo;
    private PlayerInputManager m_InputManager;

    public void Start()
    {
        m_CamInfo = GetComponent<CameraInfo>();
        m_InputManager = transform.root.gameObject.GetComponent<PlayerInputManager>();
    }

    public void FixedUpdate()
    {
        if (m_CamInfo.isActive)
        {
            m_CamInfo.cameraContainer.transform.position = Vector3.Lerp(m_CamInfo.cameraContainer.transform.position, transform.position, Time.deltaTime * smoothing);
            m_CamInfo.cameraContainer.transform.LookAt(player.transform);
        }
    }
}
