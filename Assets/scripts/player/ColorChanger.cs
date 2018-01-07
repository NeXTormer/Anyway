﻿using System.Linq;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public NetworkPlayerData networkPlayerData;

    public GameObject[] gameobjects;

    private float m_Timer = 0;

    void FixedUpdate()
    {
        //if (networkPlayerData.raceTime < 10)
        {
            //m_Timer += Time.deltaTime;
            foreach (var gameobject in gameobjects)
            {
                var rend = gameobject.GetComponent<MeshRenderer>();
                foreach (var mat in rend.materials)
                {
                    if (mat.name.Equals("PriCarColor (Instance)"))
                    {
                        mat.color = networkPlayerData.color;
                    }
                    if (mat.name.Equals("SecCarColor (Instance)"))
                    {
                        mat.color = networkPlayerData.color;
                    }
                }
            }

        }

    }
    


}
