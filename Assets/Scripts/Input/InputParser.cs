﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

namespace InputHandling
{
    // This is the base class that's going to gather all raw inputs (with the help of XInputDotNet) from the system and parse it to readable booleans and floats
    public class InputParser : MonoBehaviour
    {
        /*
        // Keeping for future configuration with the configuration files

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
    
        public enum Controls { Start, A, B, LeftTrigger, RightTrigger, BothTriggers, LeftStick, RightStick, BothSticks }
        public enum Buttons { Start, A, B, }
        public enum Triggers { LeftTrigger, RightTrigger, BothTriggers }
        public enum Sticks { LeftStick, RightStick }

        public InputMapping Actions;*/




        // TODO: This class should be abstract and the inheriting children should handle which type of input is mapped (keyboard + mouse, controller, etc.)
        // TODO: There should be 1 inheretd class by type of controller that we want to support

        public static InputParser Instance
        {
            get
            {
                return _instance;
            }
        }

        private static InputParser _instance;

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
            // Every frame, we scan all controllers and put all the input data in an InputMapper object

            InputMapper inputMapper = new InputMapper(PLAYER_AMOUNT);

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

                // TODO: Maybe pass direction the game states to the input mapper?

                inputMapper.MapButton(i, InputConstants.Buttons.Start, GetButtonDownState(InputConstants.Buttons.Start, i));
                inputMapper.MapButton(i, InputConstants.Buttons.A, GetButtonDownState(InputConstants.Buttons.A, i));
                inputMapper.MapButton(i, InputConstants.Buttons.B, GetButtonDownState(InputConstants.Buttons.B, i));

                inputMapper.MapAxis(i, InputConstants.Axis.TriggerLeft, GetRangeValue(InputConstants.Axis.TriggerLeft, i));
                inputMapper.MapAxis(i, InputConstants.Axis.TriggerRight, GetRangeValue(InputConstants.Axis.TriggerRight, i));
                inputMapper.MapAxis(i, InputConstants.Axis.LeftStickX, GetRangeValue(InputConstants.Axis.LeftStickX, i));
                inputMapper.MapAxis(i, InputConstants.Axis.LeftStickY, GetRangeValue(InputConstants.Axis.LeftStickY, i));
                inputMapper.MapAxis(i, InputConstants.Axis.TriggerLeft, GetRangeValue(InputConstants.Axis.TriggerLeft, i));
                inputMapper.MapAxis(i, InputConstants.Axis.TriggerRight, GetRangeValue(InputConstants.Axis.TriggerRight, i));
            }

            inputMapper.Dispatch();
        }

        #region Helper Methods

        private bool GetButtonDownState(InputConstants.Buttons button, int playerIndex)
        {
            bool isActivated = false;

            GamePadState previousState = _gamePadPreviousStates[playerIndex];
            GamePadState state = _gamePadStates[playerIndex];

            // TODO: Pull that data from a config or XML file for easy configuration

            // TODO: Fill in the remaining buttons

            switch (button)
            {
                case InputConstants.Buttons.A:
                    isActivated = previousState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed;
                    break;
                case InputConstants.Buttons.B:
                    isActivated = previousState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed;
                    break;
                case InputConstants.Buttons.Back:
                    break;
                case InputConstants.Buttons.Guide:
                    break;
                case InputConstants.Buttons.LeftShoulder:
                    break;
                case InputConstants.Buttons.LeftStick:
                    break;
                case InputConstants.Buttons.RightShoulder:
                    break;
                case InputConstants.Buttons.RightStick:
                    break;
                case InputConstants.Buttons.Start:
                    isActivated = previousState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed;
                    break;
                case InputConstants.Buttons.X:
                    break;
                case InputConstants.Buttons.Y:
                    break;
                case InputConstants.Buttons.DPadLeft:
                    break;
                case InputConstants.Buttons.DPadUp:
                    break;
                case InputConstants.Buttons.DPadRight:
                    break;
                case InputConstants.Buttons.DPadDown:
                    break;
            }

            return isActivated;
        }

        private float GetRangeValue(InputConstants.Axis range, int playerIndex)
        {
            float value = 0f;

            GamePadState state = _gamePadStates[playerIndex];

            // TODO: Pull that data from a config or XML file for easy configuration

            switch (range)
            {
                case InputConstants.Axis.LeftStickX:
                    value = state.ThumbSticks.Left.X;
                    break;
                case InputConstants.Axis.LeftStickY:
                    value = state.ThumbSticks.Left.Y;
                    break;
                case InputConstants.Axis.RightStickX:
                    value = state.ThumbSticks.Right.X;
                    break;
                case InputConstants.Axis.RightStickY:
                    value = state.ThumbSticks.Right.Y;
                    break;
                case InputConstants.Axis.TriggerLeft:
                    value = state.Triggers.Left;
                    break;
                case InputConstants.Axis.TriggerRight:
                    value = state.Triggers.Right;
                    break;
            }

            //value = -(Mathf.Atan2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y) * Mathf.Rad2Deg);

            return value;
        }

        #endregion
    }
}