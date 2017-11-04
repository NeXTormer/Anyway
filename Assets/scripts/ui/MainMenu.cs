using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void BtnTempScene()
    {
        SceneManager.LoadScene("temp", LoadSceneMode.Single);
    }

    public void BtnLvl1()
    {
        SceneManager.LoadScene("map1", LoadSceneMode.Single);
    }
}
