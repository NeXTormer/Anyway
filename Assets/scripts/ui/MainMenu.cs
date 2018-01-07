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
    
    [Space]

    public InputField iphostname;
    public Text errorText;
    public Text playerInfoText;
    public Text SteeringWheelEnabledText;
    public Text FFAToggleText;
    public InputField username;
    public GameObject DefaultCarPreview;

    private NetworkManager m_Manager;

    private string m_Username = "DefaultUser";

    public void Start()
    {
        m_Manager = NetworkManager.singleton;
    }

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
        PlayerDataTransfer.instance.useSteeringWheel = !PlayerDataTransfer.instance.useSteeringWheel;
        SteeringWheelEnabledText.text = "Steeringwheel: " + (PlayerDataTransfer.instance.useSteeringWheel ? "Enabled" : "Disabled");   
    }

    public void BtnToggleFFA()
    {
        PlayerDataTransfer.instance.ffaMode = !PlayerDataTransfer.instance.ffaMode;
        FFAToggleText.text = "FFA Mode: " + (PlayerDataTransfer.instance.ffaMode ? "Enabled" : "Disabled");
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

        PlayerDataTransfer.instance.playerName = m_Username;
    }

    public void BtnSettings()
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

        if (!m_Manager.IsClientConnected() && !NetworkServer.active && m_Manager.matchMaker == null)
        {
            m_Manager.StartHost();
            m_Manager.ServerChangeScene("map1");
        }
        
    }

    public void JoinGame()
    {
        bool noConnection = (m_Manager.client == null || m_Manager.client.connection == null ||
                             m_Manager.client.connection.connectionId == -1);

        if (!m_Manager.IsClientConnected() && !NetworkServer.active && m_Manager.matchMaker == null)
        {
            if (noConnection)
            {
                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
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
                    m_Manager.networkAddress = ip;
                    m_Manager.networkPort = port;

                    m_Manager.StartClient();
                }
            }
        }
    }

    public void OnFailedToConnect(NetworkConnectionError error)
    {
        errorText.text = error.ToString();
    }
}
