using UnityEngine;
using System.Collections;

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
    }

    void Update()
    {
        if (InputManager.Instance.GetInputAccept(1) || InputManager.Instance.GetInputAccept(2) || InputManager.Instance.GetInputPauseMenu())
        {
            // TODO: Call the close by the MenusManager so it can manage its current menu and everything else
            Close();
        }
    }
}
