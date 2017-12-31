using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

[AddComponentMenu("RaceManager/RaceUI")]
[RequireComponent(typeof(NetworkPlayerData))]
public class RaceUI : MonoBehaviour {

    public Text racetext;
    public Text titleText;

    private GameObject player;
    private StringBuilder sb;
    private NetworkPlayerData data;

    private int m_OldLap = -1;
    private float m_LapStartTime = 0;

    void Awake()
    {
        sb = new StringBuilder(60);
        player = this.transform.root.gameObject;
        data = player.GetComponent<NetworkPlayerData>();
    }


    void LateUpdate()
    {
        if(data != null)
        {
            UpdateText();

            if (data.currentLap != m_OldLap && data.raceActive)
            {
                Debug.Log("PETERSCHUB: " + m_OldLap + " | " + data.currentLap);

                m_OldLap = data.currentLap;

                if (m_LapStartTime != 0)
                {
                    float laptime = Time.time - m_LapStartTime;

                    WWW get = new WWW("http://faoiltiarna.ddns.net/addscore/filavandrel/" + PlayerDataTransfer.instance.playerName + "/" + laptime + "/anyway");
                    Debug.Log("WWW: " + PlayerDataTransfer.instance.playerName + ", " + laptime);
                }

                m_LapStartTime = Time.time;
            }

        }
    }

    public void UpdateText()
    {
        /* TODO: performance? */
        sb = new StringBuilder(70);
        sb.Append("Name: ");
        sb.AppendLine(PlayerDataTransfer.instance.playerName);
        sb.Append("Waypoint: ");
        sb.Append(data.currentWaypoint);
        sb.Append(" / ");
        sb.Append(data.numberOfWaypoints);
        sb.Append("\nLap: ");
        sb.Append(data.currentLap);
        sb.Append(" / ");
        sb.Append(data.numberOfLaps);
        sb.Append("\n");
        sb.Append("Time: ");
        sb.Append(data.raceTime.ToString("0.00"));

        racetext.text = sb.ToString();

        titleText.text = data.titleText;
    }
}
