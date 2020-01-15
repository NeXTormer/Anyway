using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResponse : MonoBehaviour {

    public enum CollisionType
    {
        FRONT, LEFT, RIGHT
    }

    public CollisionType collisionType;

    public void OnTriggerEnter(Collider other)
    {
        Waypoint wp = other.GetComponent<Waypoint>();
        if(wp == null)
        {
            if (LogitechGSDK.LogiIsConnected(0))
            {
                if (collisionType == CollisionType.FRONT)
                {
                    //Debug.Log("FRONT COLLISION");
                    LogitechGSDK.LogiPlayFrontalCollisionForce(0, 100);
                }
                else if (collisionType == CollisionType.LEFT)
                {
                    LogitechGSDK.LogiPlaySideCollisionForce(0, 100);
                    //Debug.Log("LEFT COLLISION");
                }
                else if (collisionType == CollisionType.RIGHT)
                {
                    LogitechGSDK.LogiPlaySideCollisionForce(0, -100);
                    //Debug.Log("RIGHT COLLISION");
                }
            }
        }
        
    }

}
