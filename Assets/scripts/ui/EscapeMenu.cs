using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public bool menuActive = false;
    public GameObject menuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuActive = !menuActive;

            menuPanel.SetActive(menuActive);
        }
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        menuActive = false;
    }

    public void Disconnect()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
    }

    public void Exit()
    {
        Application.Quit();
    }
}