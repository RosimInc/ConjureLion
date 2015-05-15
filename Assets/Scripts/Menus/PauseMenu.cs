using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InputHandling;

public class PauseMenu : Menu
{
    void Start()
    {
        InputManager.Instance.AddCallback(PauseMenuCallback);
    }

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

    private void PauseMenuCallback(MappedInput mappedInput)
    {
        if (mappedInput.Actions[ActionsConstants.Actions.ClosePauseMenu])
        {
            // TODO: Call the close by the MenusManager so it can manage its current menu and everything else
            Close();
        }
    }
}
