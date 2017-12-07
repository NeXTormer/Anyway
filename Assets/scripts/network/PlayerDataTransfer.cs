using UnityEngine;

public class PlayerDataTransfer : MonoBehaviour {

    public static PlayerDataTransfer instance = null;

    public string playerName = "";
    public bool useSteeringWheel = false;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }

}
