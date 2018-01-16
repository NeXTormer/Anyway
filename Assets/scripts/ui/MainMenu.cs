using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [Header("Panels")]
    public GameObject LoginPanel;
    public GameObject MainPanel;

    [Header("Camera Positions")]
    public GameObject loginCameraPosition;
    public GameObject mainCameraPosition;

    [Header("Camera")]
    public GameObject camera;

    [Header("Inputs")]
    public InputField iphostname;
    public InputField username;
    public Toggle steeringhweel;
    public Text playerInfoText;

    private NetworkManager m_Manager;

    private string m_Username = "DefaultUser";

    public void Start()
    {
        m_Manager = NetworkManager.singleton;
        PlayerDataTransfer.instance.useSteeringWheel = steeringhweel.isOn;

        camera.transform.position = loginCameraPosition.transform.position;
        camera.transform.rotation = loginCameraPosition.transform.rotation;

        MainPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ChangeSteeringWheel()
    {
        PlayerDataTransfer.instance.useSteeringWheel = steeringhweel.isOn;
    }

    public void BtnLogin()
    {
        m_Username = username.text;
        if(m_Username == "")
        {
            m_Username = "DefaultUser";
        }

        camera.transform.position = mainCameraPosition.transform.position;
        camera.transform.rotation = mainCameraPosition.transform.rotation;

        LoginPanel.SetActive(false);
        MainPanel.SetActive(true);

        playerInfoText.text = "Logged in as " + m_Username;

        PlayerDataTransfer.instance.playerName = m_Username;
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
}
