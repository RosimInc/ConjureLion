using UnityEngine;
using System.Collections;
using MenusHandler;

public class MainMenu : Menu
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void ShowMultiplayerModes()
    {
        GameManager.Instance.LoadNextLevel();

        // Removed for demo only

        //MenusManager.Instance.ShowMenu("MultiplayerModesMenu");
    }

    public void ShowControlsMenu()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
