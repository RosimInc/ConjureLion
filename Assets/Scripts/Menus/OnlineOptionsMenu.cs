using UnityEngine;
using System.Collections;
using InputHandling;
using MenusHandler;

public class OnlineOptionsMenu : Menu
{
    public override void Open()
    {
        gameObject.SetActive(true);
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

    public void GoBack()
    {
        MenusManager.Instance.ShowMenu("MultiplayerModesMenu");
    }
}
