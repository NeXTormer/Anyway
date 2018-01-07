using System.Linq;
using UnityEngine;

[AddComponentMenu("Player/Car Color Changer")]
public class ColorChanger : MonoBehaviour
{
    public NetworkPlayerData networkPlayerData;

    public GameObject[] gameobjects;

    private float m_Timer = 0;

    void FixedUpdate()
    { 
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
