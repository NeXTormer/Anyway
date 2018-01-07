using UnityEngine;

/// <summary>
/// Used to transfer settings and username from the main menu over to the game scene.
/// </summary>
public class PlayerDataTransfer : MonoBehaviour {

    public string playerName = "";
    public bool useSteeringWheel = false;
    public bool ffaMode = true;

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
