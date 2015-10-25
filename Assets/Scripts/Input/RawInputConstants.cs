using UnityEngine;
using System.Collections;

namespace InputHandling
{
    //TODO: When we will be ready to read raw inputs from a file, we need this to simply be generic "BUTTON_ONE, BUTTON_TWO, etc."

    public class RawInputConstants
    {
        // These buttons will eventually map to controls saved in a file
        public enum Buttons
        {
            Button1,
            Button2,
            Button3,
            Button4,
            Button5,
            Button6,
            Button7,
            Button8,
            Button9,
            Button10,
            Button11,
            Button12,
            Button13,
            Button14,
        }

        public enum Axis
        {
            Axis1,
            Axis2,
            Axis3,
            Axis4,
            Axis5,
            Axis6
        }

        /*
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
        }*/
    }
}
