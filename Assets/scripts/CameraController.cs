using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;
    
	void Awake () {
        offset = this.transform.position - player.transform.position;
	}
	
	void FixedUpdate () {
        this.transform.position = player.transform.position + offset;
	}
}
