using UnityEngine;
using System.Collections;

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
        MenusManager.Instance.ShowMenu("MultiplayerModesMenu");
    }

    public void ShowControlsMenu()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
