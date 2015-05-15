using UnityEngine;
using System.Collections;
using InputHandling;

public class OnlineOptionsMenu : Menu
{
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    void Start()
    {
        InputManager.Instance.AddCallback(OnlineOptionsMenuCallback);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public void CreateGame()
    {
        // TODO: Add a message "Waiting for player..." while the player is in the character selection screen
        // TODO: Change the character selection screen to network mode
        NetworkManager.Instance.CreateGame(GameManager.Instance.LoadNextLevel);
    }

    public void JoinGame()
    {
        MenusManager.Instance.ShowMenu("OnlineGamesBrowser");
    }

    private void OnlineOptionsMenuCallback(MappedInput mappedInput)
    {
        // TODO: Should be handled in the menu manager

        if (mappedInput.Actions[ActionsConstants.Actions.GoToPreviousMenu])
        {
            MenusManager.Instance.ShowMenu("MultiplayerModesMenu");
        }
    }
}
