using UnityEngine;
using System.Collections;
using InputHandling;
using MenusHandler;

public class MultiplayerModesMenu : Menu
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void PlayOffline()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void PlayOnline()
    {
        MenusManager.Instance.ShowMenu("OnlineOptionsMenu");
    }

    public void GoBack()
    {
        MenusManager.Instance.ShowMenu("MainMenu");
    }
}
