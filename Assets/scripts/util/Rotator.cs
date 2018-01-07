using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Rotator")]
public class Rotator : MonoBehaviour {

    public Vector3 rotationSpeed;
	
	void FixedUpdate () {
        this.transform.Rotate(rotationSpeed);
	}
}
