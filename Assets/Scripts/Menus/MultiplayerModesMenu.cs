using UnityEngine;
using System.Collections;
using InputHandling;

public class MultiplayerModesMenu : Menu
{
    void Start()
    {
        InputManager.Instance.AddCallback(MultiplayerModesMenuCallback);
    }

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

    private void MultiplayerModesMenuCallback(MappedInput mappedInput)
    {
        // TODO: Should be handled in the menu manager

        if (mappedInput.Actions[ActionsConstants.Actions.GoToPreviousMenu])
        {
            MenusManager.Instance.ShowMenu("MainMenu");
        }
    }
}
