using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public Vector3 rotationSpeed;
	
	void FixedUpdate () {
        this.transform.Rotate(rotationSpeed);
	}
}
