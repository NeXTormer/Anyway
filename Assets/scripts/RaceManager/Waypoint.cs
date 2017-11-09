using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointType
{
    Start,
    Finish,
    Waypoint
}

[RequireComponent(typeof(BoxCollider))]
public class Waypoint : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Type of Waypoint. Finish counts as 'next lap' if there are more than one lap")]
    public WaypointType type = WaypointType.Waypoint;

    [Tooltip("Incrementing ID of the waypoint starting at 1. Start and finish waypoints don't need an id.")]
    public int waypointID = 1;

    private RaceManager raceManager;

	void Start()
    {
        raceManager = GetComponentInParent<RaceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            raceManager.OnWaypointHit(other.gameObject, this.gameObject);
        }
    }

}
