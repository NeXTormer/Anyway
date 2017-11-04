using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject[] cameraPositions;

    public int currentCamera = 0;

    public void Start()
    {
		
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeCamera();
        }
	}

    private void ChangeCamera()
    {
        currentCamera = (currentCamera + 1) % cameraPositions.Length;
    }
}
