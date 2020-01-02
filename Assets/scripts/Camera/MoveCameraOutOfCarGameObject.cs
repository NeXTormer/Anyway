using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraOutOfCarGameObject : MonoBehaviour {

    public GameObject camera;

	void Start () {
        camera.transform.SetParent(GameObject.FindGameObjectWithTag("RaceManager").transform);
	}
}
