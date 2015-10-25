using UnityEngine;
using System.Collections;

namespace InputHandling
{
    // Specific to the game
    public class CharacterSelectionContext : InputContext
    {
        // TODO: Pull that data from a config or XML file for easy configuration and load the file in InputContext instead of having inheritance
        // TODO: Maybe divide ranges between Triggers (1 float value) and sticks (2 float values) ?

        public CharacterSelectionContext()
            : base()
        {
            _mappedButtons.Add(RawInputConstants.Buttons.Button1, InputConstants.START_PLAYING);
            _mappedButtons.Add(RawInputConstants.Buttons.Button2, InputConstants.CHOOSE_CHARACTER);

            _mappedAxis.Add(RawInputConstants.Axis.Axis1, InputConstants.MOVE_X);
            _mappedAxis.Add(RawInputConstants.Axis.Axis2, InputConstants.MOVE_Y);
            _mappedAxis.Add(RawInputConstants.Axis.Axis3, InputConstants.ROTATE_X);
            _mappedAxis.Add(RawInputConstants.Axis.Axis4, InputConstants.ROTATE_Y);
            _mappedAxis.Add(RawInputConstants.Axis.Axis5, InputConstants.BREATHE);
            _mappedAxis.Add(RawInputConstants.Axis.Axis6, InputConstants.BREATHE);
        }
    }
}
