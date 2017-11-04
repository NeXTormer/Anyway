using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CameraType
{
    Static,
    MouseControlled,
    SmoothFollowing
}


public class CameraPosition : MonoBehaviour
{
    [Header("Settings")]
    public CameraType cameraType = CameraType.Static;

    [HideInInspector]
    public CameraController controller;

	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
