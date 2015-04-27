using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

public class InputManager : MonoBehaviour
{
    [Serializable]
    public struct InputMapping
    {
        public Buttons PauseMenu;
        public Buttons Accept;
        public Buttons Back;
        public Triggers BreathAction;
        public Sticks Move;
        public Sticks Rotate;
    }

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    #region Inspector Values

    public enum Controls { Start, A, B, LeftTrigger, RightTrigger, BothTriggers, LeftStick, RightStick, BothSticks }
    public enum Buttons { Start, A, B, }
    public enum Triggers { LeftTrigger, RightTrigger, BothTriggers }
    public enum Sticks { LeftStick, RightStick }

    public InputMapping Actions;

    #endregion

    private static InputManager _instance;

    private bool[] _initialSetupDone;
    private PlayerIndex[] _playerIndexes;
    private GamePadState[] _gamePadPreviousStates;
    private GamePadState[] _gamePadStates;

    private const int PLAYER_AMOUNT = 2;

    void Awake()
    {
        _instance = this;

        _initialSetupDone = new bool[PLAYER_AMOUNT];
        _playerIndexes = new PlayerIndex[PLAYER_AMOUNT];
        _gamePadPreviousStates = new GamePadState[PLAYER_AMOUNT];
        _gamePadStates = new GamePadState[PLAYER_AMOUNT];

        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            _gamePadStates[i] = GamePad.GetState(_playerIndexes[i]);
        }
    }

    void Update()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            _gamePadPreviousStates[i] = _gamePadStates[i];
            _gamePadStates[i] = GamePad.GetState(_playerIndexes[i]);

            if (!_gamePadPreviousStates[i].IsConnected || !_initialSetupDone[i])
            {
                _initialSetupDone[i] = true;

                if (_gamePadStates[i].IsConnected)
                {
                    _playerIndexes[i] = (PlayerIndex)i;

                    Debug.Log(string.Format("GamePad {0} is ready", _playerIndexes[i]));
                }
            }
        }
    }

    #region Public Methods

    public bool GetInputPauseMenu()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            if (GetButtonUpState(Actions.PauseMenu, i))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetInputAccept(int playerNumber)
    {
        return GetButtonUpState(Actions.Accept, playerNumber - 1);
    }

    public bool GetInputBack(int playerNumber)
    {
        return GetButtonUpState(Actions.Back, playerNumber - 1);
    }

    public float GetInputBreathAction(int playerNumber)
    {
        return GetTriggerValue(Actions.BreathAction, playerNumber - 1);
    }

    public Vector2 GetInputMovement(int playerNumber)
    {
        return new Vector2(GetXStickValue(Actions.Move, playerNumber - 1), GetYStickValue(Actions.Move, playerNumber - 1));
    }

    public Vector2 GetInputRotation(int playerNumber)
    {
        return new Vector2(GetXStickValue(Actions.Rotate, playerNumber - 1), GetYStickValue(Actions.Rotate, playerNumber - 1));
    }

    public bool GetInputMenuUp()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            if (GetYStickValue(Actions.Move, i) > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    public bool GetInputMenuDown()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            if (GetYStickValue(Actions.Move, i) < -0.5f)
            {
                return true;
            }
        }

        return false;
    }

    public bool GetInputMenuAccept()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            if (GetButtonDownState(Actions.Accept, i))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetInputMenuBack()
    {
        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            if (GetButtonDownState(Actions.Back, i))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Helper Methods

    private bool GetButtonDownState(Buttons button, int playerIndex)
    {
        bool isActivated = false;

        GamePadState previousState = _gamePadPreviousStates[playerIndex];
        GamePadState state = _gamePadStates[playerIndex];

        switch (button)
        {
            case Buttons.Start:
                isActivated = previousState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed;
                break;
            case Buttons.A:
                isActivated = previousState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed;
                break;
            case Buttons.B:
                isActivated = previousState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed;
                break;
        }

        return isActivated;
    }

    private bool GetButtonState(Buttons button, int playerIndex)
    {
        bool isActivated = false;

        GamePadState previousState = _gamePadPreviousStates[playerIndex];
        GamePadState state = _gamePadStates[playerIndex];

        switch (button)
        {
            case Buttons.Start:
                isActivated = previousState.Buttons.Start == ButtonState.Pressed;
                break;
            case Buttons.A:
                isActivated = previousState.Buttons.A == ButtonState.Pressed;
                break;
            case Buttons.B:
                isActivated = previousState.Buttons.B == ButtonState.Pressed;
                break;
        }

        return isActivated;
    }

    private bool GetButtonUpState(Buttons button, int playerIndex)
    {
        bool isActivated = false;

        GamePadState previousState = _gamePadPreviousStates[playerIndex];
        GamePadState state = _gamePadStates[playerIndex];

        switch (button)
        {
            case Buttons.Start:
                isActivated = previousState.Buttons.Start == ButtonState.Pressed && state.Buttons.Start == ButtonState.Released;
                break;
            case Buttons.A:
                isActivated = previousState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released;
                break;
            case Buttons.B:
                isActivated = previousState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released;
                break;
        }

        return isActivated;
    }

    private float GetTriggerValue(Triggers trigger, int playerIndex)
    {
        float value = 0f;

        GamePadState state = _gamePadStates[playerIndex];

        switch (trigger)
        {
            case Triggers.LeftTrigger:
                value = state.Triggers.Left;
                break;
            case Triggers.RightTrigger:
                value = state.Triggers.Right;
                break;
            case Triggers.BothTriggers:
                value = Mathf.Max(state.Triggers.Left, state.Triggers.Right);
                break;
        }

        return value;
    }

    private float GetXStickValue(Sticks stick, int playerIndex)
    {
        float value = 0f;

        GamePadState state = _gamePadStates[playerIndex];

        switch (stick)
        {
            case Sticks.LeftStick:
                value = state.ThumbSticks.Left.X;
                break;
            case Sticks.RightStick:
                value = state.ThumbSticks.Right.X;
                break;
        }

        return value;
    }

    private float GetYStickValue(Sticks stick, int playerIndex)
    {
        float value = 0f;

        GamePadState state = _gamePadStates[playerIndex];

        switch (stick)
        {
            case Sticks.LeftStick:
                value = state.ThumbSticks.Left.Y;
                break;
            case Sticks.RightStick:
                value = state.ThumbSticks.Right.Y;
                break;
        }

        return value;
    }

    #endregion
}
