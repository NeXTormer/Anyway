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
    public GameObject LoginCanvas;
    public GameObject SettingsCanvas;
    public GameObject SelectCarCanvas;

    public InputField iphostname;
    public Text errorText;
    public Text playerInfoText;
    public Text SteeringWheelEnabledText;
    public InputField username;
    public GameObject DefaultCarPreview;

    public NetworkManager manager;

    private string m_Username = "DefaultUser";
    private bool m_SteeringWheelActive = false;

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

    public void BtnBackToMultiplayer()
    {
        SettingsCanvas.SetActive(false);
        MultiplayerCanvas.SetActive(true);
    }

    public void BtnToggleSteeringWheel()
    {
        m_SteeringWheelActive = !m_SteeringWheelActive;
        SteeringWheelEnabledText.text = "SteeringWheel: " + (m_SteeringWheelActive ? "1" : "0");   
    }

    public void BtnNextCar()
    {

    }

    public void BtnPreviousCar()
    {

    }

    public void BtnSelectCar()
    {
        MultiplayerCanvas.SetActive(false);
        SelectCarCanvas.SetActive(true);
        DefaultCarPreview.transform.rotation = Quaternion.identity;
        DefaultCarPreview.transform.Rotate(new Vector3(0, -100, 0));
    }

    public void BtnLogin()
    {
        m_Username = username.text;
        if(m_Username == "")
        {
            m_Username = "DefaultUser";
        }

        LoginCanvas.SetActive(false);
        MultiplayerCanvas.SetActive(true);

        playerInfoText.text = "Logged in as " + m_Username;
    }

    public void btnSettings()
    {
        MultiplayerCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
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
        manager.ServerChangeScene("temp");
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
            if (temp.Length == 1)
            {
                ip = temp[0];
                port = 7777;
            }
            else
            {
                ip = temp[0];
                port = int.Parse(temp[1]);
            }
        }
        manager.networkAddress = "localhost";
        manager.networkPort = 7777;
        manager.StartClient();
    }

    public void OnFailedToConnect(NetworkConnectionError error)
    {
        errorText.text = error.ToString();
    }
}
