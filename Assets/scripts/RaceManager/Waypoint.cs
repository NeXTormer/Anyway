using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public enum WaypointType
{
    Start,
    Finish,
    Waypoint
}

[AddComponentMenu("RaceManager/Waypoint")]
[RequireComponent(typeof(BoxCollider))]
public class Waypoint : NetworkBehaviour
{
    [Header("Settings")]
    [Tooltip("Type of Waypoint. Finish counts as 'next lap' if there are more than one lap")]
    public WaypointType type = WaypointType.Waypoint;

    [Tooltip("Incrementing ID of the waypoint starting at 1. Start and finish waypoints don't need an id.")]
    public int waypointID = 1;

    private RaceManager m_RaceManager;
    private Transform[] m_RespawnPoints;

    [SyncVar]
    private int m_LastRespawnPoint = 0;

	void Start()
    {
        if(isServer)
        {
            m_RaceManager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
        }

        m_RespawnPoints = GetComponentsInChildren<Transform>().Where(x => x.gameObject.name.StartsWith("CP")).ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_RaceManager == null) return;
        if(other.transform.root.gameObject.CompareTag("Player"))
        {
            m_RaceManager.OnWaypointHit(other.transform.root.gameObject, this.gameObject);
        }
    }

    public Transform GetRespawnPoint()
    {
        m_LastRespawnPoint++;
        m_LastRespawnPoint %= (m_RespawnPoints.Length - 1);

        return m_RespawnPoints[m_LastRespawnPoint];
    }

}
