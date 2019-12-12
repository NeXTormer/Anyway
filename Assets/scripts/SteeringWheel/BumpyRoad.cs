using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpyRoad : MonoBehaviour {

    public int effectMagnitude = 50;

    private void OnTriggerEnter(Collider other)
    {
        //if(other.GetComponent<PlayerInputManager>().enabled)
        {
            if(LogitechGSDK.LogiIsConnected(0))
            {
                float speed = other.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude * 3.6f;
                int period = (int) (speed / 80.0f);
                //Debug.Log("PerioD:" + period);
                //LogitechGSDK.LogiPlaySurfaceEffect(0, LogitechGSDK.LOGI_PERIODICTYPE_TRIANGLE, effectMagnitude, period);
                LogitechGSDK.LogiPlayBumpyRoadEffect(0, effectMagnitude);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.GetComponent<PlayerInputManager>().enabled)
        {
            if (LogitechGSDK.LogiIsConnected(0))
            {
                //LogitechGSDK.LogiStopSurfaceEffect(0);
                LogitechGSDK.LogiStopBumpyRoadEffect(0);            
            }
        }
    }
}
