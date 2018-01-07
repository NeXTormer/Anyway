using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[AddComponentMenu("Network/Username")]
public class Username : NetworkBehaviour
{
    [SyncVar] private string m_Username = "[undef]";

    public string PlayerName
    {
        get
        {
            return m_Username;
        }
        set
        {
            m_Username = value;
            Debug.Log("Updated Username to " + value);
        }
    }
}
