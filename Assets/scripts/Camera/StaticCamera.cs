using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : CameraController
{

    private Vector3 offset;

    public StaticCamera(CameraManager manager, GameObject p, CameraPosition origin)
        : base(manager, p, origin)
    { }

    public override void Configure()
    {
        base.Configure();
        offset = player.transform.position - cameraOrigin.transform.position;
    }

    public override void Update()
    {
        base.Update();

        transform.position = player.transform.position + offset;

    }
}
