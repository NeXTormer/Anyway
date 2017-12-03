using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera Control/CameraManager")]
public class CameraManager : MonoBehaviour
{

    public GameObject[] cameraPositions;

    public int currentCamera = 0;

    private PlayerInputManager inputManager;

    public void Start()
    {
		foreach(GameObject gobj in cameraPositions)
        {
            gobj.GetComponent<CameraInfo>().isActive = false;
        }

        inputManager = this.transform.root.gameObject.GetComponent<PlayerInputManager>();

        cameraPositions[currentCamera].GetComponent<CameraInfo>().isActive = true;
	}

    public void Update()
    {
        if(inputManager.ChangeCameraPosition())
        {
            cameraPositions[currentCamera].GetComponent<CameraInfo>().isActive = false;
            ChangeCamera();
            cameraPositions[currentCamera].GetComponent<CameraInfo>().isActive = true;
        }
    }

    private void ChangeCamera()
    {
        currentCamera = (currentCamera + 1) % cameraPositions.Length;
    }
}
