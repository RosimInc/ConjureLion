using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;
using InputHandling;

public class InputManager : MonoBehaviour
{
    /*
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

    #region Inspector Values

    public enum Controls { Start, A, B, LeftTrigger, RightTrigger, BothTriggers, LeftStick, RightStick, BothSticks }
    public enum Buttons { Start, A, B, }
    public enum Triggers { LeftTrigger, RightTrigger, BothTriggers }
    public enum Sticks { LeftStick, RightStick }

    public InputMapping Actions;

    #endregion
    */

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static InputManager _instance;

    // TODO: This class should be abstract and the inheriting children should handle which type of input is mapped (keyboard + mouse, controller, etc.)
    // TODO: There should be 1 inheretd class by type of controller that we want to support

    private bool[] _initialSetupDone;
    private PlayerIndex[] _playerIndexes;
    private GamePadState[] _gamePadPreviousStates;
    private GamePadState[] _gamePadStates;

    private InputMapper[] _inputMappers;

    private const int PLAYER_AMOUNT = 2;

    void Awake()
    {
        _instance = this;

        _initialSetupDone = new bool[PLAYER_AMOUNT];
        _playerIndexes = new PlayerIndex[PLAYER_AMOUNT];
        _gamePadPreviousStates = new GamePadState[PLAYER_AMOUNT];
        _gamePadStates = new GamePadState[PLAYER_AMOUNT];
        _inputMappers = new InputMapper[PLAYER_AMOUNT];

        for (int i = 0; i < PLAYER_AMOUNT; i++)
        {
            _gamePadStates[i] = GamePad.GetState(_playerIndexes[i]);
            _inputMappers[i] = new InputMapper();
        }
    }

    void Update()
    {
        // Every frame, we scan all controllers and put all the input data in an InputMapper object

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

            // TODO: Map the correct input depending on what controller the player has chosen
            // TODO: Make it more generic by not having the rawinputconstants buttons named after the xbox controller (use BUTTON_ONE, BUTTON_TWO, etc. instead)

            GamePadState previousState = _gamePadPreviousStates[i];
            GamePadState state = _gamePadStates[i];

            _inputMappers[i].SetRawButtonState(
                RawInputConstants.Buttons.Button1,
                state.Buttons.Start == ButtonState.Pressed,
                previousState.Buttons.Start == ButtonState.Pressed
            );

            _inputMappers[i].SetRawButtonState(
                RawInputConstants.Buttons.Button2,
                state.Buttons.A == ButtonState.Pressed,
                previousState.Buttons.A == ButtonState.Pressed
            );

            _inputMappers[i].SetRawButtonState(
                RawInputConstants.Buttons.Button3,
                state.Buttons.B == ButtonState.Pressed,
                previousState.Buttons.B == ButtonState.Pressed
            );

            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis1, state.ThumbSticks.Left.X);
            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis2, state.ThumbSticks.Left.Y);
            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis3, state.ThumbSticks.Right.X);
            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis4, state.ThumbSticks.Right.Y);
            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis5, state.Triggers.Left);
            _inputMappers[i].SetRawAxisValue(RawInputConstants.Axis.Axis6, state.Triggers.Right);

            _inputMappers[i].Dispatch();

            //value = -(Mathf.Atan2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y) * Mathf.Rad2Deg);
        }

        
    }

    public void AddCallback(int playerIndex, Action<MappedInput> action)
    {
        _inputMappers[playerIndex].AddCallback(action);
    }

    public void PushContext(InputContext context)
    {
        // For now, all input mappers are gonna have the same contexts at the same time

        for (int i = 0; i < _inputMappers.Length; i++)
        {
            _inputMappers[i].PushContext(context);
        }
    }

    public void PopContext()
    {
        // For now, all input mappers are gonna have the same contexts at the same time

        for (int i = 0; i < _inputMappers.Length; i++)
        {
            _inputMappers[i].PopContext();
        }
    }

    public void ClearContexts()
    {
        // For now, all input mappers are gonna have the same contexts at the same time

        for (int i = 0; i < _inputMappers.Length; i++)
        {
            _inputMappers[i].ClearContexts();
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < _inputMappers.Length; i++)
        {
            _inputMappers[i].ResetInputs();
        }
    }
}
