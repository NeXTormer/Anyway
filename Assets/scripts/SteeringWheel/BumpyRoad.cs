using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpyRoad : MonoBehaviour {

    public int effectMagnitude = 50;
    public static bool isInCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<PlayerInputManager>().enabled)
        {
            isInCollider = true;
            //if(LogitechGSDK.LogiIsConnected(0))
            {
                Debug.Log("Trigger Enter");
                float speed = other.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude * 3.6f;
                int period = (int) (speed / 80.0f);
                //Debug.Log("PerioD:" + period);
                //LogitechGSDK.LogiPlaySurfaceEffect(0, LogitechGSDK.LOGI_PERIODICTYPE_TRIANGLE, effectMagnitude, period);
                LogitechGSDK.LogiPlayBumpyRoadEffect(0, effectMagnitude);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<PlayerInputManager>().enabled)
        {
            if(!LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
            {
                LogitechGSDK.LogiPlayBumpyRoadEffect(0, effectMagnitude);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<PlayerInputManager>().enabled)
        {
            isInCollider = false;
            //if (LogitechGSDK.LogiIsConnected(0))
            {
                //LogitechGSDK.LogiStopSurfaceEffect(0);
                Debug.Log("Trigger Exit");
                LogitechGSDK.LogiStopBumpyRoadEffect(0);            
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isInCollider)
        {
            LogitechGSDK.LogiStopBumpyRoadEffect(0);
        }
    }

}
