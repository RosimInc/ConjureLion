using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class CharacterSelector : MonoBehaviour
{
    public SelectorCursor Player1Cursor;
    public SelectorCursor Player2Cursor;
    public GameObject StartButton;

    private bool _canStartGame = false;

    void Awake()
    {
        
    }

    void Update()
    {
        float player1XAxis = InputManager.Instance.GetInputMovement(1).x;
        float player2XAxis = InputManager.Instance.GetInputMovement(2).x;

        if (player1XAxis == 1f)
        {
            Player1Cursor.MoveRight();
            ShowOrHideStartButton();
        }
        else if (player1XAxis == -1f)
        {
            Player1Cursor.MoveLeft();
            ShowOrHideStartButton();
        }

        if (player2XAxis == 1f)
        {
            Player2Cursor.MoveRight();
            ShowOrHideStartButton();
        }
        else if (player2XAxis == -1f)
        {
            Player2Cursor.MoveLeft();
            ShowOrHideStartButton();
        }

        if (_canStartGame && (InputManager.Instance.GetInputAccept(1) || InputManager.Instance.GetInputAccept(2)))
        {
            if (Player1Cursor.Position == SelectorCursor.CursorPosition.Left)
            {
                GameManager.Instance.SetSouffliPlayerNumber(1);
                GameManager.Instance.SetAspiPlayerNumber(2);
            }
            else
            {
                GameManager.Instance.SetSouffliPlayerNumber(2);
                GameManager.Instance.SetAspiPlayerNumber(1);
            }

            GameManager.Instance.LoadNextLevel();
        }
    }

    private void ShowOrHideStartButton()
    {
        if (Player1Cursor.Position == SelectorCursor.CursorPosition.Left && Player2Cursor.Position == SelectorCursor.CursorPosition.Right
            || Player1Cursor.Position == SelectorCursor.CursorPosition.Right && Player2Cursor.Position == SelectorCursor.CursorPosition.Left)
        {
            StartButton.SetActive(true);
            _canStartGame = true;
        }
        else
        {
            StartButton.SetActive(false);
            _canStartGame = false;
        }
    }
}
