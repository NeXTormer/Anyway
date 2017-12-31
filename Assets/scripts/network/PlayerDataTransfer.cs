using UnityEngine;

public class PlayerDataTransfer : MonoBehaviour {

    public string playerName = "";
    public bool useSteeringWheel = false;

    public static PlayerDataTransfer instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }

}
