using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuCanvas;
    public GameObject SingleplayerCanvas;
    public GameObject MultiplayerCanvas;

    public InputField iphostname;
    public Text errorText;
    

    public NetworkManager manager;


    public void BtnSingleplayer()
    {
        MainMenuCanvas.SetActive(false);
        SingleplayerCanvas.SetActive(true);
    }

    public void BtnMultiplayer()
    {
        MainMenuCanvas.SetActive(false);
        MultiplayerCanvas.SetActive(true);
    }

    public void BtnTempScene()
    {
        SceneManager.LoadScene("temp", LoadSceneMode.Single);
    }

    public void BtnLvl1()
    {
        SceneManager.LoadScene("map1", LoadSceneMode.Single);
    }

    public void BtnExit()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SingleplayerCanvas.SetActive(false);
        MultiplayerCanvas.SetActive(false);

        MainMenuCanvas.SetActive(true);
    }

    public void HostAndPlay()
    {
        Debug.Log("Host and Play");
        manager.StartHost();
        
    }

    public void ConnectToServer()
    {
        string input = iphostname.text;
        string ip;
        int port;

        if (input == "")
        {
            ip = "localhost";
            port = 7777;
        }
        else
        {
            string[] temp = input.Split(':');
            ip = temp[0];
            port = int.Parse(temp[1]);
        }
        manager.networkAddress = ip;
        manager.networkPort = port;

        manager.StartClient();
    }

    public void OnFailedToConnect(NetworkConnectionError error)
    {
        errorText.text = error.ToString();
    }
}
