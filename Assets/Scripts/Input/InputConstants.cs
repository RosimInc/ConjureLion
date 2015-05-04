using UnityEngine;
using System.Collections;

namespace InputHandling
{
    public class InputConstants
    {
        public enum Buttons
        {
            A,
            B,
            Back,
            Guide,
            LeftShoulder,
            LeftStick,
            RightShoulder,
            RightStick,
            Start,
            X,
            Y,
            DPadLeft,
            DPadUp,
            DPadRight,
            DPadDown
        }

        public enum Axis
        {
            LeftStickX,
            LeftStickY,
            RightStickX,
            RightStickY,
            TriggerLeft,
            TriggerRight
        }
    }
}
