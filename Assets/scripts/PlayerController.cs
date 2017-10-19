using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2.0f;
    public float turnspeed = 180;

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

    }
}
