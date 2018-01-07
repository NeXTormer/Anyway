using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[AddComponentMenu("Network/Network Component Disabler")]
class NetworkComponentDisabler : NetworkBehaviour
{

    [SerializeField]
    private Component[] componentsToDisable;

    [SerializeField]
    private GameObject[] gameObjectsToDisable;

    [SerializeField]
    private bool localPlayer;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            localPlayer = false;
            foreach (Component c in componentsToDisable)
            {
                Destroy(c);
            }

            foreach (GameObject c in gameObjectsToDisable)
            {
                Destroy(c);
            }
        }
        else
        {
            localPlayer = true;
        }
        
    }
   
}