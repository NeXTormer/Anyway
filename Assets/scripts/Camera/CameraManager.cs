using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Settings")]
    public CameraPosition[] cameraPositions;

    [Tooltip("The player on which the camera should act")]
    public GameObject player;

    [Header("Debug")]
    public bool debug = false;

    private int currentCameraIndex = 0;
    private int numberOfCameras = 0;

	public void Awake()
    {
        numberOfCameras = cameraPositions.Length;
        SetCameraControllers();
        foreach(CameraPosition pos in cameraPositions)
        {
            pos.controller.Configure();
        }
	}

    private void SwitchCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % numberOfCameras;
    }

    private void SetCameraControllers()
    {
        foreach (CameraPosition pos in cameraPositions) {
            if (pos.cameraType == CameraType.Static) pos.controller = new StaticCamera(this, player, pos);
        }
    }

	
	public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    //Move the camera after all other calculations have finished
    public void LateUpdate()
    {
        cameraPositions[currentCameraIndex].controller.Update();
    }
}
