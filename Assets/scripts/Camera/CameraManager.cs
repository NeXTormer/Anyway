﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject[] cameraPositions;

    public int currentCamera = 0;

    public void Start()
    {
		foreach(GameObject gobj in cameraPositions)
        {
            gobj.GetComponent<CameraInfo>().isActive = false;
        }

        cameraPositions[currentCamera].GetComponent<CameraInfo>().isActive = true;
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
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
