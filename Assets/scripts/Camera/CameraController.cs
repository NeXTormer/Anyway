using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraController {

    protected CameraManager cameraManager;
    protected Transform transform;
    protected Camera camera;
    protected CameraPosition cameraOrigin;

    protected GameObject player;

    protected CameraController(CameraManager manager, GameObject p, CameraPosition origin) {
        cameraManager = manager;
        transform = cameraManager.GetComponent<Transform>();
        camera = cameraManager.GetComponent<Camera>();
        player = p;
        cameraOrigin = origin;
    }

    public virtual void Configure() { }

    public virtual void Update() { }
}
