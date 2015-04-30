using UnityEngine;
using System.Collections;

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

    void Update()
    {
        if (InputManager.Instance.GetInputMenuBack())
        {
            MenusManager.Instance.ShowMenu("MainMenu");
        }
    }
}
