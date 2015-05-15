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
    private InputParser _inputParser;

    void Awake()
    {
        _instance = this;
        _inputParser = new InputParser();
    }

    void Update()
    {
        _inputParser.ParseRawInput();
    }

    public void AddCallback(Action<MappedInput> action)
    {
        _inputParser.InputMapper.AddCallback(action);
    }

    public void SetContext(InputContext context)
    {
        _inputParser.InputMapper.SetContext(context);
    }

    void LateUpdate()
    {
        _inputParser.InputMapper.ResetInputs();
    }
}
