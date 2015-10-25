using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InputHandling;
using MenusHandler;

public class PauseMenu : Menu
{
    public override void Open()
    {
        gameObject.SetActive(true);
        GameManager.Instance.Pause();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Resume();

        // TODO: Retrieve and set the previous context
    }

    public void Restart()
    {
        Close();
        GameManager.Instance.RestartLevel();
    }

    public void GoToMainMenu()
    {
        Close();
        GameManager.Instance.LoadMainMenu();
    }
}
