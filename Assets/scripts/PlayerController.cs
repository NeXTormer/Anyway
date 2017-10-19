using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2.0f;
    public float turnspeed = 25;

    public WheelCollider F1;
    public WheelCollider F2;
    public WheelCollider B1;
    public WheelCollider B2;

    private Rigidbody body;
    private float moveInput;
    private float turnInput;

	// Use this for initialization
	void Awake () {
        body = GetComponent<Rigidbody>();
	}

    private void OnEnable()
    {
        body.isKinematic = false;
        turnInput = 0;
        moveInput = 0;
    }

    private void OnDisable()
    {
        body.isKinematic = true;
    }

    // Update is called once per frame
    void Update () {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        //move

        Vector3 delta = this.transform.forward * moveInput * speed * Time.deltaTime;
        body.MovePosition(body.position + delta);

        //turn
        float turnDegrees = turnInput * turnspeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0.0f, turnDegrees, 0.0f);
        body.MoveRotation(body.rotation * rotation);
        

        /*
        float torque = moveInput * speed;
        B1.motorTorque = torque;
        B2.motorTorque = torque;
        F1.motorTorque = torque;
        F2.motorTorque = torque;

        float steer = turnInput * turnspeed;
        F1.steerAngle = steer / 3.4f;
        F2.steerAngle = steer / 3.4f;

    */
    }
}
