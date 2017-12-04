using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataTransfer : MonoBehaviour {

    public string playerName = "";

    public void Start()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }

}
