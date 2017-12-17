using UnityEngine;
using System.Linq;

class CarResetToCheckpoint : MonoBehaviour
{
    private PlayerInputManager m_InputManager;
    private Waypoint[] m_Waypoints;
    private NetworkPlayerData m_PlayerData;


    public void Start()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("Waypoint").OrderBy(x => x.name).ToArray();
        m_Waypoints = new Waypoint[tmp.Length];

        for(int i = 0; i < tmp.Length; i++)
        {
            m_Waypoints[i] = tmp[i].GetComponent<Waypoint>();
        }
        m_PlayerData = this.transform.gameObject.GetComponent<NetworkPlayerData>();
        m_InputManager = this.transform.gameObject.GetComponent<PlayerInputManager>();
    }

    public void FixedUpdate()
    {
        if(m_InputManager.ResetCar())
        {
            if (m_PlayerData.raceActive)
            {
                int currwp = m_PlayerData.currentWaypoint;
                int currlap = m_PlayerData.currentLap;
                if (currwp == 0)
                {
                    if (currlap != -1)
                    {
                        Reset(0);
                    }
                }
                else
                {
                    Reset(currwp);
                }
            }
        }
    }

    private void Reset(int cp)
    {
        Transform tr;

        if(cp == 0)
        {
            tr = m_Waypoints[0].GetRespawnPoint();
        }
        else
        {
            tr = m_Waypoints[cp + 1].GetRespawnPoint();
        }

        this.gameObject.transform.position = tr.position;
        this.transform.rotation = tr.rotation;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

    }
}