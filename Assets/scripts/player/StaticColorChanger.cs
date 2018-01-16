using UnityEngine;

public class StaticColorChanger : MonoBehaviour
{
    public GameObject[] gameobjects;

    public Material material;

    void Start()
    {
        foreach (var gameobject in gameobjects)
        {
            var rend = gameobject.GetComponent<MeshRenderer>();
            foreach (var mat in rend.materials)
            {
                if (mat.name.Equals("PriCarColor (Instance)"))
                {
                    mat.color = material.color;
                }
                if (mat.name.Equals("SecCarColor (Instance)"))
                {
                    mat.color = material.color;
                }
            }
        }
    }
}